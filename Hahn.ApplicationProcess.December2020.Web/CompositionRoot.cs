using Hahn.ApplicationProcess.December2020.Web.Applicants;
using Hahn.ApplicationProcess.December2020.Web.Infrastructure;
using Hahn.ApplicationProcess.December2020.Web.Paging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Hahn.ApplicationProcess.December2020.Web
{
    public sealed class CompositionRoot
    {
        public CompositionRoot(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddControllersAsServices()
                    .AddJsonOptions();
            services.AddSwagger()
                    .AddCorsIfNecessary(Environment)
                    .AddDomainModule()
                    .AddDataAccessModule(Configuration)
                    .AddPagingModule()
                    .AddApplicantsModule()
                    .AddAutoMapper(typeof(CompositionRoot));
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                   .UseCorsIfNecessary(Configuration);
            }

            app.UseSerilogRequestLogging()
               .UseRouting()
               .UseSwaggerAndSwaggerUi()
               .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}