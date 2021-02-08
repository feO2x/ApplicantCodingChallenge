using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation.TestHelper;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Tests.TestHelpers;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Hahn.ApplicationProcess.December2020.Tests.Domain
{
    public sealed class ApplicantValidatorTests
    {
        public ApplicantValidatorTests(ITestOutputHelper output)
        {
            var logger = output.CreateLoggerFactory().CreateLogger<ApplicantValidator>();
            Validator = new ApplicantValidator(new CountryNameValidatorStub(), logger);
        }

        private ApplicantValidator Validator { get; }

        [Fact]
        public static void ApplicantValidatorMustDeriveFromBaseApplicantValidator() =>
            typeof(ApplicantValidator).Should().BeDerivedFrom<BaseApplicantValidator<Applicant>>();

        [Theory]
        [InlineData(1)]
        [InlineData(25)]
        [InlineData(240192)]
        public async Task ValidId(int id)
        {
            var applicant = CreateApplicant(id);

            var result = await Validator.TestValidateAsync(applicant);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-845)]
        public async Task InvalidId(int invalidId)
        {
            var applicant = CreateApplicant(invalidId);

            var result = await Validator.TestValidateAsync(applicant);

            result.ShouldHaveValidationErrorFor(a => a.Id);
        }

        private static Applicant CreateApplicant(int id) =>
            new()
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Address = "135 Fantasy Road, Michigan",
                CountryOfOrigin = "USA",
                DateOfBirth = new DateTime(1979, 8, 18),
                EmailAddress = "john.doe@gmail.com"
            };
    }
}