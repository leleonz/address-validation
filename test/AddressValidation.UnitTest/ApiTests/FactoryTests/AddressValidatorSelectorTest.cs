using AddressValidation.Api.Factories;
using AddressValidation.Domain.Validators.Interfaces;

namespace AddressValidation.UnitTest.ApiTests.FactoryTests
{
    public class AddressValidatorSelectorTest
    {
        private const string MockCountryOne = "Country One", MockCountryTwo = "Country Two", MockCountryThree = "Country Three";

        [Theory]
        [InlineData($"Part1, Part2, {MockCountryOne}", MockCountryOne, 1, 0, 0)]
        [InlineData("Part1,Part2,country two", MockCountryTwo, 0, 1, 0)]
        [InlineData($"Part1,Part2,{MockCountryThree}", MockCountryThree, 0, 0, 1)]
        public void GetAddressValidatorByAddress_WithValidCountry_ShouldReturnRespectiveValidator(string input, string extractedCountry,
            int expectedValidatorOneCallCount, int expectedValidatorTwoCallCount, int expectedValidatorThreeCallCount)
        {
            var addressTesterMock = new Mock<IAddressTester>();
            addressTesterMock.Setup(t => t.CanValidateAddress(It.IsAny<string>(), out extractedCountry)).Returns(true);

            var validatorOneFuncMock = new Mock<Func<IAddressValidator>>();
            var validatorTwoFuncMock = new Mock<Func<IAddressValidator>>();
            var validatorThreeFuncMock = new Mock<Func<IAddressValidator>>();
            var fakeValidatorDictionary = new Dictionary<string, Func<IAddressValidator>>
            {
                [MockCountryOne] = validatorOneFuncMock.Object,
                [MockCountryTwo] = validatorTwoFuncMock.Object,
                [MockCountryThree] = validatorThreeFuncMock.Object,
            };

            var addressValidatorSelector = new AddressValidatorSelector(fakeValidatorDictionary, addressTesterMock.Object);
            addressValidatorSelector.GetAddressValidatorByAddress(input);

            validatorOneFuncMock.Verify(x => x(), Times.Exactly(expectedValidatorOneCallCount));
            validatorTwoFuncMock.Verify(x => x(), Times.Exactly(expectedValidatorTwoCallCount));
            validatorThreeFuncMock.Verify(x => x(), Times.Exactly(expectedValidatorThreeCallCount));
        }

        [Fact]
        public void GetAddressValidatorByAddress_WithUnavailableCountry_ShouldReturnUnknownAddressValidator()
        {
            var addressTesterMock = new Mock<IAddressTester>();
            var emptyCountry = string.Empty;
            addressTesterMock.Setup(t => t.CanValidateAddress(It.IsAny<string>(), out emptyCountry)).Returns(false);

            var validatorOneFuncMock = new Mock<Func<IAddressValidator>>();
            var validatorTwoFuncMock = new Mock<Func<IAddressValidator>>();
            var validatorThreeFuncMock = new Mock<Func<IAddressValidator>>();
            var fakeValidatorDictionary = new Dictionary<string, Func<IAddressValidator>>
            {
                [MockCountryOne] = validatorOneFuncMock.Object,
                [MockCountryTwo] = validatorTwoFuncMock.Object,
                [MockCountryThree] = validatorThreeFuncMock.Object,
            };

            var addressValidatorSelector = new AddressValidatorSelector(fakeValidatorDictionary, addressTesterMock.Object);
            var result = addressValidatorSelector.GetAddressValidatorByAddress(It.IsAny<string>());

            validatorOneFuncMock.Verify(x => x(), Times.Never);
            validatorTwoFuncMock.Verify(x => x(), Times.Never);
            validatorThreeFuncMock.Verify(x => x(), Times.Never);
            Assert.True(result.GetType() == typeof(UnknownAddressValidator));
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void GetAddressValidatorByAddress_WithEmptyString_ShouldReturnUnknownAddressValidator(string input)
        {
            var addressTesterMock = new Mock<IAddressTester>();
            var validatorOneFuncMock = new Mock<Func<IAddressValidator>>();
            var validatorTwoFuncMock = new Mock<Func<IAddressValidator>>();
            var validatorThreeFuncMock = new Mock<Func<IAddressValidator>>();
            var fakeValidatorDictionary = new Dictionary<string, Func<IAddressValidator>>
            {
                [MockCountryOne] = validatorOneFuncMock.Object,
                [MockCountryTwo] = validatorTwoFuncMock.Object,
                [MockCountryThree] = validatorThreeFuncMock.Object,
            };

            var addressValidatorSelector = new AddressValidatorSelector(fakeValidatorDictionary, addressTesterMock.Object);
            var result = addressValidatorSelector.GetAddressValidatorByAddress(input);

            validatorOneFuncMock.Verify(x => x(), Times.Never);
            validatorTwoFuncMock.Verify(x => x(), Times.Never);
            validatorThreeFuncMock.Verify(x => x(), Times.Never);
            Assert.True(result.GetType() == typeof(UnknownAddressValidator));
        }
    }
}
