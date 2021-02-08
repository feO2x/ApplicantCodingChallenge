using System;
using System.Threading;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.December2020.Domain;

namespace Hahn.ApplicationProcess.December2020.Tests.Domain
{
    public sealed class CountryNameValidatorStub : ICountryNameValidator
    {
        public bool IsValidCountry { get; set; } = true;

        public bool ThrowException { get; set; }

        public Task<bool> CheckIfCountryNameIsValidAsync(string countryName, CancellationToken cancellationToken)
        {
            if (ThrowException)
                throw new Exception("An exception occurred while validating the country");
            return Task.FromResult(IsValidCountry);
        }
    }
}