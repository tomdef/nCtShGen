using NUnit.Framework;
using nCtShGen.Api.Providers;
using nCtShGen.Api.Model;
using System.Collections.Generic;
using System;

namespace nCtShGen.Tests.Tests;

public class MacroProviderTests
{

    private MacroProvider macroProvider = default!;

    [SetUp]
    public void Setup()
    {
        macroProvider = new MacroProvider();
        macroProvider.Init();
    }

    [Test, Category("Providers")]
    public void MacroProvider_Resolve()
    {
        DateTime dt = new(2021, 12, 01);

        macroProvider.Set(MacroName.rootFolderPath, @"c:\root");
        macroProvider.Set(MacroName.currentFolderPath, @"c:\root");
        macroProvider.Set(MacroName.currentFolderName, @"c:\root\A\current");
        macroProvider.Set(MacroName.outputFolderPath, @"c:\root\A\output");
        macroProvider.Set(MacroName.date, dt);
        macroProvider.Set(MacroName.year, dt);
        macroProvider.Set(MacroName.month, dt);
        macroProvider.Set(MacroName.day, dt);
        macroProvider.Set(MacroName.counter, 1);

        string resoldedPath = macroProvider.Resolve(@"{rootPath}\{date}_{counter}");
        Assert.AreEqual(@"c:\root\20211201_1", resoldedPath, "Invalid resolved path");

        macroProvider.Set(MacroName.counter, 2);
        resoldedPath = macroProvider.Resolve(@"{rootPath}\{date}_{counter}");
        Assert.AreEqual(@"c:\root\20211201_2", resoldedPath, "Invalid resolved path");
    }
}