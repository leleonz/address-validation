using AddressValidation.Api.Models.Requests;
using AddressValidation.Api.Models.Responses;
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
    public class ValidateAddressTest
    {
        [Fact]
        public async Task ValidateAddress_ShouldReturnOKResponse()
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

                services.AddDbContext<AddressValidationDb>(options => options.UseInMemoryDatabase("TestValidateAddressDatabase"));
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

            var request = new ValidateAddressesRequest()
            {
                RawAddresses = new List<string>
                {
                    "Stanhope Pkwy & Sentry Dr, Stanhope Gardens NSW 2768, Australia",
                    "175 Suffolk St, Caversham WA 6055, Australia",
                    "Casula NSW 2170, Australia",
                    "175 Suffolk St, Casula WA 6055, Australia",
                    "random, random, random notdigit, NotAustralia"
                }
            };

            var result = await client.PostAsJsonAsync($"/address-validator", request);
            var content = await result.Content.ReadFromJsonAsync<IEnumerable<ValidateAddressResponse>>();

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(content);
            Assert.Equal(request.RawAddresses.Count(), content.Count());
        }
    }
}
