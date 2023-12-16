namespace AddressValidation.Domain.Models
{
    public class UsageStatistic
    {
        public Address Address { get; private set; }
        public int UsageCount { get; private set; }

        private UsageStatistic() { }

        public UsageStatistic(Address address)
        {
            Address = address;
            UsageCount = 1;
        }

        public void IncreaseUsage(int usedCount = 1)
        {
            UsageCount += usedCount;
        }
    }
}
