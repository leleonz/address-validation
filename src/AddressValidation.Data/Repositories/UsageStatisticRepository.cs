using AddressValidation.Data.Persistences;
using AddressValidation.Data.Repositories.Interfaces;
using AddressValidation.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AddressValidation.Data.Repositories
{
    public class UsageStatisticRepository : IUsageStatisticRepository, IDisposable
    {
        private readonly AddressValidationDb _addressValidationDb;

        public UsageStatisticRepository(AddressValidationDb addressValidationDb)
        {
            _addressValidationDb = addressValidationDb;
        }

        public virtual async Task<UsageStatistic> AddOrUpdateAsync(UsageStatistic stat)
        {
            var addressStatistic = await FindAsync(stat.Address);

            if (addressStatistic != null)
            {
                var difference = stat.UsageCount - addressStatistic.UsageCount;
                addressStatistic.IncreaseUsage(difference);
            }
            else
            {
                addressStatistic = stat;
                _addressValidationDb.UsageStatistics.Add(addressStatistic);
            }

            return addressStatistic;
        }

        public virtual IQueryable<UsageStatistic> GetAll()
        {
            return _addressValidationDb.UsageStatistics;
        }

        public virtual async Task<UsageStatistic?> FindAsync(Address address)
        {
            return await _addressValidationDb.UsageStatistics.SingleOrDefaultAsync(IsMatchAddress(address));
        }

        public virtual UsageStatistic? Find(Address address)
        {
            return _addressValidationDb.UsageStatistics.SingleOrDefault(IsMatchAddress(address));
        }

        public async Task SaveAsync()
        {
            await _addressValidationDb.SaveChangesAsync();
        }

        public void Save()
        {
            _addressValidationDb.SaveChanges();
        }

        /// <summary>
        /// Used as criteria/ specification for matching usage statistics with same address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private Expression<Func<UsageStatistic, bool>> IsMatchAddress(Address address)
        {
            // can be moved to specification class if used in more than one place
            return stat =>
                stat.Address != null &&
                address != null &&
                (stat.Address.Street != null ? stat.Address.Street.ToUpper() : stat.Address.Street) == (address.Street != null ? address.Street.ToUpper() : address.Street) &&
                (stat.Address.Suburb != null ? stat.Address.Suburb.ToUpper() : stat.Address.Suburb) == (address.Suburb != null ? address.Suburb.ToUpper() : address.Suburb) &&
                (stat.Address.State != null ? stat.Address.State.ToUpper() : stat.Address.State) == (address.State != null ? address.State.ToUpper() : address.State) &&
                (stat.Address.PostCode != null ? stat.Address.PostCode.ToUpper() : stat.Address.PostCode) == (address.PostCode != null ? address.PostCode.ToUpper() : address.PostCode) &&
                (stat.Address.Country != null ? stat.Address.Country.ToUpper() : stat.Address.Country) == (address.Country != null ? address.Country.ToUpper() : address.Country);
        }

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _addressValidationDb.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
