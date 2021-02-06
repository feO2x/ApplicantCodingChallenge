using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hahn.ApplicationProcess.December2020.Web.Infrastructure
{
    public static class FluentValidation
    {
        public static IMvcBuilder AddFluentValidation(this IMvcBuilder mvcBuilder) =>
            mvcBuilder.AddFluentValidation(
                configuration =>
                    configuration.RegisterValidatorsFromAssemblyContaining(typeof(FluentValidation), lifetime: ServiceLifetime.Singleton)
            );
    }
}