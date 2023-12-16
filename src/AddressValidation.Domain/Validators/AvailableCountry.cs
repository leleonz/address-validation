namespace AddressValidation.Domain.Validators
{
    public class AvailableCountry
    {
        public static readonly string Australia = "Australia";
    }

    public class AvailableCountryHolder : IAvailableCountryHolder
    {
        public IEnumerable<string> GetAvailableCountries()
        {
            yield return AvailableCountry.Australia;

            // add other available country before this line
        }
    }

    public interface IAvailableCountryHolder
    {
        IEnumerable<string> GetAvailableCountries();
    }
}
