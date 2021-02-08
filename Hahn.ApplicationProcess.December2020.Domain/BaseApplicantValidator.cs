using System;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Hahn.ApplicationProcess.December2020.Domain
{
    public abstract class BaseApplicantValidator<T> : AbstractValidator<T>
        where T : class, IApplicantProperties
    {
        protected BaseApplicantValidator(ICountryNameValidator countryNameValidator, ILogger logger)
        {
            RuleFor(applicant => applicant.FirstName).MinimumLength(2);
            RuleFor(applicant => applicant.LastName).MinimumLength(2);
            RuleFor(applicant => applicant.DateOfBirth).GreaterThan(new DateTime(1900, 1, 1)).LessThan(DateTime.Today);
            RuleFor(applicant => applicant.Address).MinimumLength(10);
            RuleFor(applicant => applicant.EmailAddress).EmailAddress();
            RuleFor(applicant => applicant.CountryOfOrigin).ValidateCountryNameAsync(countryNameValidator, logger);
        }

        protected override bool PreValidate(ValidationContext<T> context, ValidationResult result)
        {
            if (context.InstanceToValidate == null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, "You cannot pass null in the request body."));
                return false;
            }

            context.InstanceToValidate.TrimStringProperties();
            return true;
        }
    }
}