using AddressValidation.Api.Factories.Interfaces;
using AddressValidation.Domain.Validators.Interfaces;

namespace AddressValidation.Api.Factories
{
    public class AddressValidatorSelector : IAddressValidatorSelector
    {
        private readonly IAddressTester _tester;
        private readonly Dictionary<string, Func<IAddressValidator>> _addressValidators;

        public AddressValidatorSelector(Dictionary<string, Func<IAddressValidator>> addressValidators, IAddressTester tester)
        {
            _addressValidators = addressValidators;
            _tester = tester;
        }

        public IAddressValidator GetAddressValidatorByAddress(string rawAddress)
        {
            if (string.IsNullOrWhiteSpace(rawAddress)) return new UnknownAddressValidator();

            var canValidate = _tester.CanValidateAddress(rawAddress, out var country);
            if (!canValidate || !_addressValidators.TryGetValue(country, out var validator) || validator == null)
            {
                return new UnknownAddressValidator();
            }
            return validator();
        }
    }
}
