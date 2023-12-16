using AddressValidation.Api.Factories;
using AddressValidation.Api.Factories.Interfaces;
using AddressValidation.Domain.Services;
using AddressValidation.Domain.Services.Interfaces;
using AddressValidation.Domain.Validators;
using AddressValidation.Domain.Validators.Interfaces;

namespace AddressValidation.Api.DependencyInjectors
{
    public static class ValidatorInjector
    {
        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddTransient<IAddressDataSetService, FakeAddressDataSetService>();

            services.AddTransient<IAvailableCountryHolder, AvailableCountryHolder>();
            services.AddTransient<IAddressTester, BasicAddressTester>();
            services.AddTransient<IAustraliaAddressValidator, BasicAustraliaAddressValidator>();

            services.AddSingleton<IAddressValidatorSelector>(ctx =>
            {
                var factory = new Dictionary<string, Func<IAddressValidator>>()
                {
                    [AvailableCountry.Australia] = () => ctx.GetService<IAustraliaAddressValidator>(),
                };

                var addressTester = ctx.GetRequiredService<IAddressTester>();

                return new AddressValidatorSelector(factory, addressTester);
            });
        }
    }
}
