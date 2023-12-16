using AddressValidation.Data.Persistences;
using AddressValidation.Domain.Services;
using AddressValidation.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace AddressValidation.IntegrationTest.AddressValidatorWebApiTests
{
    public class GetUsageCountByStateNameTest
    {
        [Fact]
        public async Task GetUsageCountByStateName_ShouldReturnOKResponse()
        {
            await using var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureServices(services =>
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

                services.AddDbContext<AddressValidationDb>(options => options.UseInMemoryDatabase("TestGetCountDatabase"));
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
            }));

            var client = application.CreateClient();

            var stateName = "New South Wales";
            var expectedCount = DataSeeder.GetUsageStatisticSeed().Count(stat => stat.Address != null && stat.Address.State.Trim().ToUpper() == stateName.Trim().ToUpper());

            var result = await client.GetAsync($"/address-validator/stat/{stateName}");
            var content = await result.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(expectedCount, content);
        }
    }
}
