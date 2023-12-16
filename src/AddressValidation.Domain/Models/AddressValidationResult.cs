namespace AddressValidation.Domain.Models
{
    public class AddressValidationResult
    {
        public string ValidatedAddress { get; private set; }
        public bool IsValid
        {
            get
            {
                return ValidAddress is not null;
            }
        }
        public Address? ValidAddress { get; set; }

        public AddressValidationResult(string validatedAddress, Address? validAddress = null)
        {
            ValidatedAddress = validatedAddress;
            ValidAddress = validAddress;
        }
    }
}
