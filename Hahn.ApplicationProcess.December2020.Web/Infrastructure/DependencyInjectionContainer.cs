using LightInject;
using LightInject.Microsoft.DependencyInjection;

namespace Hahn.ApplicationProcess.December2020.Web.Infrastructure
{
    public static class DependencyInjectionContainer
    {
        public static ServiceContainer Instance { get; } = new ServiceContainer(ContainerOptions.Default.WithMicrosoftSettings());
    }
}