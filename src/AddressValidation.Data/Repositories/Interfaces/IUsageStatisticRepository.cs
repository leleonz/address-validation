using AddressValidation.Domain.Models;

namespace AddressValidation.Data.Repositories.Interfaces
{
    public interface IUsageStatisticRepository
    {
        IQueryable<UsageStatistic> GetAll();
        Task<UsageStatistic?> FindAsync(Address address);
        UsageStatistic? Find(Address address);
        Task<UsageStatistic> AddOrUpdateAsync(UsageStatistic stat);
        Task SaveAsync();
        void Save();
    }
}
