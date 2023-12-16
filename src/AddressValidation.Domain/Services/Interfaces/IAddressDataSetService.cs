namespace AddressValidation.Domain.Services.Interfaces
{
    public interface IAddressDataSetService
    {
        Task<IEnumerable<string>> GetSuburbsByCountryAsync(string country);
        Task<IReadOnlyDictionary<string, string>> GetStatesByCountryAsync(string country);
    }
}
