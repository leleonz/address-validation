using AddressValidation.Data.Persistences;
using AddressValidation.Data.Repositories;
using AddressValidation.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressValidation.UnitTest.DataTests.RepositoryTests
{
    public class UsageStatisticRepositoryTest
    {
        [Fact]
        public async Task AddOrUpdateAsync_NewUsageStatistic_ShouldReturnAddedItem()
        {
            var queryableData = Enumerable.Empty<UsageStatistic>().AsQueryable();

            var mockSet = new Mock<DbSet<UsageStatistic>>();
            mockSet.As<IAsyncEnumerable<UsageStatistic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestDbAsyncEnumerator<UsageStatistic>(queryableData.GetEnumerator()));

            mockSet.As<IQueryable<UsageStatistic>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<UsageStatistic>(queryableData.Provider));

            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            var dbContextMock = new Mock<AddressValidationDb>();
            dbContextMock.Setup(db => db.UsageStatistics).Returns(mockSet.Object);
            var usageStatisticRepository = new UsageStatisticRepository(dbContextMock.Object);

            var newStat = new UsageStatistic(new Address("NewCountry", "NewSuburb", "NewState", "NewPostCode", "NewStreet"));
            var result = await usageStatisticRepository.AddOrUpdateAsync(newStat);

            Assert.NotNull(result);
            mockSet.Verify(m => m.Add(It.IsAny<UsageStatistic>()), Times.Once);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public async Task AddOrUpdateAsync_ExistingUsageStatistic_ShouldIncreaseUsageCountAndReturnAddedItem(int additionalIncrement, int expectedDifference)
        {
            var queryableData = Enumerable.Empty<UsageStatistic>().AsQueryable();

            var mockSet = new Mock<DbSet<UsageStatistic>>();
            mockSet.As<IAsyncEnumerable<UsageStatistic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestDbAsyncEnumerator<UsageStatistic>(queryableData.GetEnumerator()));

            mockSet.As<IQueryable<UsageStatistic>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<UsageStatistic>(queryableData.Provider));

            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            var address = new Address("CountryOne", "SuburbOne", "StateOne", "1234", "StreetOne");
            var existingStat = new UsageStatistic(address);
            var initialUsageCount = existingStat.UsageCount;
            var newStat = new UsageStatistic(address);
            newStat.IncreaseUsage(additionalIncrement);

            var dbContextMock = new Mock<AddressValidationDb>();
            dbContextMock.Setup(db => db.UsageStatistics).Returns(mockSet.Object);
            var usageStatisticRepositoryMock = new Mock<UsageStatisticRepository>(dbContextMock.Object) { CallBase = true };
            usageStatisticRepositoryMock.Setup(repo => repo.FindAsync(address)).ReturnsAsync(existingStat);
            var usageStatisticRepository = new UsageStatisticRepository(dbContextMock.Object);

            var result = await usageStatisticRepository.AddOrUpdateAsync(newStat);

            Assert.NotNull(result);
            Assert.True(initialUsageCount + expectedDifference == result.UsageCount);
        }

        [Fact]
        public void GetAll_ShouldReturnQueryableUsageStatistics()
        {
            var data = GetFakeUsageStatistics();
            var queryableData = data.AsQueryable();

            var mockSet = new Mock<DbSet<UsageStatistic>>();
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            var dbContextMock = new Mock<AddressValidationDb>();
            dbContextMock.Setup(db => db.UsageStatistics).Returns(mockSet.Object);
            var usageStatisticRepository = new UsageStatisticRepository(dbContextMock.Object);

            var results = usageStatisticRepository.GetAll();

            Assert.Equal(data.Count(), results.Count());
        }

        [Fact]
        public async Task FindAsync_UsingExistingAddress_ShouldReturnSingleExistingUsageStatistic()
        {
            var data = GetFakeUsageStatistics();
            var firstItem = data.First();
            var queryableData = data.AsQueryable();

            var mockSet = new Mock<DbSet<UsageStatistic>>();
            mockSet.As<IAsyncEnumerable<UsageStatistic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestDbAsyncEnumerator<UsageStatistic>(queryableData.GetEnumerator()));

            mockSet.As<IQueryable<UsageStatistic>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<UsageStatistic>(queryableData.Provider));

            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            var dbContextMock = new Mock<AddressValidationDb>();
            dbContextMock.Setup(db => db.UsageStatistics).Returns(mockSet.Object);
            var usageStatisticRepository = new UsageStatisticRepository(dbContextMock.Object);

            var result = await usageStatisticRepository.FindAsync(firstItem.Address);

            Assert.NotNull(result);
            Assert.Equal(firstItem.Address, result.Address);
        }

        [Fact]
        public async Task FindAsync_UsingNonExistingAddress_ShouldReturnNull()
        {
            var randomAddress = new Address("Random", "Random", "Random", "Random");
            var data = GetFakeUsageStatistics();
            var queryableData = data.AsQueryable();

            var mockSet = new Mock<DbSet<UsageStatistic>>();
            mockSet.As<IAsyncEnumerable<UsageStatistic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestDbAsyncEnumerator<UsageStatistic>(queryableData.GetEnumerator()));

            mockSet.As<IQueryable<UsageStatistic>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<UsageStatistic>(queryableData.Provider));

            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            var dbContextMock = new Mock<AddressValidationDb>();
            dbContextMock.Setup(db => db.UsageStatistics).Returns(mockSet.Object);
            var usageStatisticRepository = new UsageStatisticRepository(dbContextMock.Object);

            var result = await usageStatisticRepository.FindAsync(randomAddress);

            Assert.Null(result);
        }

        [Fact]
        public void Find_UsingExistingAddress_ShouldReturnSingleExistingUsageStatistic()
        {
            var data = GetFakeUsageStatistics();
            var firstItem = data.First();
            var queryableData = data.AsQueryable();

            var mockSet = new Mock<DbSet<UsageStatistic>>();
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            var dbContextMock = new Mock<AddressValidationDb>();
            dbContextMock.Setup(db => db.UsageStatistics).Returns(mockSet.Object);
            var usageStatisticRepository = new UsageStatisticRepository(dbContextMock.Object);

            var result = usageStatisticRepository.Find(firstItem.Address);

            Assert.NotNull(result);
            Assert.Equal(firstItem.Address, result.Address);
        }

        [Fact]
        public void Find_UsingNonExistingAddress_ShouldReturnNull()
        {
            var randomAddress = new Address("Random", "Random", "Random", "Random");
            var data = GetFakeUsageStatistics();
            var queryableData = data.AsQueryable();

            var mockSet = new Mock<DbSet<UsageStatistic>>();
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockSet.As<IQueryable<UsageStatistic>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

            var dbContextMock = new Mock<AddressValidationDb>();
            dbContextMock.Setup(db => db.UsageStatistics).Returns(mockSet.Object);
            var usageStatisticRepository = new UsageStatisticRepository(dbContextMock.Object);

            var result = usageStatisticRepository.Find(randomAddress);

            Assert.Null(result);
        }

        [Fact]
        public async Task SaveAsync_ShouldCallSaveChangeAsyncOnce()
        {
            var dbContextMock = new Mock<AddressValidationDb>();
            var usageStatisticRepository = new UsageStatisticRepository(dbContextMock.Object);

            await usageStatisticRepository.SaveAsync();

            dbContextMock.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Save_ShouldCallSaveChangeOnce()
        {
            var dbContextMock = new Mock<AddressValidationDb>();
            var usageStatisticRepository = new UsageStatisticRepository(dbContextMock.Object);

            usageStatisticRepository.Save();

            dbContextMock.Verify(db => db.SaveChanges(), Times.Once);
        }

        private IEnumerable<UsageStatistic> GetFakeUsageStatistics()
        {
            var usageStatOne = new UsageStatistic(new Address("CountryOne", "SuburbOne", "StateOne", "1234", "StreetOne"));
            var usageStatTwo = new UsageStatistic(new Address("CountryOne", "SuburbTwo", "StateOne", "1235"));
            var usageStatThree = new UsageStatistic(new Address("CountryOne", "SuburbThree", "StateTwo", "4657", "StreetTwo"));
            var usageStatFour = new UsageStatistic(new Address("CountryTwo", "SuburbFour", "StateThree", "7894"));
            var usageStatFive = new UsageStatistic(new Address("CountryTwo", "SuburbFour", "StateThree", "7894", "StreetThree"));

            usageStatOne.IncreaseUsage();
            usageStatTwo.IncreaseUsage(2);
            usageStatThree.IncreaseUsage();
            usageStatFour.IncreaseUsage(2);
            usageStatFive.IncreaseUsage();

            yield return usageStatOne;
            yield return usageStatTwo;
            yield return usageStatThree;
            yield return usageStatFour;
            yield return usageStatFive;
        }
    }
}
