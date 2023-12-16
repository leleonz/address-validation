namespace AddressValidation.Api.Models.Responses
{
    public class ValidateAddressResponse
    {
        public string ValidatedAddress { get; set; }
        public bool IsValid { get; set; }
        public AddressBreakdown? AddressBreakdown { get; set; }

        public ValidateAddressResponse() { }

        //not require parameterized ctor if using any mapper
        public ValidateAddressResponse(string validatedAddress, bool isValid, AddressBreakdown? addressBreakdown = null)
        {
            ValidatedAddress = validatedAddress;
            IsValid = isValid;
            AddressBreakdown = addressBreakdown;
        }

        public ValidateAddressResponse(string validatedAddress, bool isValid, string? street = null, string? suburb = null, string? state = null, string? postCode = null, string? country = null)
        {
            ValidatedAddress = validatedAddress;
            IsValid = isValid;
            AddressBreakdown = IsAddressInformationAvailable(street, suburb, state, postCode, country) ? new AddressBreakdown(street, suburb, state, postCode, country) : null;
        }

        private bool IsAddressInformationAvailable(string? street, string? suburb, string? state, string? postCode, string? country)
        {
            return !string.IsNullOrWhiteSpace(street) ||
                !string.IsNullOrWhiteSpace(suburb) ||
                !string.IsNullOrWhiteSpace(state) ||
                !string.IsNullOrWhiteSpace(postCode) ||
                !string.IsNullOrWhiteSpace(country);
        }
    }
}
