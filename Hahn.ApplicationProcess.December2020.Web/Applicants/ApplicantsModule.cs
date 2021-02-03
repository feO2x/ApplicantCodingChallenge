using Hahn.ApplicationProcess.December2020.Web.Applicants.GetApplicants;
using Microsoft.Extensions.DependencyInjection;

namespace Hahn.ApplicationProcess.December2020.Web.Applicants
{
    public static class ApplicantsModule
    {
        public static IServiceCollection AddApplicantsModule(this IServiceCollection services) =>
            services.AddTransient<IGetApplicantsSession, EfGetApplicantsSession>();
    }
}