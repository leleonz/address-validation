namespace AddressValidation.Api.Models.Requests
{
    public class ValidateAddressesRequest
    {
        public required IEnumerable<string> RawAddresses { get; set; }
    }
}
