using BI;
using Contracts.Managers;

namespace ClimateControlModuleBackEnd.AppStart
{
    public static class ApplicationServicesConfiguration
    {
        public static void ConfigureApplicationServices(IServiceCollection services)
        {
            ConfigureManagers(services);

            // ConfigureRepositories(services); -- Chintan: this can be uncommented if repositories are added in future
        }

        public static void ConfigureManagers(IServiceCollection services)
        {
            // Register the WeatherObservationManager as the implementation of IWeatherObservationManager
            services.AddScoped<Contracts.Managers.IWeatherObservationManager, BI.WeatherObservationManager>();

        }
    }
}
