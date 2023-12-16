using AddressValidation.Domain.Services.Interfaces;
using AddressValidation.Domain.Validators;

namespace AddressValidation.Domain.Services
{
    /// <summary>
    /// Fake (external) service to return sources for address
    /// </summary>
    public class FakeAddressDataSetService : IAddressDataSetService
    {
        private IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> _stateList;
        private IReadOnlyDictionary<string, IEnumerable<string>> _suburbList;

        public FakeAddressDataSetService()
        {
            #region States
            _stateList = new Dictionary<string, IReadOnlyDictionary<string, string>>(StringComparer.OrdinalIgnoreCase)
            {
                [AvailableCountry.Australia] = new Dictionary<string, string>
                {
                    ["NSW"] = "New South Wales",
                    ["QLD"] = "Queensland",
                    ["SA"] = "South Australia",
                    ["TAS"] = "Tasmania",
                    ["VIC"] = "Victoria",
                    ["WA"] = "Western Australia"
                },
            };
            #endregion

            #region Suburbs
            _suburbList = new Dictionary<string, IEnumerable<string>>(StringComparer.OrdinalIgnoreCase)
            {
                [AvailableCountry.Australia] = new List<string>
                {
                    "Abbey",
                    "Abbotsbury",
                    "Abbotsford",
                    "Babinda",
                    "Bakery Hill",
                    "Casula",
                    "Caversham",
                    "Stanhope Gardens"
                },
            };
            #endregion
        }

        public async Task<IEnumerable<string>> GetSuburbsByCountryAsync(string country)
        {
            _suburbList.TryGetValue(country, out var suburbs);

            return await Task.FromResult(suburbs ?? Enumerable.Empty<string>());
        }

        public async Task<IReadOnlyDictionary<string, string>> GetStatesByCountryAsync(string country)
        {
            _stateList.TryGetValue(country, out var states);

            return await Task.FromResult(states ?? new Dictionary<string, string>().AsReadOnly());
        }
    }
}
