using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;
using Light.GuardClauses;
using Light.GuardClauses.Exceptions;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hahn.ApplicationProcess.December2020.Web.Infrastructure
{
    public static class DependencyInjectionContainer
    {
        public static ServiceContainer Instance { get; } = new(ContainerOptions.Default.WithMicrosoftSettings());

        public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var numberOfApplicants = configuration.GetValue("numberOfApplicants", 100);
            var allowedRange = ApplicantFactory.NumberOfApplicantsRange;
            if (numberOfApplicants.IsNotIn(allowedRange))
                throw new InvalidConfigurationException($"{nameof(numberOfApplicants)} must be between {allowedRange.From} and {allowedRange.To}, but you provided {numberOfApplicants}. Please adjust appsettings.json.");

            var options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase("Applicants")
                                                                        .Options;
            var applicants = ApplicantFactory.GenerateFakeData(numberOfApplicants);
            using var context = new DatabaseContext(options);
            context.Applicants.AddRange(applicants);
            context.SaveChanges();

            return services.AddTransient<DatabaseContext>()
                           .AddSingleton<DbContextOptions>(options);
        }
    }
}