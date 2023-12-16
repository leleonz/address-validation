namespace AddressValidation.Domain.Validators.Interfaces
{
    public interface IAustraliaAddressValidator : IAddressValidator
    {
        Task<(bool IsValid, string? Country)> ValidateCountryAsync(string country);
        Task<(bool IsValid, string? PostCode)> ValidatePostCodeAsync(string postCode);
        Task<(bool IsValid, string? State)> ValidateStateCodeAsync(string state);
        Task<(bool IsValid, string? Suburb)> ValidateSuburbAsync(string suburb);
    }
}
