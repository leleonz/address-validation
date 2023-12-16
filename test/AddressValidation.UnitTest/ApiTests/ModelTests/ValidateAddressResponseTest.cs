using AddressValidation.Api.Models.Responses;

namespace AddressValidation.UnitTest.ApiTests.ModelTests
{
    public class ValidateAddressResponseTest
    {
        [Fact]
        public void ConstructValidateAddressResponse_WithAllAddressParts_ShouldHaveAddressBreakdown()
        {
            var response = new ValidateAddressResponse(It.IsAny<string>(), It.IsAny<bool>(), "Street", "Suburb", "State", "PostCode", "Country");

            Assert.NotNull(response);
            Assert.NotNull(response.AddressBreakdown);
        }

        [Fact]
        public void ConstructValidateAddressResponse_WithOnlyStreet_ShouldHaveAddressBreakdown()
        {
            var response = new ValidateAddressResponse(It.IsAny<string>(), It.IsAny<bool>(), "Street");

            Assert.NotNull(response);
            Assert.NotNull(response.AddressBreakdown);
        }

        [Fact]
        public void ConstructValidateAddressResponse_WithOnlySuburb_ShouldHaveAddressBreakdown()
        {
            var response = new ValidateAddressResponse(It.IsAny<string>(), It.IsAny<bool>(), suburb: "Suburb");

            Assert.NotNull(response);
            Assert.NotNull(response.AddressBreakdown);
        }

        [Fact]
        public void ConstructValidateAddressResponse_WithOnlyState_ShouldHaveAddressBreakdown()
        {
            var response = new ValidateAddressResponse(It.IsAny<string>(), It.IsAny<bool>(), state: "State");

            Assert.NotNull(response);
            Assert.NotNull(response.AddressBreakdown);
        }

        [Fact]
        public void ConstructValidateAddressResponse_WithOnlyPostCode_ShouldHaveAddressBreakdown()
        {
            var response = new ValidateAddressResponse(It.IsAny<string>(), It.IsAny<bool>(), postCode: "PostCode");

            Assert.NotNull(response);
            Assert.NotNull(response.AddressBreakdown);
        }

        [Fact]
        public void ConstructValidateAddressResponse_WithOnlyCountry_ShouldHaveAddressBreakdown()
        {
            var response = new ValidateAddressResponse(It.IsAny<string>(), It.IsAny<bool>(), country: "Country");

            Assert.NotNull(response);
            Assert.NotNull(response.AddressBreakdown);
        }

        [Fact]
        public void ConstructValidateAddressResponse_WithAllEmptyAddressParts_ShouldHaveNullAddressBreakdown()
        {
            var response = new ValidateAddressResponse(It.IsAny<string>(), It.IsAny<bool>(), null, null, null, null, null);

            Assert.NotNull(response);
            Assert.Null(response.AddressBreakdown);
        }
    }
}
