using System;
using FluentValidation;
using Light.GuardClauses;
using Microsoft.Extensions.Logging;

namespace Hahn.ApplicationProcess.December2020.Domain
{
    public static class Validation
    {
        // ReSharper disable ConstantConditionalAccessQualifier -- as TApplicant instances usually are crossing the process boundary (JSON, EF), we explicitly disable NRT support here
#nullable disable
        public static TApplicant TrimStringProperties<TApplicant>(this TApplicant applicant)
            where TApplicant : class, IApplicantProperties
        {
            applicant.MustNotBeNullReference(nameof(applicant));

            applicant.FirstName = applicant.FirstName?.Trim();
            applicant.LastName = applicant.LastName?.Trim();
            applicant.Address = applicant.Address?.Trim();
            applicant.CountryOfOrigin = applicant.CountryOfOrigin?.Trim();
            applicant.EmailAddress = applicant.EmailAddress?.Trim();

            return applicant;
        }
#nullable restore
        // ReSharper restore ConstantConditionalAccessQualifier

        public static IRuleBuilder<TEntity, string> ValidateCountryNameAsync<TEntity>(this IRuleBuilderInitial<TEntity, string> ruleBuilder,
                                                                                      ICountryNameValidator countryNameValidator,
                                                                                      ILogger logger) =>
            ruleBuilder.CustomAsync(async (countryName, context, cancellationToken) =>
            {
                try
                {
                    var result = await countryNameValidator.CheckIfCountryNameIsValidAsync(countryName, cancellationToken);
                    if (!result)
                        context.AddFailure($"The country \"{countryName}\" does not exist.");
                }
                catch (Exception exception)
                {
                    logger.LogError(exception, "Error while checking country name {CountryName}", countryName);
                    context.AddFailure($"The country \"{countryName}\" could not be validated due to a network error.");
                }
            });
    }
}