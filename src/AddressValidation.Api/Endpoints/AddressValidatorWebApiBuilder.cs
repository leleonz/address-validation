using AddressValidation.Api.Models.Requests;
using AddressValidation.Api.Models.Responses;
using AddressValidation.Api.Services.Interfaces;

namespace AddressValidation.Api.Endpoints
{
    public static class AddressValidatorWebApiBuilder
    {
        public static void BuildAddressValidatorWebApis(this IEndpointRouteBuilder app)
        {
            RouteGroupBuilder addressValidationApi = app.MapGroup("/address-validators");

            addressValidationApi.MapPost("/", async (ValidateAddressesRequest request, IAddressValidationService service) =>
            {
                return await ValidateAddress(request, service);
            }).Produces<IEnumerable<ValidateAddressResponse>>(StatusCodes.Status200OK)
            .WithName("Validate addresses");

            addressValidationApi.MapGet("/stat/{stateName}", async (string stateName, IAddressValidationService service) =>
            {
                return await GetUsageCountByState(stateName, service);
            }).Produces<int>(StatusCodes.Status200OK)
            .WithName("Get validation usage count by state name");
        }

        #region extract api methods for testability
        public static async Task<IResult> ValidateAddress(ValidateAddressesRequest request, IAddressValidationService service)
        {
            if (request == null || request.RawAddresses == null || !request.RawAddresses.Any()) return TypedResults.BadRequest();

            return TypedResults.Ok(await service.ValidateAddressAsync(request));
        }

        public static async Task<IResult> GetUsageCountByState(string stateName, IAddressValidationService service)
        {
            if (string.IsNullOrWhiteSpace(stateName)) return TypedResults.BadRequest();

            return TypedResults.Ok(await service.GetUsageCountByStateAsync(stateName));
        }
        #endregion
    }
}
