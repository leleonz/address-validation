namespace AddressValidation.Api.Models.Responses
{
    public class AddressBreakdown
    {
        public string? Street { get; set; }
        public string? Suburb { get; set; }
        public string? State { get; set; }
        public string? PostCode { get; set; }
        public string? Country { get; set; }

        public AddressBreakdown() { }

        //not require parameterized ctor if using any mapper
        public AddressBreakdown(string? street, string? suburb, string? state, string? postCode, string? country)
        {
            Street = street;
            Suburb = suburb;
            State = state;
            PostCode = postCode;
            Country = country;
        }
    }
}
