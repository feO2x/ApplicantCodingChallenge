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
        public CompositionRoot(IConfiguration configuration) => Configuration = configuration;

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddControllersAsServices();
            services.AddSwagger()
                    .AddDomainModule()
                    .AddDataAccessModule(Configuration)
                    .AddPagingModule()
                    .AddApplicantsModule()
                    .AddAutoMapper(typeof(CompositionRoot));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSerilogRequestLogging()
               .UseRouting()
               .UseSwaggerAndSwaggerUi()
               .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}