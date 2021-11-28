using Microsoft.Extensions.Configuration;
using nCtShGen.Api.Model;

namespace nCtShGen.Api.Providers;

public class ConfigurationProvider
{
    public static ConfigurationItem Read(string configFileName = "appsettings.json")
    {
        Console.WriteLine("current folder:{0}", AppDomain.CurrentDomain.BaseDirectory);

        var config = new ConfigurationBuilder()
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile(configFileName).Build();
        ConfigurationItem configurationItem = new();
        config.GetSection("appsettings").Bind(configurationItem);

        return configurationItem;
    }
}