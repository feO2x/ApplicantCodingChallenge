using Microsoft.Extensions.DependencyInjection;

namespace Hahn.ApplicationProcess.December2020.Web.Paging
{
    public static class PagingModule
    {
        public static IServiceCollection AddPagingModule(this IServiceCollection services) =>
            services.AddSingleton<PageDtoValidator>();
    }
}