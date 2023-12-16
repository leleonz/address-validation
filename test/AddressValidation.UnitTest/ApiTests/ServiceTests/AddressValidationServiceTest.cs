using AddressValidation.Api.Factories.Interfaces;
using AddressValidation.Api.Services;
using AddressValidation.Data.Repositories.Interfaces;
using AddressValidation.Domain.Models;
using AddressValidation.Domain.Validators.Interfaces;
using AddressValidation.UnitTest.DataTests.RepositoryTests;
using AddressValidation.Api.Models.Requests;

namespace AddressValidation.UnitTest.ApiTests.ServiceTests
{
    public class AddressValidationServiceTest
    {
        private const string StateOne = "State ONE", StateTwo = "State 2", StateThree = "S3";

        #region Get usage count
        [Theory]
        [InlineData(StateOne, 2)]
        [InlineData("state one", 2)]
        [InlineData(StateTwo, 2)]
        [InlineData(StateThree, 1)]
        public async Task GetUsageCountByStateAsync_UsingExistingStateName_ShouldReturnCountPerUniqueAddress(string input, int expectedCount)
        {
            var statList = new List<UsageStatistic>
            {
                new UsageStatistic(new Address("Country", "suburb", StateOne, "1234", "street 88")),
                new UsageStatistic(new Address("Country", "Suburb", StateTwo, "1234", "STREET 1")),
                new UsageStatistic(new Address("Country", "SUBURB", StateOne, "4567")),
                new UsageStatistic(new Address("Country", null, StateTwo, "5678", "High Street")),
                new UsageStatistic(new Address("Country", "SUBurb", StateThree, "8945")),
            }.AsQueryable();

            var fakeStatList = new Mock<IQueryable<UsageStatistic>>();
            fakeStatList.As<IAsyncEnumerable<UsageStatistic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestDbAsyncEnumerator<UsageStatistic>(statList.GetEnumerator()));

            fakeStatList.As<IQueryable<UsageStatistic>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<UsageStatistic>(statList.Provider));

            fakeStatList.As<IQueryable<UsageStatistic>>().Setup(m => m.Expression).Returns(statList.Expression);
            fakeStatList.As<IQueryable<UsageStatistic>>().Setup(m => m.ElementType).Returns(statList.ElementType);
            fakeStatList.As<IQueryable<UsageStatistic>>().Setup(m => m.GetEnumerator()).Returns(() => statList.GetEnumerator());

            var addressValidatorSelectorMock = new Mock<IAddressValidatorSelector>();
            var usageStatisticRepositoryMock = new Mock<IUsageStatisticRepository>();
            usageStatisticRepositoryMock.Setup(repo => repo.GetAll()).Returns(fakeStatList.Object);
            var addressValidationService = new AddressValidationService(addressValidatorSelectorMock.Object, usageStatisticRepositoryMock.Object);

            var result = await addressValidationService.GetUsageCountByStateAsync(input);

