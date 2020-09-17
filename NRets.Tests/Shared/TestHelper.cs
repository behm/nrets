using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace NRets.Tests.Shared
{
    public class TestHelper
    {
        public static IConfigurationRoot GetIConfigurationRoot(string outputPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(outputPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets("A16E16AB-AAA8-45A7-9BAB-20C572A1AE11")
                .AddEnvironmentVariables()
                .Build();
        }

        public static NRetsConfig GetApplicationConfiguration(string outputPath)
        {
            var settings = new NRetsConfig();

            var config = GetIConfigurationRoot(outputPath);

            var loginUrl = config["LoginUrl"];

            config
                .GetSection("NRets")
                .Bind(settings);

            return settings;
        }

        public static string GetEmbeddedResourceText(string filename)
        {
            // todo: if this is costly, load it up in constructor or whenever test class is loaded.
            var assembly = Assembly.GetExecutingAssembly();

            var resourcePath = $"{assembly.GetName().Name}.Resources.{filename}";

            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null) { throw new Exception($"Test data for {filename} was not found"); }

                using (var reader = new StreamReader(stream))
                {
                    var fileContent = reader.ReadToEnd();

                    return fileContent;
                }
            }
        }
    }
}
