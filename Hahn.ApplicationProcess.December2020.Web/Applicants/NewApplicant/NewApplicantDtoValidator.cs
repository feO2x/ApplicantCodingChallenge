using Hahn.ApplicationProcess.December2020.Domain;
using Microsoft.Extensions.Logging;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants.NewApplicant
{
    public sealed class NewApplicantDtoValidator : BaseApplicantValidator<NewApplicantDto>
    {
        public NewApplicantDtoValidator(ICountryNameValidator countryNameValidator, ILogger<NewApplicantDtoValidator> logger)
            : base(countryNameValidator, logger) { }
    }
}