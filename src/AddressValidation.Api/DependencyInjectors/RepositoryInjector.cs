using AddressValidation.Data.Repositories;
using AddressValidation.Data.Repositories.Interfaces;

namespace AddressValidation.Api.DependencyInjectors
{
    public static class RepositoryInjector
    {
        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsageStatisticRepository, UsageStatisticRepository>();
        }
    }
}
