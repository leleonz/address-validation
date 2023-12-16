using AddressValidation.Api.Factories.Interfaces;
using AddressValidation.Api.Models.Requests;
using AddressValidation.Api.Models.Responses;
using AddressValidation.Api.Services.Interfaces;
using AddressValidation.Data.Repositories.Interfaces;
using AddressValidation.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressValidation.Api.Services
{
    public class AddressValidationService : IAddressValidationService
    {
        private readonly IAddressValidatorSelector _addressValidatorSelector;
        private readonly IUsageStatisticRepository _usageStatisticRepository;

        public AddressValidationService(IAddressValidatorSelector addressValidatorSelector, IUsageStatisticRepository usageStatisticRepository)
        {
            _addressValidatorSelector = addressValidatorSelector;
            _usageStatisticRepository = usageStatisticRepository;
        }

        public async Task<int> GetUsageCountByStateAsync(string state)
        {
            var uniqueCount = 0;
            if (!string.IsNullOrWhiteSpace(state))
            {
                uniqueCount = await _usageStatisticRepository.GetAll().CountAsync(stat => stat.Address.State != null && stat.Address.State.ToUpper() == state.ToUpper());
            }

            return uniqueCount;
        }

        public async Task<IEnumerable<ValidateAddressResponse>> ValidateAddressAsync(ValidateAddressesRequest request)
        {
            var validationResults = new List<ValidateAddressResponse>();
            var validationTasks = new List<Task<AddressValidationResult>>();
            var updateUsageStatTasks = new List<Task<UsageStatistic>>();

            foreach (var rawAddress in request.RawAddresses)
            {
                var validator = _addressValidatorSelector.GetAddressValidatorByAddress(rawAddress);
                validationTasks.Add(validator.ValidateAddressAsync(rawAddress));
            }

            foreach (var validationTask in validationTasks)
            {
                var validationResult = await validationTask;

                if (validationResult != null)
                {
                    var validAddress = validationResult.ValidAddress;
                    var addressBreakdown = validAddress != null ?
                        new AddressBreakdown(validAddress.Street, validAddress.Suburb, validAddress.State, validAddress.PostCode, validAddress.Country) : null;

                    validationResults.Add(new ValidateAddressResponse(validationResult.ValidatedAddress, validationResult.IsValid, addressBreakdown));

                    if (validationResult.IsValid && validationResult.ValidAddress is not null)
                    {
                        var statResult = _usageStatisticRepository.Find(validationResult.ValidAddress);
                        if (statResult == null)
                        {
                            statResult = new UsageStatistic(validationResult.ValidAddress);
                        }
                        else
                        {
                            statResult.IncreaseUsage();
                        }

                        updateUsageStatTasks.Add(_usageStatisticRepository.AddOrUpdateAsync(statResult));
                    }
                }
            }

            await Task.WhenAll(updateUsageStatTasks);
            _usageStatisticRepository.Save();

            return validationResults;
        }
    }
}
