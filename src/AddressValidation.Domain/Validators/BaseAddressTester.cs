using AddressValidation.Domain.Validators.Interfaces;

namespace AddressValidation.Domain.Validators
{
    public abstract class BaseAddressTester : IAddressTester
    {
        private readonly IAvailableCountryHolder _availableCountryHolder;

        public BaseAddressTester(IAvailableCountryHolder availableCountryHolder)
        {
            _availableCountryHolder = availableCountryHolder;
        }

        public bool CanValidateAddress(string rawAddress, out string country)
        {
            country = string.Empty;

            if (string.IsNullOrWhiteSpace(rawAddress)) return false;

            _ = TryGetCountryFromAddress(rawAddress, out var extractedCountryName);
            if (!string.IsNullOrWhiteSpace(extractedCountryName)) country = extractedCountryName.Trim();

            return !string.IsNullOrWhiteSpace(country);
        }

        protected abstract bool TryGetCountryFromAddress(string rawAddress, out string? countryName);

        public bool CanValidateByCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country)) return false;

            return _availableCountryHolder.GetAvailableCountries().Any(c => c.Trim().Equals(country.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }
}
