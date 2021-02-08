using Hahn.ApplicationProcess.December2020.Data;
using Hahn.ApplicationProcess.December2020.Domain;
using Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicant;
using Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicants;
using Hahn.ApplicationProcess.December2020.Web.Applicants.NewApplicant;
using Microsoft.Extensions.DependencyInjection;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants
{
    public static class ApplicantsModule
    {
        public static IServiceCollection AddApplicantsModule(this IServiceCollection services) =>
            services.AddTransient<IGetApplicantsSession, EfGetApplicantsSession>()
                    .AddSingleton<ICountryNameValidator, HttpCountryNameValidator>()
                    .AddSingleton<NewApplicantDtoValidator>()
                    .AddTransient<INewApplicantSession, EfNewApplicantSession>()
                    .AddTransient<IGetApplicantSession, EfGetApplicantSession>();
    }
}