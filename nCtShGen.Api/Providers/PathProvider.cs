using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using ExifPhotoReader;
using Microsoft.Extensions.Configuration;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class PathProvider
{
    private readonly MacroProvider macroProvider;

    private string rootPath = "";

    public PathProvider(MacroProvider macroProvider)
    {
        this.macroProvider = macroProvider;
    }

    public void Init(string rootPath)
    {
        this.rootPath = rootPath;
        macroProvider.Set(MacroName.rootFolderPath, rootPath);
    }

}