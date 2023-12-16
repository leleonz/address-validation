using AddressValidation.Api.Models.Requests;
using AddressValidation.Api.Models.Responses;

namespace AddressValidation.Api.Services.Interfaces
{
    public interface IAddressValidationService
    {
        Task<IEnumerable<ValidateAddressResponse>> ValidateAddressAsync(ValidateAddressesRequest rawAddresses);
        Task<int> GetUsageCountByStateAsync(string state);
    }
}
