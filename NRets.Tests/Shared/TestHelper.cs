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
    }
}
