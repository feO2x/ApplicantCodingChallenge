using Microsoft.Extensions.Configuration;

namespace Hahn.ApplicationProcess.December2020.Tests
{
    public static class TestSettings
    {
        public static IConfiguration Configuration { get; }

        static TestSettings()
        {
            Configuration = new ConfigurationBuilder().AddJsonFile("testsettings.json", true)
                                                      .AddJsonFile("testsettings.Development.json", true)
                                                      .Build();
        }
    }
}