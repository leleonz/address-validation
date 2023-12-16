namespace AddressValidation.Domain.Validators
{
    /// <summary>
    /// Implement basic checker where address sections are expected to be split by comma.
    /// </summary>
    public class BasicAddressTester : BaseAddressTester
    {
        public BasicAddressTester(IAvailableCountryHolder availableCountryHolder) : base(availableCountryHolder) { }

        protected override bool TryGetCountryFromAddress(string rawAddress, out string? countryName)
        {
            countryName = null;
            if (string.IsNullOrWhiteSpace(rawAddress)) return false;

            //split address string by comma, and assuming the last part is the country name
            var addressSections = rawAddress.Split(",");
            var extractedCountryName = addressSections[addressSections.Length - 1].Trim();

            var isValid = CanValidateByCountry(extractedCountryName);
            if (isValid) countryName = extractedCountryName;

            return isValid;
        }
    }
}
