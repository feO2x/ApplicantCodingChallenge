using Light.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Hahn.ApplicationProcess.December2020.Web.Infrastructure
{
    public static class Cors
    {
        public static IApplicationBuilder UseCorsIfNecessary(this IApplicationBuilder app, IConfiguration configuration)
        {
            var allowedCorsOrigins = configuration.GetSection("allowedCorsOrigins").Get<string[]>();
            if (allowedCorsOrigins.IsNullOrEmpty())
                return app;

            return app.UseCors(builder => builder.WithOrigins(allowedCorsOrigins)
                                          .AllowAnyHeader()
                                          .AllowAnyMethod());
        }
    }
}