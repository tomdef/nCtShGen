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

    public event EventHandler<ContactSheetEventArgs> OnStartGenerateContactSheet = default!;
    public event EventHandler<ContactSheetEventArgs> OnFinishGenerateContactSheet = default!;
    public event EventHandler<ContactSheetItemEventArgs> OnAddContactSheetItem = default!;
    public event EventHandler<ContactSheetItemWarningEventArgs> OnWarningContactSheetItem = default!;

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

    public void Generate(string inputFolder, string outputFolder, string scanSubfolderStruct = "{1}\\+\\**")
    {
        EnumerationOptions eo = new()
        {
            RecurseSubdirectories = true,
            ReturnSpecialDirectories = false,
            IgnoreInaccessible = true,
            MaxRecursionDepth = configuration.ContactSheetSubfolderDeepLevel
        };

        int counter = 0;

        string[] folders = Directory.GetDirectories(inputFolder, "*", eo);
        foreach (string folder in folders)
        {
            string[] levels = folder
                .Replace(inputFolder, "")
                .Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

            int levelsCount = levels.Length;
            if ((levelsCount >= 0) && (levelsCount == configuration.ContactSheetRootFolderOnLevel))
            {
                counter++;
                string contactSheetRootFolder = inputFolder;
                for (int x = 0; x < configuration.ContactSheetRootFolderOnLevel; x++)
                {
                    contactSheetRootFolder = Path.Combine(contactSheetRootFolder, levels[x]);
                }

                Console.WriteLine($"\t[+] Skan folder [{contactSheetRootFolder}]");

                DirectoryInfo di = new(contactSheetRootFolder);

                macroProvider.Set(MacroName.rootFolderPath, contactSheetRootFolder);
                macroProvider.Set(MacroName.currentFolderPath, folder);
                macroProvider.Set(MacroName.currentFolderName, di.Name);
                macroProvider.Set(MacroName.outputFolderPath, outputFolder);
                macroProvider.Set(MacroName.counter, counter++);
                macroProvider.Set(MacroName.date, DateTime.Now);
                macroProvider.Set(MacroName.day, DateTime.Now);
                macroProvider.Set(MacroName.month, DateTime.Now);
                macroProvider.Set(MacroName.year, DateTime.Now);

                string title = di.Name;
                string fileName = macroProvider.Resolve("${yearYYYY}_${currentfolder}");
                string contactSheetOutputPath = @"d:\temp\test\output";
                string path = Path.Combine(contactSheetOutputPath, string.Format($"{fileName}.jpg"));

                Console.WriteLine($"\t\t -- [output path]:{path}");

                var image = csProvider.GenerateContactSheet(title, di.FullName);
                if (image != null)
                {
                    Console.WriteLine($"Write image into [{path}]");

                    bool saveIsOk = ((File.Exists(path) == false));// || (o == true));

                    if (saveIsOk)
                    {
                        image.Save(path);
                    }
                }
                else
                {
                    Console.WriteLine("Image is empty.");
                }
            }
            else
            {
                Console.WriteLine($"Skip [{folder}].");
            }
        }


    }
}