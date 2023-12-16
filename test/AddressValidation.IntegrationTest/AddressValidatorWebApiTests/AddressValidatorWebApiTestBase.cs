using AddressValidation.Data.Persistences;
using AddressValidation.Domain.Models;
using AddressValidation.Domain.Services;
using AddressValidation.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AddressValidation.IntegrationTest.AddressValidatorWebApiTests
{
    public abstract class AddressValidatorWebApiTestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient Client;

        /// <summary>
        /// Common testbed (warning: database is being shared)
        /// </summary>
        /// <param name="factory"></param>
        public AddressValidatorWebApiTestBase(WebApplicationFactory<Program> factory)
            => Client = factory.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
            {
                #region remove to override dependency
                var dataSetService = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IAddressDataSetService));
                if (dataSetService != null) services.Remove(dataSetService);

                var context = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(AddressValidationDb));
                if (context != null)
                {
                    services.Remove(context);
                    var options = services.Where(r => (r.ServiceType == typeof(DbContextOptions))
                      || (r.ServiceType.IsGenericType && r.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>))).ToArray();
                    foreach (var option in options)
                    {
                        services.Remove(option);
                    }
                }
                #endregion

                services.AddDbContext<AddressValidationDb>(options => options.UseInMemoryDatabase("IntegrationTestDatabase"));
                services.AddScoped<DataSeeder>();
                //for the sake of demo - swap to use local fake service if depending on external service
                services.AddTransient<IAddressDataSetService, FakeAddressDataSetService>();

                var provider = services.BuildServiceProvider();
                // seed data
                using (var scope = provider.CreateScope())
                {
                    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                    dataSeeder.Seed();
                }

            })).CreateClient();
    }

    public class DataSeeder
    {
        private readonly AddressValidationDb context;

        public DataSeeder(AddressValidationDb context) { this.context = context; }

        public void Seed()
        {
            foreach (var stat in GetUsageStatisticSeed())
            {
                context.Add(stat);
            }

            context.SaveChanges();
        }

        public static IEnumerable<UsageStatistic> GetUsageStatisticSeed()
        {
            yield return new UsageStatistic(new Address("Australia", "Stanhope Gardens", "New South Wales", "2768", "Stanhope Pkwy & Sentry Dr"));
            yield return new UsageStatistic(new Address("Australia", "Caversham", "West Australia", "6055", "175 Suffolk St"));
            yield return new UsageStatistic(new Address("Australia", "Casula", "New South Wales", "2170", null));
        }
    }
}
