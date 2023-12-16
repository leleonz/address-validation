using AddressValidation.Domain.Services.Interfaces;
using AddressValidation.Domain.Validators;

namespace AddressValidation.UnitTest.DomainTests.ValidatorTests.AddressValidatorTests
{
    public class BasicAustraliaAddressValidatorTest
    {
        private const string Australia = "Australia";
        private const string MockStateCodeOne = "s1", MockStateCodeTwo = "S2", MockStateCodeThree = "s3";
        private const string MockStateOne = "State One", MockStateTwo = "State Two", MockStateThree = "State Three";
        private const string MockSuburbOne = "Suburb One", MockSuburbTwo = "Suburb Two", MockSuburbThree = "Suburb Three";

        #region Validate Country
        [Fact]
        public async Task ValidateCountryAsync_UsingAustralia_ShouldReturnTrueWithCountryName()
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidateCountryAsync(Australia);

            Assert.True(result.IsValid);
            Assert.NotNull(result.Country);
            Assert.Equal(AvailableCountry.Australia.ToUpper(), result.Country.ToUpper());
        }

        [Theory]
        [InlineData("Random")]
        [InlineData("Random123")]
        [InlineData("123456")]
        [InlineData("123!@#")]
        public async Task ValidateCountryAsync_UsingRandomString_ShouldReturnFalseWithEmptyCountryName(string input)
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidateCountryAsync(input);

            Assert.False(result.IsValid);
            Assert.NotNull(result.Country);
            Assert.Empty(result.Country);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task ValidateCountryAsync_UsingEmptyString_ShouldReturnFalseWithEmptyCountryName(string input)
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidateCountryAsync(input);

