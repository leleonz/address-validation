using AddressValidation.Domain.Models;
using AddressValidation.Domain.Validators.Interfaces;

namespace AddressValidation.Api.Factories
{
    public class UnknownAddressValidator : IAddressValidator
    {
        public Task<AddressValidationResult> ValidateAddressAsync(string rawAddress)
        {
            return Task.FromResult(new AddressValidationResult(rawAddress));
        }
    }
}
