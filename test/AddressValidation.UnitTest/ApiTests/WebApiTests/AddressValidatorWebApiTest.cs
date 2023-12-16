using AddressValidation.Api.Endpoints;
using AddressValidation.Api.Models.Requests;
using AddressValidation.Api.Models.Responses;
using AddressValidation.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AddressValidation.UnitTest.ApiTests.WebApiTests
{
    public class AddressValidatorWebApiTest
    {
        [Fact]
        public async Task GetUsage_ShouldReturnOkWithCount()
        {
            const string stateName = "some state";
            const int expectedCount = 2;
            var addressValidationServiceMock = new Mock<IAddressValidationService>();
            addressValidationServiceMock.Setup(svc => svc.GetUsageCountByStateAsync(stateName)).ReturnsAsync(expectedCount);

            var result = await AddressValidatorWebApiBuilder.GetUsageCountByState(stateName, addressValidationServiceMock.Object);

            Assert.IsType<Ok<int>>(result);
            var okResult = (Ok<int>)result;
            Assert.Equal(expectedCount, okResult.Value);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public async Task GetUsage_WithEmptyStateName_ShouldReturnBadRequest(string input)
        {
            var addressValidationServiceMock = new Mock<IAddressValidationService>();

            var result = await AddressValidatorWebApiBuilder.GetUsageCountByState(input, addressValidationServiceMock.Object);

            Assert.IsType<BadRequest>(result);
        }

        [Fact]
        public async Task ValidateAddress_ShouldReturnOkWithValidateAddressResponse()
        {
            var fakeResponses = new List<ValidateAddressResponse>
            {
                new ValidateAddressResponse(),
                new ValidateAddressResponse()
            };
            var addressValidationServiceMock = new Mock<IAddressValidationService>();
            addressValidationServiceMock.Setup(svc => svc.ValidateAddressAsync(It.IsAny<ValidateAddressesRequest>())).ReturnsAsync(fakeResponses);

            var request = new ValidateAddressesRequest() { RawAddresses = new List<string> { It.IsAny<string>(), It.IsAny<string>() } };
            var result = await AddressValidatorWebApiBuilder.ValidateAddress(request, addressValidationServiceMock.Object);

            Assert.IsType<Ok<IEnumerable<ValidateAddressResponse>>>(result);
            var okResult = (Ok<IEnumerable<ValidateAddressResponse>>)result;
            Assert.NotNull(okResult.Value);
            Assert.Equal(fakeResponses.Count(), okResult.Value.Count());
        }

        [Fact]
        public async Task ValidateAddress_WithEmptyRequest_ShouldReturnBadRequest()
        {
            var addressValidationServiceMock = new Mock<IAddressValidationService>();

            var result = await AddressValidatorWebApiBuilder.ValidateAddress(null, addressValidationServiceMock.Object);

            Assert.IsType<BadRequest>(result);
        }

        [Fact]
        public async Task ValidateAddress_WithNullAddressList_ShouldReturnBadRequest()
        {
            var addressValidationServiceMock = new Mock<IAddressValidationService>();

            var request = new ValidateAddressesRequest() { RawAddresses = null };
            var result = await AddressValidatorWebApiBuilder.ValidateAddress(request, addressValidationServiceMock.Object);

            Assert.IsType<BadRequest>(result);
        }

        [Fact]
        public async Task ValidateAddress_WithEmptyAddressList_ShouldReturnBadRequest()
        {
            var addressValidationServiceMock = new Mock<IAddressValidationService>();

            var request = new ValidateAddressesRequest() { RawAddresses = new List<string>() };
            var result = await AddressValidatorWebApiBuilder.ValidateAddress(request, addressValidationServiceMock.Object);

            Assert.IsType<BadRequest>(result);
        }
    }
}
