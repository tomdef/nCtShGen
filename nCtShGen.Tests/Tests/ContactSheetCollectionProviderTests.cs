using NUnit.Framework;
using System;
using nCtShGen.Api.Providers;
using nCtShGen.Api.Model;
using System.Drawing;

namespace nCtShGen.Tests.Tests;

public class ContactSheetCollectionProviderTests
{
    private ConfigurationItem configurationItem = default!;

    [SetUp]
    public void Setup()
    {
        configurationItem = ConfigurationProvider.Read(@".\TestData\appsettings.test.json");
        configurationItem.ContactSheetRootFolderOnLevel = 0;
        configurationItem.ContactSheetExistsAction = ExistsAction.Skip;


        // configurationItem = new ConfigurationItem()
        // {
        //     Thumbnail = new ConfigurationThumbnailItem()
        //     {
        //         MaxWidth = 300,
        //         MaxHeight = 300
        //     }
        // };
    }

    [Category("Providers")]
    [TestCase(@".\TestData", "")]
    public void ContactSheetCollectionProviderTests_Generate(string inputFolder, string outputFolder)
    {
        ContactSheetCollectionProvider cscProvider = new(configurationItem);

        cscProvider.OnError += (s, e) => { Console.WriteLine($"Error. {e.Details}"); };
        cscProvider.OnStart += (s, e) => { Console.WriteLine("Start"); };
        cscProvider.OnFinish += (s, e) => { Console.WriteLine($"Finish (Duration = {e.Duration})"); };
        cscProvider.OnWarning += (s, e) => { Console.WriteLine($"Warning. ({e.Details})"); };
        cscProvider.OnScanFolder += (s, e) => { Console.WriteLine($"Scan folder. ({e.Details})"); };
        cscProvider.OnSkipSaveContactSheet += (s, e) => { Console.WriteLine($"Skip save CS. ({e.Details})"); };
        cscProvider.OnSaveContactSheet += (s, e) => { Console.WriteLine($"Save CS. ({e.Details})"); };
        cscProvider.OnResolveContactSheetOutput += (s, e) => { Console.WriteLine($"Resolve CS output. ({e.Details})"); };
        cscProvider.OnCreateDirectoryBeforeSaveContactSheet += (s, e) => { Console.WriteLine($"Create directory. ({e.Details})"); };

        cscProvider.OnStartGenerateContactSheet += (s, e) => { Console.WriteLine($"\t* Start generate CS. ({e.Folder})"); };
        cscProvider.OnFinishGenerateContactSheet += (s, e) => { Console.WriteLine($"\t* Finish generate CS. ({e.Folder}, {e.AllItems})"); };
        cscProvider.OnAddContactSheetItem += (s, e) => { Console.WriteLine($"\t* Add CS item. ({e.FileName})"); };
        cscProvider.OnWarningContactSheetItem += (s, e) => { Console.WriteLine($"\t* Warning CS item. ({e.FileName}, {e.Details})"); };

        cscProvider.Generate(inputFolder, outputFolder);
    }

}