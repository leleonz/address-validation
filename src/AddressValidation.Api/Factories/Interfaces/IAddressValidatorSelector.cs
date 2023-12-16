using AddressValidation.Domain.Validators.Interfaces;

namespace AddressValidation.Api.Factories.Interfaces
{
    public interface IAddressValidatorSelector
    {
        IAddressValidator GetAddressValidatorByAddress(string rawAddress);
    }
}
