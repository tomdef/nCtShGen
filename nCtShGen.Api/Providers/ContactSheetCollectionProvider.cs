using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Security;
using Microsoft.Extensions.Configuration;
using nCtShGen.Api.Model;
using nCtShGen.Api.Model.Events;

namespace nCtShGen.Api.Providers;

public class ContactSheetCollectionProvider
{

    private readonly MacroProvider macroProvider;
    private readonly ContactSheetProvider csProvider;
    private readonly ConfigurationItem configuration;

    public event EventHandler<EventArgs> OnStart = default!;
    public event EventHandler<ContactSheetCollectionSummaryEventArgs> OnFinish = default!;
    public event EventHandler<ContactSheetCollectionErrorEventArgs> OnError = default!;
    public event EventHandler<ContactSheetCollectionGenericEventArgs> OnWarning = default!;
    public event EventHandler<ContactSheetCollectionGenericEventArgs> OnScanFolder = default!;
    public event EventHandler<ContactSheetCollectionGenericEventArgs> OnSkipSaveContactSheet = default!;
    public event EventHandler<ContactSheetCollectionGenericEventArgs> OnOverrideContactSheet = default!;
    public event EventHandler<ContactSheetCollectionGenericEventArgs> OnSaveContactSheet = default!;
    public event EventHandler<ContactSheetCollectionGenericEventArgs> OnResolveContactSheetOutput = default!;
    public event EventHandler<ContactSheetCollectionGenericEventArgs> OnCreateDirectoryBeforeSaveContactSheet = default!;

    public event EventHandler<ContactSheetEventArgs> OnStartGenerateContactSheet = default!;
    public event EventHandler<ContactSheetEventArgs> OnFinishGenerateContactSheet = default!;
    public event EventHandler<ContactSheetItemEventArgs> OnAddContactSheetItem = default!;
    public event EventHandler<ContactSheetItemWarningEventArgs> OnWarningContactSheetItem = default!;

    public string DisplayTextOnAskOverride { get; set; } = " Override existing ContactSheet [{0}/{1}] ? ";
    public char AskOverrideYesChar { get; set; } = 'Y';
    public char AskOverrideNoChar { get; set; } = 'n';

    public ContactSheetCollectionProvider(ConfigurationItem configuration)
    {
        this.configuration = configuration;

        macroProvider = new();
        macroProvider.Init();

        csProvider = new(configuration, Api.Model.ColorSchemaName.Dark);

        csProvider.OnStartGenerateContactSheet += (o, e) => OnStartGenerateContactSheet(o, e);
        csProvider.OnFinishGenerateContactSheet += (o, e) => OnFinishGenerateContactSheet(o, e);
        csProvider.OnAddContactSheetItem += (o, e) => OnAddContactSheetItem(o, e);
        csProvider.OnWarningContactSheetItem += (o, e) => OnWarningContactSheetItem(o, e);
    }

