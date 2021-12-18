using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using nCtShGen.Api.Providers;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using System.Data;
using System.Reflection;

namespace nCtShGen
{
    public class Program
    {
        public static int Main(params string[] args)
        {
            RootCommand rootCommand = new("Tool to generate contact sheet(s) for images.");

            Command generateCommand = new("generate", "Generate contactsheet(s).");

            Option imagesFolderOption = new(aliases: new[] { "--imgFolder", "-i" },
                    description: "Image(s) input folder.",
                    argumentType: typeof(string))
            {
                IsRequired = true
            };

            Option contactSheetFolderOption = new(aliases: new[] { "--csFolder", "-c" },
                    description: "Contactsheet(s) output folder.",
                    argumentType: typeof(string))
            {
                IsRequired = true
            };

            Option overrideOption = new(aliases: new[] { "--override", "-o" },
                    description: "Override if contact sheet exists",
                    argumentType: typeof(bool),
                    getDefaultValue: () => false)
            {
                IsRequired = false
            };

            generateCommand.AddOption(imagesFolderOption);
            generateCommand.AddOption(contactSheetFolderOption);
            generateCommand.AddOption(overrideOption);
            generateCommand.Handler = CommandHandler.Create<string, string, bool>((i, c, o) =>
            {
                Generate(i, c, o);
            });

            rootCommand.AddCommand(generateCommand);

            return rootCommand.Invoke(args);
        }

        public static void Generate(string i, string c, bool o)
        {
            MacroProvider macroProvider = new();
            var configuration = ConfigurationProvider.Read();

            Stopwatch swAll = new();
            swAll.Start();

            Console.WriteLine("+-------------------------------------------------------");
            Console.WriteLine("| ContactSheet generator");
            Console.WriteLine("+-----------------------------+-------------------------");
            Console.WriteLine("| Images input folder         | {0}", i);
            Console.WriteLine("| Contact sheet output folder | {0}", c);
            Console.WriteLine("| Override contact sheet(s)   | {0}", o);
            Console.WriteLine("+-----------------------------+-------------------------");

            macroProvider.Init();
            // macroProvider.Init(
            //     new string[] {
            //         "rootfolder",
            //         "currentfolder",
            //         "dateYYYYMMDD",
            //         "dateYYMMDD",
            //         "yearYYYY",
            //         "monthMM",
            //         "dayDD"
            //     }.AsEnumerable());

            EnumerationOptions eo = new()
            {
                RecurseSubdirectories = true,
                ReturnSpecialDirectories = false,
                IgnoreInaccessible = true,
                MaxRecursionDepth = configuration.ContactSheetRootFolderOnLevel
            };

            ContactSheetProvider csProvider = new(configuration, Api.Model.ColorSchemaName.Dark);

            string rootFolder = i;
            string[] folders = Directory.GetDirectories(rootFolder, "*", eo);
            foreach (string folder in folders)
            {
                string[] levels = folder
                    .Replace(rootFolder, "")
                    .Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                int levelsCount = levels.Length;
                if ((levelsCount >= 0) && (levelsCount == configuration.ContactSheetRootFolderOnLevel))
                {
                    string contactSheetRootFolder = rootFolder;
                    for (int x = 0; x < configuration.ContactSheetRootFolderOnLevel; x++)
                    {
                        contactSheetRootFolder = Path.Combine(contactSheetRootFolder, levels[x]);
                    }

                    Console.WriteLine($"\t[+] Skan folder [{contactSheetRootFolder}]");

                    DirectoryInfo di = new(contactSheetRootFolder);

                    // macroProvider.Set("rootfolder", contactSheetRootFolder);
                    // macroProvider.Set("currentfolder", di.Name);
                    // macroProvider.Set("dateYYYYMMDD", DateTime.Now.ToString("yyyyMMdd"));
                    // macroProvider.Set("yearYYYY", DateTime.Now.ToString("yyyy"));
                    // macroProvider.Set("monthMM", DateTime.Now.ToString("MM"));
                    // macroProvider.Set("dayDD", DateTime.Now.ToString("dd"));
                    // macroProvider.Set("year", DateTime.Now.Year);
                    // macroProvider.Set("month", DateTime.Now.Month);
                    // macroProvider.Set("day", DateTime.Now.Day);

                    string title = di.Name;
                    string fileName = macroProvider.Resolve("${yearYYYY}_${currentfolder}");
                    string contactSheetOutputPath = @"d:\temp\test\output";
                    string path = Path.Combine(contactSheetOutputPath, string.Format($"{fileName}.jpg"));

                    Console.WriteLine($"\t\t -- [output path]:{path}");

                    var image = csProvider.GenerateContactSheet(title, di.FullName);
                    if (image != null)
                    {
                        Console.WriteLine($"Write image into [{path}]");

                        bool saveIsOk = ((File.Exists(path) == false) || (o == true));

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

            swAll.Stop();
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("Elapsed time : {0}", swAll.Elapsed.ToString());
            Console.WriteLine();
        }
    }
}
