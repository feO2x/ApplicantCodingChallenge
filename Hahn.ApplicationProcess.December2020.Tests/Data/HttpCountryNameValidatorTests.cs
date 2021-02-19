using System.Threading.Tasks;
using FluentAssertions;
using Hahn.ApplicationProcess.December2020.Data;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Hahn.ApplicationProcess.December2020.Tests.Data
{
    public static class HttpCountryNameValidatorTests
    {
        [SkippableFact]
        public static async Task ValidateCountry()
        {
            Skip.IfNot(TestSettings.Configuration.GetValue<bool>("runHttpIntegrationTests"));

            var countryNameValidator = new HttpCountryNameValidator();

            var result = await countryNameValidator.CheckIfCountryNameIsValidAsync("Puerto Rico");

            result.Should().BeTrue();
        }
    }
}