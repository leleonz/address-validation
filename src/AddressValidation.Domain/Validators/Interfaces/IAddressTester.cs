namespace AddressValidation.Domain.Validators.Interfaces
{
    public interface IAddressTester
    {
        public bool CanValidateByCountry(string country);
        public bool CanValidateAddress(string rawAddress, out string country);
    }
}