    public void Generate(string inputFolder, string outputFolder)
    {
        OnStart?.Invoke(this, new EventArgs());

        int contactSheetsCounter = 0;
        Stopwatch sw = new();
        sw.Start();

        EnumerationOptions eo = new()
        {
            RecurseSubdirectories = true,
            ReturnSpecialDirectories = false,
            IgnoreInaccessible = true,
            MaxRecursionDepth = configuration.ContactSheetSubfolderDeepLevel
        };

        try
        {
            inputFolder = Path.GetFullPath(inputFolder);

            string[] folders = new string[] { inputFolder };
            string[] subDirFolders = Directory.GetDirectories(inputFolder, "*", eo);
            string[] excludeFolders = configuration.ExcludeFolders.Split(';', StringSplitOptions.RemoveEmptyEntries);
            folders = folders.Concat(subDirFolders)
                .Except(excludeFolders)
                .Except(new string[] { outputFolder })
                .ToArray();

            foreach (string folder in folders)
            {
                string[] levels = folder
                    .Replace(inputFolder, "")
                    .Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                int levelsCount = levels.Length;
                if ((levelsCount >= 0) && (levelsCount == configuration.ContactSheetRootFolderOnLevel))
                {
                    contactSheetsCounter++;
                    string contactSheetRootFolder = inputFolder;
                    for (int x = 0; x < configuration.ContactSheetRootFolderOnLevel; x++)
                    {
                        contactSheetRootFolder = Path.Combine(contactSheetRootFolder, levels[x]);
                    }

                    OnScanFolder?.Invoke(this, new ContactSheetCollectionGenericEventArgs(contactSheetRootFolder));

                    DirectoryInfo di = new(contactSheetRootFolder);

                    macroProvider.Set(MacroName.rootFolderPath, contactSheetRootFolder);
                    macroProvider.Set(MacroName.currentFolderPath, folder);
                    macroProvider.Set(MacroName.currentFolderName, di.Name);
                    macroProvider.Set(MacroName.outputFolderPath, outputFolder);
                    macroProvider.Set(MacroName.counter, contactSheetsCounter);
                    macroProvider.SetValueFor(DateTime.Now, MacroName.date, MacroName.day, MacroName.month, MacroName.year);

                    string title = di.Name;
                    string path = macroProvider.Resolve(
                        Path.Combine(configuration.ContactSheetFolder, configuration.ContactSheetFileNameTemplate));

                    FileInfo fi = new(path);

                    OnResolveContactSheetOutput?.Invoke(this, new ContactSheetCollectionGenericEventArgs(path));

                    if ((fi.Exists == true) && (configuration.ContactSheetExistsAction == ExistsAction.Skip))
                    {
                        OnSkipSaveContactSheet?.Invoke(this,
                            new ContactSheetCollectionGenericEventArgs(fi.FullName, "File exists and ExistsAction is `Skip`"));
                        continue;
                    }

                    var image = csProvider.GenerateContactSheet(title, di.FullName);
                    if (image != null)
                    {
                        if ((fi.Exists == true) && (configuration.ContactSheetExistsAction == ExistsAction.Ask))
                        {
                            Console.WriteLine();
                            ConsoleColor foregroundColor = Console.ForegroundColor;
                            ConsoleColor backgroundColor = Console.BackgroundColor;
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.DarkRed;

                            Console.Write(DisplayTextOnAskOverride, AskOverrideYesChar, AskOverrideNoChar);
                            Console.ForegroundColor = foregroundColor;
                            Console.BackgroundColor = backgroundColor;
                            Console.WriteLine();
                            Console.WriteLine();
                            char pressedKey = Console.ReadKey(true).KeyChar;
                            if (pressedKey.Equals(AskOverrideNoChar))
                            {
                                OnSkipSaveContactSheet?.Invoke(this,
                                    new ContactSheetCollectionGenericEventArgs(fi.FullName, $"File exists and ExistsAction is `Ask` but you choose '{pressedKey}' key."));
                                continue;
                            }
                        }

                        if (fi.Directory?.Exists == false)
                        {
                            OnCreateDirectoryBeforeSaveContactSheet?.Invoke(this, new ContactSheetCollectionGenericEventArgs(fi.Directory.FullName));
                            fi.Directory.Create();
                        }

                        if (fi.Exists)
                        {
                            OnOverrideContactSheet?.Invoke(this, new ContactSheetCollectionGenericEventArgs(path));
                        }
                        else
                        {
                            OnSaveContactSheet?.Invoke(this, new ContactSheetCollectionGenericEventArgs(path));
                        }

                        image.Save(path);
                    }
                    else
                    {
                        OnWarning?.Invoke(this,
                            new ContactSheetCollectionGenericEventArgs("Image is empty"));
                    }
                }
                else
                {
                    OnWarning?.Invoke(this,
                        new ContactSheetCollectionGenericEventArgs(string.Format($"Skip folder [{folder}]")));
                }
            }
        }
        catch (Exception ex)
        {
            OnError?.Invoke(this,
                new ContactSheetCollectionErrorEventArgs(ex.ToString(), ex));
        }
        finally
        {
            sw.Stop();
            OnFinish?.Invoke(this,
                new ContactSheetCollectionSummaryEventArgs(contactSheetsCounter, sw.Elapsed));
        }
    }
}