using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Hahn.ApplicationProcess.December2020.Web.Infrastructure
{
    public static class Swagger
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Applicants",
                    Version = "v1",
                    Description = "A sample CRUD Web API for the Hahn Coding Challenge",
                    Contact = new OpenApiContact
                    {
                        Name = "Kenny Pflug",
                        Email = "kenny.pflug@live.de"
                    }
                });
            });
        }

        public static IApplicationBuilder UseSwaggerAndSwaggerUi(this IApplicationBuilder app) =>
            app.UseSwagger()
               .UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Applicants Web API v1"));
    }
}