            Assert.False(result.IsValid);
            Assert.NotNull(result.Country);
            Assert.Empty(result.Country);
        }
        #endregion

        #region Validate PostCode
        [Theory]
        [InlineData("1234")]
        [InlineData("7801")]
        public async Task ValidatePostCodeAsync_UsingFourDigitsOnlyString_ShouldReturnTrueWithPostCode(string input)
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidatePostCodeAsync(input);

            Assert.True(result.IsValid);
            Assert.NotNull(result.PostCode);
            Assert.Equal(input, result.PostCode);
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("Random")]
        [InlineData("Random123")]
        [InlineData("123!@#")]
        public async Task ValidatePostCodeAsync_UsingRandomString_ShouldReturnFalseWithEmptyPostCode(string input)
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidatePostCodeAsync(input);

            Assert.False(result.IsValid);
            Assert.NotNull(result.PostCode);
            Assert.Empty(result.PostCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task ValidatePostCodeAsync_UsingEmptyString_ShouldReturnFalseWithEmptyPostCode(string input)
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidatePostCodeAsync(input);

            Assert.False(result.IsValid);
            Assert.NotNull(result.PostCode);
            Assert.Empty(result.PostCode);
        }
        #endregion

        #region Validate State
        [Theory]
        [InlineData(MockStateCodeOne, MockStateOne)]
        [InlineData(MockStateCodeTwo, MockStateTwo)]
        [InlineData(MockStateCodeThree, MockStateThree)]
        public async Task ValidateStateCodeAsync_UsingAvailableStateCode_ShouldReturnTrueWithStateName(string input, string expectedStateName)
        {
            var fakeStateData = new Dictionary<string, string>
            {
                [MockStateCodeOne] = MockStateOne,
                [MockStateCodeTwo] = MockStateTwo,
                [MockStateCodeThree] = MockStateThree,
            };
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            addressDataSetServiceMock.Setup(s => s.GetStatesByCountryAsync(AvailableCountry.Australia)).ReturnsAsync(fakeStateData);
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidateStateCodeAsync(input);

            Assert.True(result.IsValid);
            Assert.NotNull(result.State);
            Assert.Equal(expectedStateName.ToUpper(), result.State.ToUpper());
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("Random")]
        [InlineData("Random123")]
        [InlineData("123!@#")]
        public async Task ValidateStateCodeAsync_UsingRandomString_ShouldReturnFalseWithEmptyStateName(string input)
        {
            var fakeStateData = new Dictionary<string, string>
            {
                [MockStateCodeOne] = MockStateOne,
                [MockStateCodeTwo] = MockStateTwo,
                [MockStateCodeThree] = MockStateThree,
            };
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            addressDataSetServiceMock.Setup(s => s.GetStatesByCountryAsync(AvailableCountry.Australia)).ReturnsAsync(fakeStateData);
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidateStateCodeAsync(input);

            Assert.False(result.IsValid);
            Assert.NotNull(result.State);
            Assert.Empty(result.State);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task ValidateStateCodeAsync_UsingEmptyString_ShouldReturnFalseWithEmptyStateName(string input)
        {
            var fakeStateData = new Dictionary<string, string>
            {
                [MockStateCodeOne] = MockStateOne,
                [MockStateCodeTwo] = MockStateTwo,
                [MockStateCodeThree] = MockStateThree,
            };
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            addressDataSetServiceMock.Setup(s => s.GetStatesByCountryAsync(AvailableCountry.Australia)).ReturnsAsync(fakeStateData);
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidateStateCodeAsync(input);

            Assert.False(result.IsValid);
            Assert.NotNull(result.State);
            Assert.Empty(result.State);
        }
        #endregion

        #region Validate Suburb
        [Theory]
        [InlineData(MockSuburbOne, MockSuburbOne)]
        [InlineData(MockSuburbTwo, MockSuburbTwo)]
        [InlineData(MockSuburbThree, MockSuburbThree)]
        public async Task ValidateSuburbAsync_UsingAvailableSuburb_ShouldReturnTrueWithSuburbName(string input, string expectedStateName)
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            addressDataSetServiceMock.Setup(s => s.GetSuburbsByCountryAsync(AvailableCountry.Australia)).ReturnsAsync(new List<string> { MockSuburbOne, MockSuburbTwo, MockSuburbThree });
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidateSuburbAsync(input);

            Assert.True(result.IsValid);
            Assert.NotNull(result.Suburb);
            Assert.Equal(expectedStateName.ToUpper(), result.Suburb.ToUpper());
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("Suburb Four")]
        [InlineData("Random123")]
        [InlineData("123!@#")]
        public async Task ValidateSuburbAsync_UsingRandomString_ShouldReturnFalseWithEmptySuburbName(string input)
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            addressDataSetServiceMock.Setup(s => s.GetSuburbsByCountryAsync(AvailableCountry.Australia)).ReturnsAsync(new List<string> { MockSuburbOne, MockSuburbTwo, MockSuburbThree });
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidateSuburbAsync(input);

            Assert.False(result.IsValid);
            Assert.NotNull(result.Suburb);
            Assert.Empty(result.Suburb);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task ValidateSuburbAsync_UsingEmptyString_ShouldReturnFalseWithEmptySuburbName(string input)
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            addressDataSetServiceMock.Setup(s => s.GetSuburbsByCountryAsync(AvailableCountry.Australia)).ReturnsAsync(new List<string> { MockSuburbOne, MockSuburbTwo, MockSuburbThree });
            var basicAustraliaAddressValidator = new BasicAustraliaAddressValidator(addressDataSetServiceMock.Object);

            var result = await basicAustraliaAddressValidator.ValidateSuburbAsync(input);

            Assert.False(result.IsValid);
            Assert.NotNull(result.Suburb);
            Assert.Empty(result.Suburb);
        }
        #endregion

        #region Validate Address
        [Theory]
        [InlineData("Stanhope Pkwy & Sentry Dr, Stanhope Gardens NSW 2768, Australia", "Stanhope Pkwy & Sentry Dr")]
        [InlineData("175 Suffolk St, Caversham WA 6055, Australia", "175 Suffolk St")]
        [InlineData("Casula NSW 2170, Australia", null)]
        public async Task ValidateAddressAsync_UsingValidAddress_ShouldReturnAddressValidationResult_IsValid_True_ValidAddress_NotNull(string input, string? expectedStreet)
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidatorMock = new Mock<BasicAustraliaAddressValidator>(addressDataSetServiceMock.Object) { CallBase = true };
            (bool, string?) validResult = (true, It.IsAny<string>());
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateCountryAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidatePostCodeAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateStateCodeAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateSuburbAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            var basicAustraliaAddressValidator = basicAustraliaAddressValidatorMock.Object;

            var result = await basicAustraliaAddressValidator.ValidateAddressAsync(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.ValidatedAddress);
            Assert.True(result.IsValid);
            Assert.NotNull(result.ValidAddress);
            Assert.Equal(expectedStreet, result.ValidAddress.Street);
        }

        [Theory]
        [InlineData("Stanhope Pkwy & Sentry Dr, Stanhope Pkwy, Stanhope Gardens NSW 2768, Australia", "Stanhope Pkwy & Sentry Dr, Stanhope Pkwy")]
        [InlineData("175 Suffolk St, 175, Suffolk, St, Caversham WA 6055, Australia", "175 Suffolk St, 175, Suffolk, St")]
        public async Task
            ValidateAddressAsync_UsingValidAddressWithMoreThanThreeSections_ShouldReturnAddressValidationResult_IsValid_True_JoinAllExceptLastTwoSections_As_ValidAddressStreet(string input, string expectedStreet)
        {
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidatorMock = new Mock<BasicAustraliaAddressValidator>(addressDataSetServiceMock.Object) { CallBase = true };
            (bool, string?) validResult = (true, It.IsAny<string>());
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateCountryAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidatePostCodeAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateStateCodeAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateSuburbAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            var basicAustraliaAddressValidator = basicAustraliaAddressValidatorMock.Object;

            var result = await basicAustraliaAddressValidator.ValidateAddressAsync(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.ValidatedAddress);
            Assert.True(result.IsValid);
            Assert.NotNull(result.ValidAddress);
            Assert.Equal(expectedStreet, result.ValidAddress.Street);
        }

        [Theory]
        [InlineData("fake street, fake suburb STATE 1324, Country")]
        [InlineData("street, suburb STATE 1324, Country")]
        [InlineData("suburb STATE 1324, Country")]
        public async Task ValidateAddressAsync_UsingValidFormatInvalidAddress_ShouldReturnAddressValidationResult_IsValid_False_ValidAddress_Null(string input)
        {

            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidatorMock = new Mock<BasicAustraliaAddressValidator>(addressDataSetServiceMock.Object) { CallBase = true };
            (bool, string?) invalidResult = (false, string.Empty);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateCountryAsync(It.IsAny<string>())).ReturnsAsync(invalidResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidatePostCodeAsync(It.IsAny<string>())).ReturnsAsync(invalidResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateStateCodeAsync(It.IsAny<string>())).ReturnsAsync(invalidResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateSuburbAsync(It.IsAny<string>())).ReturnsAsync(invalidResult);
            var basicAustraliaAddressValidator = basicAustraliaAddressValidatorMock.Object;

            var result = await basicAustraliaAddressValidator.ValidateAddressAsync(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.ValidatedAddress);
            Assert.False(result.IsValid);
            Assert.Null(result.ValidAddress);
        }

        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task ValidateAddressAsync_UsingEmptyAddress_ShouldReturnAddressValidationResult_IsValid_False_ValidAddress_Null(string input)
        {

            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidatorMock = new Mock<BasicAustraliaAddressValidator>(addressDataSetServiceMock.Object) { CallBase = true };
            var basicAustraliaAddressValidator = basicAustraliaAddressValidatorMock.Object;

            var result = await basicAustraliaAddressValidator.ValidateAddressAsync(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.ValidatedAddress);
            Assert.False(result.IsValid);
            Assert.Null(result.ValidAddress);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateCountryAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidatePostCodeAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateStateCodeAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateSuburbAsync(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("Country")]
        [InlineData("1324 Country")]
        [InlineData("suburb STATE 1324 Country")]
        public async Task ValidateAddressAsync_UsingAddressWithOneSection_ShouldReturnAddressValidationResult_IsValid_False_ValidAddress_Null(string input)
        {

            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidatorMock = new Mock<BasicAustraliaAddressValidator>(addressDataSetServiceMock.Object) { CallBase = true };
            var basicAustraliaAddressValidator = basicAustraliaAddressValidatorMock.Object;

            var result = await basicAustraliaAddressValidator.ValidateAddressAsync(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.ValidatedAddress);
            Assert.False(result.IsValid);
            Assert.Null(result.ValidAddress);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateCountryAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidatePostCodeAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateStateCodeAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateSuburbAsync(It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("1234, Country")]
        [InlineData("state 1324, Country")]
        public async Task ValidateAddressAsync_UsingAddressWithoutThreeMiddleSection_ShouldReturnAddressValidationResult_IsValid_False_ValidAddress_Null(string input)
        {

            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidatorMock = new Mock<BasicAustraliaAddressValidator>(addressDataSetServiceMock.Object) { CallBase = true };
            (bool, string?) validResult = (true, It.IsAny<string>());
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateCountryAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            var basicAustraliaAddressValidator = basicAustraliaAddressValidatorMock.Object;

            var result = await basicAustraliaAddressValidator.ValidateAddressAsync(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.ValidatedAddress);
            Assert.False(result.IsValid);
            Assert.Null(result.ValidAddress);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateCountryAsync(It.IsAny<string>()), Times.Once);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidatePostCodeAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateStateCodeAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateSuburbAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ValidateAddressAsync_UsingValidAddressWithUnavailableCountry_ShouldReturnAddressValidationResult_IsValid_False_ValidAddress_Null()
        {
            const string input = "Casula NSW 2170, Some Country";
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidatorMock = new Mock<BasicAustraliaAddressValidator>(addressDataSetServiceMock.Object) { CallBase = true };
            (bool, string?) invalidResult = (false, string.Empty);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateCountryAsync(It.IsAny<string>())).ReturnsAsync(invalidResult);
            var basicAustraliaAddressValidator = basicAustraliaAddressValidatorMock.Object;

            var result = await basicAustraliaAddressValidator.ValidateAddressAsync(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.ValidatedAddress);
            Assert.False(result.IsValid);
            Assert.Null(result.ValidAddress);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateCountryAsync(It.IsAny<string>()), Times.Once);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidatePostCodeAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateStateCodeAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateSuburbAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ValidateAddressAsync_UsingValidAddressWithInvalidPostCode_ShouldReturnAddressValidationResult_IsValid_False_ValidAddress_Null()
        {
            const string input = "Casula NSW 2170, Some Country";
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidatorMock = new Mock<BasicAustraliaAddressValidator>(addressDataSetServiceMock.Object) { CallBase = true };
            (bool, string?) validResult = (true, It.IsAny<string>());
            (bool, string?) invalidResult = (false, string.Empty);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateCountryAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidatePostCodeAsync(It.IsAny<string>())).ReturnsAsync(invalidResult);
            var basicAustraliaAddressValidator = basicAustraliaAddressValidatorMock.Object;

            var result = await basicAustraliaAddressValidator.ValidateAddressAsync(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.ValidatedAddress);
            Assert.False(result.IsValid);
            Assert.Null(result.ValidAddress);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateCountryAsync(It.IsAny<string>()), Times.Once);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidatePostCodeAsync(It.IsAny<string>()), Times.Once);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateStateCodeAsync(It.IsAny<string>()), Times.Never);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateSuburbAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ValidateAddressAsync_UsingValidAddressWithUnavailableStateCode_ShouldReturnAddressValidationResult_IsValid_False_ValidAddress_Null()
        {
            const string input = "Casula NSW 2170, Some Country";
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidatorMock = new Mock<BasicAustraliaAddressValidator>(addressDataSetServiceMock.Object) { CallBase = true };
            (bool, string?) validResult = (true, It.IsAny<string>());
            (bool, string?) invalidResult = (false, string.Empty);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateCountryAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidatePostCodeAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateStateCodeAsync(It.IsAny<string>())).ReturnsAsync(invalidResult);
            var basicAustraliaAddressValidator = basicAustraliaAddressValidatorMock.Object;

            var result = await basicAustraliaAddressValidator.ValidateAddressAsync(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.ValidatedAddress);
            Assert.False(result.IsValid);
            Assert.Null(result.ValidAddress);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateCountryAsync(It.IsAny<string>()), Times.Once);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidatePostCodeAsync(It.IsAny<string>()), Times.Once);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateStateCodeAsync(It.IsAny<string>()), Times.Once);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateSuburbAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ValidateAddressAsync_UsingValidAddressWithUnavailableSuburb_ShouldReturnAddressValidationResult_IsValid_False_ValidAddress_Null()
        {
            const string input = "Casula NSW 2170, Some Country";
            var addressDataSetServiceMock = new Mock<IAddressDataSetService>();
            var basicAustraliaAddressValidatorMock = new Mock<BasicAustraliaAddressValidator>(addressDataSetServiceMock.Object) { CallBase = true };
            (bool, string?) validResult = (true, It.IsAny<string>());
            (bool, string?) invalidResult = (false, string.Empty);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateCountryAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidatePostCodeAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateStateCodeAsync(It.IsAny<string>())).ReturnsAsync(validResult);
            basicAustraliaAddressValidatorMock.Setup(v => v.ValidateSuburbAsync(It.IsAny<string>())).ReturnsAsync(invalidResult);
            var basicAustraliaAddressValidator = basicAustraliaAddressValidatorMock.Object;

            var result = await basicAustraliaAddressValidator.ValidateAddressAsync(input);

            Assert.NotNull(result);
            Assert.Equal(input, result.ValidatedAddress);
            Assert.False(result.IsValid);
            Assert.Null(result.ValidAddress);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateCountryAsync(It.IsAny<string>()), Times.Once);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidatePostCodeAsync(It.IsAny<string>()), Times.Once);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateStateCodeAsync(It.IsAny<string>()), Times.Once);
            basicAustraliaAddressValidatorMock.Verify(v => v.ValidateSuburbAsync(It.IsAny<string>()), Times.Once);
        }
        #endregion
    }
}
