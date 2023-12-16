using AddressValidation.Domain.Validators;

namespace AddressValidation.UnitTest.DomainTests.ValidatorTests
{
    public class BasicAddressTesterTest
    {
        private const string MockCountry1 = "Country1", MockCountry2 = "Country2", MockCountry3 = "Country3", MockCountry4 = "Country4";

        [Theory]
        [InlineData($"{MockCountry1}", MockCountry1)]
        [InlineData($"PostCode2, {MockCountry2}", MockCountry2)]
        [InlineData($"PostCode2,{MockCountry2}", MockCountry2)]
        [InlineData($"Street3, PostCode3, {MockCountry3}", MockCountry3)]
        [InlineData($"Street3,PostCode3,{MockCountry3}", MockCountry3)]
        [InlineData($"Street4, PostCode4, {MockCountry4}", MockCountry4)]
        [InlineData($"Street4,PostCode4,{MockCountry4}", MockCountry4)]
        public void TryGetCountryFromAddress_WithAvailableCountry_ShouldReturnTrue(string input, string expectedCountry)
        {
            var holderMock = new Mock<IAvailableCountryHolder>();
            holderMock.Setup(h => h.GetAvailableCountries()).Returns(new List<string> { MockCountry1, MockCountry2, MockCountry3, MockCountry4 });
            var basicAddressTester = new BasicAddressTester(holderMock.Object);

            var result = basicAddressTester.CanValidateAddress(input, out var country);

            Assert.True(result);
            Assert.Equal(expectedCountry.ToUpper(), country.ToUpper());
        }

        [Theory]
        [InlineData($"Street5, PostCode5, Country5")]
        [InlineData($"Street6,PostCode6,Country6")]
        public void TryGetCountryFromAddress_WithUnavailableCountry_ShouldReturnFalse(string input)
        {
            var holderMock = new Mock<IAvailableCountryHolder>();
            holderMock.Setup(h => h.GetAvailableCountries()).Returns(new List<string> { MockCountry1, MockCountry2, MockCountry3, MockCountry4 });
            var basicAddressTester = new BasicAddressTester(holderMock.Object);

            var result = basicAddressTester.CanValidateAddress(input, out var country);

            Assert.False(result);
            Assert.Empty(country);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void TryGetCountryFromAddress_WithEmptyAddress_ShouldReturnFalse(string input)
        {
            var holderMock = new Mock<IAvailableCountryHolder>();
            var basicAddressTester = new BasicAddressTester(holderMock.Object);

            var result = basicAddressTester.CanValidateAddress(input, out var country);

            Assert.False(result);
            Assert.Empty(country);
        }
    }
}
