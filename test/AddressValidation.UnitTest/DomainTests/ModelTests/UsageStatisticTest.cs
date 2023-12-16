using AddressValidation.Domain.Models;

namespace AddressValidation.UnitTest.DomainTests.ModelTests
{
    public class UsageStatisticTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void IncreaseUsage_ByUsedCount_ShouldIncreaseUsageCountAsPerUsedCount(int usedCount)
        {
            var usageStatistic = new UsageStatistic(new Address(It.IsAny<string>()));
            var initialUsedCount = usageStatistic.UsageCount;

            usageStatistic.IncreaseUsage(usedCount);

            Assert.True(usageStatistic.UsageCount - initialUsedCount == usedCount);
        }
    }
}
