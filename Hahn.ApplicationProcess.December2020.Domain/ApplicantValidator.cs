using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Hahn.ApplicationProcess.December2020.Domain
{
    public sealed class ApplicantValidator : BaseApplicantValidator<Applicant>
    {
        public ApplicantValidator(ICountryNameValidator countryNameValidator, ILogger<ApplicantValidator> logger) : base(countryNameValidator, logger)
        {
            RuleFor(applicant => applicant.Id).GreaterThan(0);
        }
    }
}