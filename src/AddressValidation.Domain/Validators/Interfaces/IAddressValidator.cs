using AddressValidation.Domain.Models;

namespace AddressValidation.Domain.Validators.Interfaces
{
    public interface IAddressValidator
    {
        Task<AddressValidationResult> ValidateAddressAsync(string rawAddress);
    }
}
