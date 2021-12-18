using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Runtime.Serialization;
using ExifPhotoReader;
using Microsoft.Extensions.Configuration;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class MacroProvider
{
    private readonly string MacroEmpty = string.Empty;
    private readonly Dictionary<MacroName, string> macros;

    public MacroProvider()
    {
        macros = new Dictionary<MacroName, string>();
    }

    public void Init()
    {
        foreach (MacroName macro in Enum.GetValues<MacroName>())
        {
            Set(macro);
        }
    }

    public void Set(MacroName macro, object? value = null)
    {
        string valueAsString = string.Empty;
        if (value != null)
        {
            var dfa = EnumHelpers.GetAttributeOfType<DisplayFormatAttribute>(macro);

            valueAsString = ((dfa != null) && (!string.IsNullOrEmpty(dfa.DataFormatString)))
                ? ((DateTime)value).ToString(dfa.DataFormatString)
                : value.ToString() ?? string.Empty;
        }

        if (macros.ContainsKey(macro))
        {
            macros[macro] = valueAsString;
        }
        else
        {
            macros.Add(macro, valueAsString);
        }
    }

    public string Get(MacroName macro)
    {
        return (macros.ContainsKey(macro)) ? macros[macro] : MacroEmpty;
    }

    public IDictionary<MacroName, string> Get()
    {
        return this.macros;
    }

    public string Resolve(string value)
    {
        string result = value;
        foreach (var kv in macros)
        {
            string macro = string.Concat("{", kv.Key.ToString(), "}");
            result = result.Replace(macro, kv.Value, false, CultureInfo.InvariantCulture);
        }

        return result;
    }
}