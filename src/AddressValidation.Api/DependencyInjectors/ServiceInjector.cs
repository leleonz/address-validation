using AddressValidation.Api.Services;
using AddressValidation.Api.Services.Interfaces;

namespace AddressValidation.Api.DependencyInjectors
{
    public static class ServiceInjector
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IAddressValidationService, AddressValidationService>();
        }
    }
}