            Assert.Equal(expectedCount, result);
        }

        [Fact]
        public async Task GetUsageCountByStateAsync_UsingNotFoundStateName_ShouldReturnZero()
        {
            const string notFoundStateName = "Unknown State";
            var statList = new List<UsageStatistic>
            {
                new UsageStatistic(new Address("Country", "suburb", StateOne, "1234", "street 88")),
                new UsageStatistic(new Address("Country", "Suburb", StateTwo, "1234", "STREET 1")),
                new UsageStatistic(new Address("Country", "SUBURB", StateOne, "4567")),
                new UsageStatistic(new Address("Country", null, StateTwo, "5678", "High Street")),
                new UsageStatistic(new Address("Country", "SUBurb", StateThree, "8945")),
            }.AsQueryable();

            var fakeStatList = new Mock<IQueryable<UsageStatistic>>();
            fakeStatList.As<IAsyncEnumerable<UsageStatistic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestDbAsyncEnumerator<UsageStatistic>(statList.GetEnumerator()));

            fakeStatList.As<IQueryable<UsageStatistic>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<UsageStatistic>(statList.Provider));

            fakeStatList.As<IQueryable<UsageStatistic>>().Setup(m => m.Expression).Returns(statList.Expression);
            fakeStatList.As<IQueryable<UsageStatistic>>().Setup(m => m.ElementType).Returns(statList.ElementType);
            fakeStatList.As<IQueryable<UsageStatistic>>().Setup(m => m.GetEnumerator()).Returns(() => statList.GetEnumerator());

            var addressValidatorSelectorMock = new Mock<IAddressValidatorSelector>();
            var usageStatisticRepositoryMock = new Mock<IUsageStatisticRepository>();
            usageStatisticRepositoryMock.Setup(repo => repo.GetAll()).Returns(fakeStatList.Object);
            var addressValidationService = new AddressValidationService(addressValidatorSelectorMock.Object, usageStatisticRepositoryMock.Object);

            var result = await addressValidationService.GetUsageCountByStateAsync(notFoundStateName);

            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task GetUsageCountByStateAsync_UsingEmptyStateName_ShouldReturnZero(string input)
        {
            var statList = Enumerable.Empty<UsageStatistic>().AsQueryable();

            var fakeStatList = new Mock<IQueryable<UsageStatistic>>();
            fakeStatList.As<IAsyncEnumerable<UsageStatistic>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestDbAsyncEnumerator<UsageStatistic>(statList.GetEnumerator()));

            fakeStatList.As<IQueryable<UsageStatistic>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<UsageStatistic>(statList.Provider));

            fakeStatList.As<IQueryable<UsageStatistic>>().Setup(m => m.Expression).Returns(statList.Expression);
            fakeStatList.As<IQueryable<UsageStatistic>>().Setup(m => m.ElementType).Returns(statList.ElementType);
            fakeStatList.As<IQueryable<UsageStatistic>>().Setup(m => m.GetEnumerator()).Returns(() => statList.GetEnumerator());

            var addressValidatorSelectorMock = new Mock<IAddressValidatorSelector>();
            var usageStatisticRepositoryMock = new Mock<IUsageStatisticRepository>();
            usageStatisticRepositoryMock.Setup(repo => repo.GetAll()).Returns(fakeStatList.Object);
            var addressValidationService = new AddressValidationService(addressValidatorSelectorMock.Object, usageStatisticRepositoryMock.Object);

            var result = await addressValidationService.GetUsageCountByStateAsync(input);

            Assert.Equal(0, result);
        }
        #endregion

        #region Validate address
        [Fact]
        public async Task ValidateAddressAsync_SingleAddress_ShouldReturnSingleValidateAddressResponse()
        {
            var addressValidatorMock = new Mock<IAddressValidator>();
            addressValidatorMock.Setup(v => v.ValidateAddressAsync(It.IsAny<string>())).ReturnsAsync(new AddressValidationResult(It.IsAny<string>(), new Address(It.IsAny<string>())));

            var addressValidatorSelectorMock = new Mock<IAddressValidatorSelector>();
            addressValidatorSelectorMock.Setup(s => s.GetAddressValidatorByAddress(It.IsAny<string>())).Returns(addressValidatorMock.Object);

            var usageStatisticRepositoryMock = new Mock<IUsageStatisticRepository>();
            usageStatisticRepositoryMock.Setup(repo => repo.Find(It.IsAny<Address>())).Returns(default(UsageStatistic));

            var addressValidationService = new AddressValidationService(addressValidatorSelectorMock.Object, usageStatisticRepositoryMock.Object);

            var request = new ValidateAddressesRequest() { RawAddresses = new List<string> { It.IsAny<string>() } };
            var results = await addressValidationService.ValidateAddressAsync(request);

            Assert.NotNull(results);
            Assert.Single(results);
            usageStatisticRepositoryMock.Verify(repo => repo.AddOrUpdateAsync(It.IsAny<UsageStatistic>()), Times.Once);
            usageStatisticRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task ValidateAddressAsync_MultipleAddresses_ShouldReturnListOfValidateAddressResponses()
        {
            const int expectedCount = 3;

            var addressValidatorMock = new Mock<IAddressValidator>();
            addressValidatorMock.Setup(v => v.ValidateAddressAsync(It.IsAny<string>())).ReturnsAsync(new AddressValidationResult(It.IsAny<string>(), new Address(It.IsAny<string>())));

            var addressValidatorSelectorMock = new Mock<IAddressValidatorSelector>();
            addressValidatorSelectorMock.Setup(s => s.GetAddressValidatorByAddress(It.IsAny<string>())).Returns(addressValidatorMock.Object);

            var usageStatisticRepositoryMock = new Mock<IUsageStatisticRepository>();
            usageStatisticRepositoryMock.Setup(repo => repo.Find(It.IsAny<Address>())).Returns(default(UsageStatistic));

            var addressValidationService = new AddressValidationService(addressValidatorSelectorMock.Object, usageStatisticRepositoryMock.Object);

            var request = new ValidateAddressesRequest() { RawAddresses = new List<string> { It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>() } };
            var results = await addressValidationService.ValidateAddressAsync(request);

            Assert.NotNull(results);
            Assert.True(results.Count() == expectedCount);
            usageStatisticRepositoryMock.Verify(repo => repo.AddOrUpdateAsync(It.IsAny<UsageStatistic>()), Times.Exactly(expectedCount));
            usageStatisticRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }

        [Fact]
        public async Task ValidateAddressAsync_SingleExistingAddress_ShouldReturnResponseAndIncreaseUsageCount()
        {
            var addressMock = new Address(It.IsAny<string>());

            var addressValidatorMock = new Mock<IAddressValidator>();
            addressValidatorMock.Setup(v => v.ValidateAddressAsync(It.IsAny<string>())).ReturnsAsync(new AddressValidationResult(It.IsAny<string>(), addressMock));

            var addressValidatorSelectorMock = new Mock<IAddressValidatorSelector>();
            addressValidatorSelectorMock.Setup(s => s.GetAddressValidatorByAddress(It.IsAny<string>())).Returns(addressValidatorMock.Object);

            var existingStat = new UsageStatistic(addressMock);
            var initialUsageCount = existingStat.UsageCount;
            var usageStatisticRepositoryMock = new Mock<IUsageStatisticRepository>();
            usageStatisticRepositoryMock.Setup(repo => repo.Find(It.IsAny<Address>())).Returns(existingStat);

            var addressValidationService = new AddressValidationService(addressValidatorSelectorMock.Object, usageStatisticRepositoryMock.Object);

            var request = new ValidateAddressesRequest() { RawAddresses = new List<string> { It.IsAny<string>() } };
            var results = await addressValidationService.ValidateAddressAsync(request);

            Assert.NotNull(results);
            Assert.Single(results);
            var result = results.First();
            Assert.True(result.IsValid);
            Assert.NotNull(result.AddressBreakdown);

            Assert.True(initialUsageCount + 1 == existingStat.UsageCount);
            usageStatisticRepositoryMock.Verify(repo => repo.AddOrUpdateAsync(It.IsAny<UsageStatistic>()), Times.Once);
            usageStatisticRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }

        [Theory]
        [InlineData("Stanhope Pkwy & Sentry Dr, Stanhope Gardens NSW 2768, Australia", "Stanhope Pkwy & Sentry Dr", "Stanhope Gardens", "New South Wales", "2768", "Australia")]
        [InlineData("175 Suffolk St, Caversham WA 6055, Australia", "175 Suffolk St", "Caversham", "West Australia", "6055", "Australia")]
        [InlineData("Casula NSW 2170, Australia", null, "Casula", "New South Wales", "2170", "Australia")]
        public async Task ValidateAddressAsync_IsValidResponse_ShouldReturnResponseMatchingAddressResult(string input,
            string? expectedStreet, string? expectedSuburb, string? expectedStateName, string? expectedPostCode, string expectedCountry)
        {
            var addressMock = new Address(expectedCountry, expectedSuburb, expectedStateName, expectedPostCode, expectedStreet);

            var addressValidatorMock = new Mock<IAddressValidator>();
            addressValidatorMock.Setup(v => v.ValidateAddressAsync(It.IsAny<string>())).ReturnsAsync(new AddressValidationResult(input, addressMock));

            var addressValidatorSelectorMock = new Mock<IAddressValidatorSelector>();
            addressValidatorSelectorMock.Setup(s => s.GetAddressValidatorByAddress(It.IsAny<string>())).Returns(addressValidatorMock.Object);

            var existingStat = new UsageStatistic(addressMock);
            var initialUsageCount = existingStat.UsageCount;
            var usageStatisticRepositoryMock = new Mock<IUsageStatisticRepository>();
            usageStatisticRepositoryMock.Setup(repo => repo.Find(It.IsAny<Address>())).Returns(existingStat);

            var addressValidationService = new AddressValidationService(addressValidatorSelectorMock.Object, usageStatisticRepositoryMock.Object);

            var request = new ValidateAddressesRequest() { RawAddresses = new List<string> { input } };
            var results = await addressValidationService.ValidateAddressAsync(request);

            Assert.NotNull(results);
            Assert.Single(results);

            var result = results.First();
            Assert.Equal(input, result.ValidatedAddress);
            Assert.True(result.IsValid);

            Assert.NotNull(result.AddressBreakdown);
            var addressBreakdown = result.AddressBreakdown;
            Assert.Equal(expectedStreet, addressBreakdown.Street);
            Assert.Equal(expectedSuburb, addressBreakdown.Suburb);
            Assert.Equal(expectedStateName, addressBreakdown.State);
            Assert.Equal(expectedPostCode, addressBreakdown.PostCode);
            Assert.Equal(expectedCountry, addressBreakdown.Country);
        }

        [Fact]
        public async Task ValidateAddressAsync_InvalidResponse_ShouldReturnResponse_IsValid_False_AddressBreakdown_Null()
        {
            var input = "random string";

            var addressValidatorMock = new Mock<IAddressValidator>();
            addressValidatorMock.Setup(v => v.ValidateAddressAsync(It.IsAny<string>())).ReturnsAsync(new AddressValidationResult(input));

            var addressValidatorSelectorMock = new Mock<IAddressValidatorSelector>();
            addressValidatorSelectorMock.Setup(s => s.GetAddressValidatorByAddress(It.IsAny<string>())).Returns(addressValidatorMock.Object);

            var usageStatisticRepositoryMock = new Mock<IUsageStatisticRepository>();
            var addressValidationService = new AddressValidationService(addressValidatorSelectorMock.Object, usageStatisticRepositoryMock.Object);

            var request = new ValidateAddressesRequest() { RawAddresses = new List<string> { input } };
            var results = await addressValidationService.ValidateAddressAsync(request);

            Assert.NotNull(results);
            Assert.Single(results);

            var result = results.First();
            Assert.Equal(input, result.ValidatedAddress);
            Assert.False(result.IsValid);
            Assert.Null(result.AddressBreakdown);

            usageStatisticRepositoryMock.Verify(repo => repo.AddOrUpdateAsync(It.IsAny<UsageStatistic>()), Times.Never);
            usageStatisticRepositoryMock.Verify(repo => repo.Save(), Times.Once);
        }
        #endregion
    }
}
