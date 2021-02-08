using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Hahn.ApplicationProcess.December2020.Tests.Domain
{
    public sealed class BaseApplicantValidatorTests
    {
        public BaseApplicantValidatorTests(ITestOutputHelper output)
        {
            var logger = output.CreateLoggerFactory().CreateLogger<ApplicantValidatorDummy>();
            CountryNameValidator = new CountryNameValidatorStub();
            Validator = new ApplicantValidatorDummy(CountryNameValidator, logger);
        }

        private ApplicantValidatorDummy Validator { get; }

        private CountryNameValidatorStub CountryNameValidator { get; }

        [Fact]
        public async Task ValidApplicant()
        {
            var applicant = CreateApplicant();

            var result = await Validator.TestValidateAsync(applicant);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData("B")]
        [InlineData("\t\tt")]
        public Task InvalidFirstName(string invalidFirstName)
        {
            var applicant = CreateApplicant();
            applicant.FirstName = invalidFirstName;

            return CheckValidationErrorAsync(applicant, a => a.FirstName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("F")]
        [InlineData("T       ")]
        public Task InvalidLastName(string invalidLastName)
        {
            var applicant = CreateApplicant();
            applicant.LastName = invalidLastName;

            return CheckValidationErrorAsync(applicant, a => a.LastName);
        }

        [Theory]
        [MemberData(nameof(InvalidDateOfBirthData))]
        public Task InvalidDateOfBirth(DateTime invalidDateOfBirth)
        {
            var applicant = CreateApplicant();
            applicant.DateOfBirth = invalidDateOfBirth;

            return CheckValidationErrorAsync(applicant, a => a.DateOfBirth);
        }

        [Theory]
        [InlineData("Too short")]
        [InlineData("AtLeast10")]
        [InlineData("")]
        [InlineData("          NotEnough\t")]
        public Task InvalidAddress(string invalidAddress)
        {
            var applicant = CreateApplicant();
            applicant.Address = invalidAddress;

            return CheckValidationErrorAsync(applicant, a => a.Address);
        }

        [Theory]
        [InlineData("this is not an email address")]
        [InlineData("missing.domain@")]
        [InlineData("@just-domain.com")]
        [InlineData("where.is.the.add.sign.eu")]
        [InlineData("")]
        [InlineData("  extra-padding  ")]
        public Task InvalidEmailAddress(string invalidEmailAddress)
        {
            var applicant = CreateApplicant();
            applicant.EmailAddress = invalidEmailAddress;

            return CheckValidationErrorAsync(applicant, a => a.EmailAddress);
        }

        [Fact]
        public Task InvalidCountryName()
        {
            var applicant = CreateApplicant();
            applicant.CountryOfOrigin = "Invalid Country Name";
            CountryNameValidator.IsValidCountry = false;

            return CheckValidationErrorAsync(applicant, a => a.CountryOfOrigin);
        }

        [Fact]
        public Task ErrorDuringCountryValidation()
        {
            var applicant = CreateApplicant();
            applicant.CountryOfOrigin = "Germany";
            CountryNameValidator.ThrowException = true;

            return CheckValidationErrorAsync(applicant, a => a.CountryOfOrigin);
        }

        public static readonly TheoryData<DateTime> InvalidDateOfBirthData =
            new()
            {
                new DateTime(1899, 12, 31),
                DateTime.Today.AddDays(1)
            };

        private async Task CheckValidationErrorAsync<TProperty>(Applicant applicant,
                                                                Expression<Func<Applicant, TProperty>> memberAccessor)
        {
            var result = await Validator.TestValidateAsync(applicant);
            result.ShouldHaveValidationErrorFor(memberAccessor);
        }

        private static Applicant CreateApplicant() =>
            new()
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "135 Fantasy Road, Michigan",
                CountryOfOrigin = "USA",
                DateOfBirth = new DateTime(1979, 8, 18),
                EmailAddress = "john.doe@gmail.com"
            };

        private sealed class ApplicantValidatorDummy : BaseApplicantValidator<Applicant>
        {
            public ApplicantValidatorDummy(ICountryNameValidator countryNameValidator, ILogger logger) : base(countryNameValidator, logger) { }
        }
    }
}