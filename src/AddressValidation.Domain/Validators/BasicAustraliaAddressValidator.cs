using AddressValidation.Domain.Models;
using AddressValidation.Domain.Services.Interfaces;
using AddressValidation.Domain.Validators.Interfaces;
using System.Text.RegularExpressions;

namespace AddressValidation.Domain.Validators
{
    /// <summary>
    /// Implement basic format validation where address sections are expected to be split by comma.
    /// </summary>
    public class BasicAustraliaAddressValidator : IAustraliaAddressValidator
    {
        private readonly string CountryName = AvailableCountry.Australia.Trim();
        private const string PostCodePattern = "^[0-9]{4}$";
        protected readonly (bool, string?) DefaultInvalidResult = (false, string.Empty);

        private readonly IAddressDataSetService _addressDataSetService;

        public BasicAustraliaAddressValidator(IAddressDataSetService addressDataSetService)
        {
            _addressDataSetService = addressDataSetService;
        }

        //Example of valid address format (street, suburb state_code postcode, country):  
        //1. Stanhope Pkwy & Sentry Dr, Stanhope Gardens NSW 2768, Australia
        //2. 175 Suffolk St, Caversham WA 6055, Australia
        //3. Casula NSW 2170, Australia
        //e.g. {optional section,} {mandatory section - 3 parts, } {mandatory country section}
        public async Task<AddressValidationResult> ValidateAddressAsync(string rawAddress)
        {
            var result = new AddressValidationResult(rawAddress);

            if (string.IsNullOrWhiteSpace(rawAddress)) return result;

            var addressSections = rawAddress.Split(",");
            var addressSectionsLength = addressSections.Length;
            //Address must at least contain (Suburb State_Code PostCode) section and (Country) section, split by comma
            if (addressSectionsLength < 2) return result;

            #region Validate Country
            var countrySection = addressSections[addressSectionsLength - 1].Trim(); //to get the last section (assuming it as country)
            var countryResult = await ValidateCountryAsync(countrySection);
            if (!countryResult.IsValid) return result;
            #endregion

            var midSection = addressSections[addressSectionsLength - 2].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries); //remove continuous spaces
            var midSectionLength = midSection.Length;
            //Middle section of address must contain Suburb, State_Code and PostCode, split by spaces
            if (midSectionLength < 3) return result;

            #region Validate PostCode
            var postcodeResult = await ValidatePostCodeAsync(midSection[midSectionLength - 1].Trim()); //to get the last item in middle section (assuming it as post code)
            if (!postcodeResult.IsValid) return result;
            #endregion

            #region Validate State
            var stateResult = await ValidateStateCodeAsync(midSection[midSectionLength - 2].Trim()); //to get the one item before last in middle section (assuming it as post code)
            if (!stateResult.IsValid) return result;
            #endregion

            #region Validate Suburb
            var suburbSection = midSection.Select(s => s.Trim()).Take(midSectionLength - 2);
            var suburbResult = await ValidateSuburbAsync(suburbSection.Any() ? string.Join(" ", suburbSection) : string.Empty); // assuming remaining middle section as suburb
            if (!suburbResult.IsValid) return result;
            #endregion

            #region Construct Street
            var streetSection = addressSections.Select(a => a.Trim()).Take(addressSectionsLength - 2); //group remaining sections as street, excluding middle and country section
            var streetResult = streetSection.Any() ? string.Join(", ", streetSection) : null;
            #endregion

            var validAddress = new Address(countryResult.Country ?? CountryName, suburbResult.Suburb, stateResult.State, postcodeResult.PostCode, streetResult);
            result.ValidAddress = validAddress;

            return result;
        }

        public virtual async Task<(bool IsValid, string? Country)> ValidateCountryAsync(string country)
        {
            if (string.IsNullOrWhiteSpace(country)) return DefaultInvalidResult;

            var isValid = CountryName.Equals(country.Trim(), StringComparison.OrdinalIgnoreCase);
            var result = isValid ? (isValid, CountryName) : DefaultInvalidResult;

            return await Task.FromResult(result);
        }

        public virtual async Task<(bool IsValid, string? PostCode)> ValidatePostCodeAsync(string postCode)
        {
            if (string.IsNullOrWhiteSpace(postCode)) return DefaultInvalidResult;

            var regex = new Regex(PostCodePattern);
            postCode = postCode.Trim();
            var isMatch = regex.IsMatch(postCode);
            var result = isMatch ? (isMatch, postCode) : DefaultInvalidResult;

            return await Task.FromResult(result);
        }

        public virtual async Task<(bool IsValid, string? State)> ValidateStateCodeAsync(string stateCode)
        {
            if (string.IsNullOrWhiteSpace(stateCode)) return DefaultInvalidResult;

            var states = await _addressDataSetService.GetStatesByCountryAsync(CountryName);

            return states != null ? (states.TryGetValue(stateCode.Trim(), out var stateName), stateName ?? string.Empty) : DefaultInvalidResult;

        }

        public virtual async Task<(bool IsValid, string? Suburb)> ValidateSuburbAsync(string suburb)
        {
            if (string.IsNullOrWhiteSpace(suburb)) return DefaultInvalidResult;

            var suburbs = await _addressDataSetService.GetSuburbsByCountryAsync(CountryName);
            var result = suburbs?.FirstOrDefault(s => s.Equals(suburb.Trim(), StringComparison.OrdinalIgnoreCase));

            return (result != null, result ?? string.Empty);
        }
    }
}
