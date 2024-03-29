﻿using System.CommandLine;
using System.CommandLine.Invocation;
using nCtShGen.Api.Providers;
using System.Diagnostics;
using nCtShGen.Api.Model;
using Microsoft.Extensions.Logging;

namespace nCtShGen
{
    public class Program
    {
        private static ILogger logger = default!;
        private static ConfigurationItem configuration = default!;

        public static int Main(params string[] args)
        {
            configuration = ConfigurationProvider.Read();

            RootCommand rootCommand = new("Tool to generate contact sheet(s) for images.");

            Command generateCommand = new("generate", "Generate contactsheet(s).");

            Option photoFolderOption = new(aliases: new[] { "--photoFolder", "-p" },
                    description: "Image(s) input folder.",
                    argumentType: typeof(string),
                    getDefaultValue: () => configuration.RootPhotoFolder)
            {
                IsRequired = false
            };

            Option contactSheetFolderOption = new(aliases: new[] { "--csFolder", "-c" },
                    description: "Contactsheet(s) output folder.",
                    argumentType: typeof(string),
                    getDefaultValue: () => configuration.ContactSheetFolder)
            {
                IsRequired = false
            };

            Option overrideActionOption = new(aliases: new[] { "--overrideAction", "-o" },
                    description: "Override if contact sheet exists",
                    argumentType: typeof(ExistsAction),
                    getDefaultValue: () => configuration.ContactSheetExistsAction)
            {
                IsRequired = false
            };

            Option LogLevelOption = new(aliases: new[] { "--logLevel", "-l" },
                    description: "Logging level ()",
                    argumentType: typeof(LogLevel),
                    getDefaultValue: () => LogLevel.Information)
            {
                IsRequired = false
            };

            generateCommand.AddOption(photoFolderOption);
            generateCommand.AddOption(contactSheetFolderOption);
            generateCommand.AddOption(overrideActionOption);
            generateCommand.AddOption(LogLevelOption);
            generateCommand.Handler = CommandHandler.Create<string, string, ExistsAction, LogLevel>((photoFolder, csFolder, overrideAction, logLevel) =>
            {
                Generate(photoFolder, csFolder, overrideAction, logLevel);
            });

            rootCommand.AddCommand(generateCommand);

            return rootCommand.Invoke(args);
        }

        public static void Generate(string photoFolder, string csFolder, ExistsAction overrideAction, LogLevel logLevel)
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole(options => options.FormatterName = "nCtShGenLoggerFormatter")
                    .AddConsoleFormatter<nCtShGen.LoggerFormatter, nCtShGen.LoggerOptions>()
                    .AddFilter("nCtShGen", logLevel);
            });

            logger = loggerFactory.CreateLogger<Program>();


            configuration.ContactSheetExistsAction = overrideAction;

            Stopwatch swAll = new();
            swAll.Start();

            Console.WriteLine("+-------------------------------------------------------");
            Console.WriteLine("| ContactSheet generator");
            Console.WriteLine("+-----------------------------+-------------------------");
            Console.WriteLine("| Images input folder         | {0}", photoFolder);
            Console.WriteLine("| Contact sheet output folder | {0}", csFolder);
            Console.WriteLine("| Contact sheet exists action | {0}", overrideAction);
            Console.WriteLine("+-----------------------------+-------------------------");
            Console.WriteLine("| Logging level               | {0}", logLevel);
            Console.WriteLine("+-----------------------------+-------------------------");

            ContactSheetCollectionProvider cscProvider = new(configuration);

            cscProvider.OnStart += (s, e) => { logger.LogInformation("Start."); };
            cscProvider.OnFinish += (s, e) => { logger.LogInformation($"Finish. Duration: [{e.Duration}]"); };
            cscProvider.OnWarning += (s, e) => { logger.LogWarning(e.Details); };
            cscProvider.OnError += (s, e) => { logger.LogError(e.Details); };
            cscProvider.OnScanFolder += (s, e) => { logger.LogDebug($"Scan folder [{e.Details}]"); };
            cscProvider.OnSkipSaveContactSheet += (s, e) => { logger.LogWarning($"□ Skip save CS into [{e.Details}]. Reason: [{e.Reason}]"); };
            cscProvider.OnSaveContactSheet += (s, e) => { logger.LogInformation($"■ Save CS into [{e.Details}]"); };
            cscProvider.OnOverrideContactSheet += (s, e) => { logger.LogInformation($"▣ Override CS into [{e.Details}]"); };
            cscProvider.OnResolveContactSheetOutput += (s, e) => { logger.LogDebug($"Resolve CS output path as [{e.Details}])"); };
            cscProvider.OnCreateDirectoryBeforeSaveContactSheet += (s, e) => { logger.LogDebug($"Create not existing directory [{e.Details}]"); };

            cscProvider.OnStartGenerateContactSheet += (s, e) => { logger.LogInformation($"▶ Start generate CS for [{e.Folder}]"); };
            cscProvider.OnFinishGenerateContactSheet += (s, e) => { logger.LogInformation($"◀ Finish generate CS for [{e.Folder}]. Item(s): [{e.AllItems}]"); };
            cscProvider.OnAddContactSheetItem += (s, e) => { logger.LogDebug($"\t▣ Add CS item [{e.FileName}]"); };
            cscProvider.OnWarningContactSheetItem += (s, e) => { logger.LogDebug($"\t□ Warning CS item [{e.FileName}]. Reason: [{e.Details}]"); };

            cscProvider.Generate(photoFolder, csFolder);

            swAll.Stop();
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Elapsed time : {0}", swAll.Elapsed.ToString());
            Console.WriteLine();
        }
    }
}
