using AddressValidation.Domain.Validators;

namespace AddressValidation.UnitTest.DomainTests.ValidatorTests
{
    public class BaseAddressTesterTest
    {
        private const string MockCountry1 = "Country1", MockCountry2 = "Country2", MockCountry3 = "Country3", MockCountry4 = "Country4";

        [Theory]
        [InlineData(MockCountry1)]
        [InlineData(MockCountry2)]
        [InlineData(MockCountry3)]
        [InlineData(MockCountry4)]
        public void CanValidateByCountry_WithAvailableCountry_ShouldReturnTrue(string input)
        {
            var holderMock = new Mock<IAvailableCountryHolder>();
            holderMock.Setup(h => h.GetAvailableCountries()).Returns(new List<string> { MockCountry1, MockCountry2, MockCountry3, MockCountry4 });
            var baseAddressTesterHolder = new Mock<BaseAddressTester>(holderMock.Object) { CallBase = true };
            var baseAddressTester = baseAddressTesterHolder.Object;

            var result = baseAddressTester.CanValidateByCountry(input);

            Assert.True(result);
        }

        [Theory]
        [InlineData("Country5")]
        [InlineData("Country6")]
        public void CanValidateByCountry_WithUnavailableCountry_ShouldReturnFalse(string input)
        {
            var holderMock = new Mock<IAvailableCountryHolder>();
            holderMock.Setup(h => h.GetAvailableCountries()).Returns(new List<string> { MockCountry1, MockCountry2, MockCountry3, MockCountry4 });
            var baseAddressTesterHolder = new Mock<BaseAddressTester>(holderMock.Object) { CallBase = true };
            var baseAddressTester = baseAddressTesterHolder.Object;

            var result = baseAddressTester.CanValidateByCountry(input);

            Assert.False(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void CanValidateByCountry_WithEmptyCountry_ShouldReturnFalse(string input)
        {
            var holderMock = new Mock<IAvailableCountryHolder>();
            holderMock.Setup(h => h.GetAvailableCountries()).Returns(new List<string> { MockCountry1, MockCountry2, MockCountry3, MockCountry4 });
            var baseAddressTesterHolder = new Mock<BaseAddressTester>(holderMock.Object) { CallBase = true };
            var baseAddressTester = baseAddressTesterHolder.Object;

            var result = baseAddressTester.CanValidateByCountry(input);

            Assert.False(result);
        }
    }
}
