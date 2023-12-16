using AddressValidation.Domain.Models;

namespace AddressValidation.UnitTest.DomainTests.ModelTests
{
    public class AddressTest
    {
        [Fact]
        public void Equals_WithAllSameAddressValues_ShouldReturnTrue()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");

            //Equals
            Assert.True(address1.Equals(address2));

            //object.Equals
            Assert.True(address1.Equals((object)address2));

            //== operator
            Assert.True(address1 == address2);

            //!= operator
            Assert.False(address1 != address2);

            //hashcode
            Assert.True(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithSameAddressValuesAndNullSuburb_ShouldReturnTrue()
        {
            var address1 = new Address("Country1", null, "State1", "PostCode1", "Street1");
            var address2 = new Address("Country1", null, "State1", "PostCode1", "Street1");

            //Equals
            Assert.True(address1.Equals(address2));

            //object.Equals
            Assert.True(address1.Equals((object)address2));

            //== operator
            Assert.True(address1 == address2);

            //!= operator
            Assert.False(address1 != address2);

            //hashcode
            Assert.True(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithSameAddressValuesAndNullState_ShouldReturnTrue()
        {
            var address1 = new Address("Country1", "Suburb1", null, "PostCode1", "Street1");
            var address2 = new Address("Country1", "Suburb1", null, "PostCode1", "Street1");

            //Equals
            Assert.True(address1.Equals(address2));

            //object.Equals
            Assert.True(address1.Equals((object)address2));

            //== operator
            Assert.True(address1 == address2);

            //!= operator
            Assert.False(address1 != address2);

            //hashcode
            Assert.True(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithSameAddressValuesAndNullPostCode_ShouldReturnTrue()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", null, "Street1");
            var address2 = new Address("Country1", "Suburb1", "State1", null, "Street1");

            //Equals
            Assert.True(address1.Equals(address2));

            //object.Equals
            Assert.True(address1.Equals((object)address2));

            //== operator
            Assert.True(address1 == address2);

            //!= operator
            Assert.False(address1 != address2);

            //hashcode
            Assert.True(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithSameAddressValuesAndNullStreet_ShouldReturnTrue()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1");
            var address2 = new Address("Country1", "Suburb1", "State1", "PostCode1");

            //Equals
            Assert.True(address1.Equals(address2));

            //object.Equals
            Assert.True(address1.Equals((object)address2));

            //== operator
            Assert.True(address1 == address2);

            //!= operator
            Assert.False(address1 != address2);

            //hashcode
            Assert.True(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithAllDifferentAddressValues_ShouldReturnFalse()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country2", "Suburb2", "State2", "PostCode2", "Street2");

            //Equals
            Assert.False(address1.Equals(address2));

            //object.Equals
            Assert.False(address1.Equals((object)address2));

            //== operator
            Assert.False(address1 == address2);

            //!= operator
            Assert.True(address1 != address2);

            //hashcode
            Assert.False(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithDifferentAddressValuesAndSameCountry_ShouldReturnFalse()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country1", "Suburb2", "State2", "PostCode2", "Street2");

            //Equals
            Assert.False(address1.Equals(address2));

            //object.Equals
            Assert.False(address1.Equals((object)address2));

            //== operator
            Assert.False(address1 == address2);

            //!= operator
            Assert.True(address1 != address2);

            //hashcode
            Assert.False(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithDifferentAddressValuesAndSameSuburb_ShouldReturnFalse()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country2", "Suburb1", "State2", "PostCode2", "Street2");

            //Equals
            Assert.False(address1.Equals(address2));

            //object.Equals
            Assert.False(address1.Equals((object)address2));

            //== operator
            Assert.False(address1 == address2);

            //!= operator
            Assert.True(address1 != address2);

            //hashcode
            Assert.False(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithSameAddressValuesAndSameState_ShouldReturnTrue()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country2", "Suburb2", "State1", "PostCode2", "Street2");

            //Equals
            Assert.False(address1.Equals(address2));

            //object.Equals
            Assert.False(address1.Equals((object)address2));

            //== operator
            Assert.False(address1 == address2);

            //!= operator
            Assert.True(address1 != address2);

            //hashcode
            Assert.False(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithDifferentAddressValuesAndSamePostCode_ShouldReturnTrue()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country2", "Suburb2", "State2", "PostCode1", "Street2");

            //Equals
            Assert.False(address1.Equals(address2));

            //object.Equals
            Assert.False(address1.Equals((object)address2));

            //== operator
            Assert.False(address1 == address2);

            //!= operator
            Assert.True(address1 != address2);

            //hashcode
            Assert.False(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithDifferentAddressValuesAndSameStreet_ShouldReturnTrue()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country2", "Suburb2", "State2", "PostCode2", "Street1");

            //Equals
            Assert.False(address1.Equals(address2));

            //object.Equals
            Assert.False(address1.Equals((object)address2));

            //== operator
            Assert.False(address1 == address2);

            //!= operator
            Assert.True(address1 != address2);

            //hashcode
            Assert.False(address1.GetHashCode() == address2.GetHashCode());
        }

        [Fact]
        public void Equals_WithSameAddressValuesAndDifferentSuburb_ShouldReturnFalse()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country1", "Suburb2", "State1", "PostCode1", "Street1");
            var address3 = new Address("Country1", null, "State1", "PostCode1", "Street1");

            //Equals
            Assert.False(address1.Equals(address2));
            Assert.False(address1.Equals(address3));

            //object.Equals
            Assert.False(address1.Equals((object)address2));
            Assert.False(address1.Equals((object)address3));

            //== operator
            Assert.False(address1 == address2);
            Assert.False(address1 == address3);

            //!= operator
            Assert.True(address1 != address2);
            Assert.True(address1 != address3);

            //hashcode
            Assert.False(address1.GetHashCode() == address2.GetHashCode());
            Assert.False(address1.GetHashCode() == address3.GetHashCode());
        }

        [Fact]
        public void Equals_WithSameAddressValuesAndDifferentState_ShouldReturnFalse()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country1", "Suburb1", "State2", "PostCode1", "Street1");
            var address3 = new Address("Country1", "Suburb1", null, "PostCode1", "Street1");

            //Equals
            Assert.False(address1.Equals(address2));
            Assert.False(address1.Equals(address3));

            //object.Equals
            Assert.False(address1.Equals((object)address2));
            Assert.False(address1.Equals((object)address3));

            //== operator
            Assert.False(address1 == address2);
            Assert.False(address1 == address3);

            //!= operator
            Assert.True(address1 != address2);
            Assert.True(address1 != address3);

            //hashcode
            Assert.False(address1.GetHashCode() == address2.GetHashCode());
            Assert.False(address1.GetHashCode() == address3.GetHashCode());
        }

        [Fact]
        public void Equals_WithSameAddressValuesAndDifferentPostCode_ShouldReturnFalse()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country1", "Suburb1", "State1", "PostCode2", "Street1");
            var address3 = new Address("Country1", "Suburb1", "State1", null, "Street1");

            //Equals
            Assert.False(address1.Equals(address2));
            Assert.False(address1.Equals(address3));

            //object.Equals
            Assert.False(address1.Equals((object)address2));
            Assert.False(address1.Equals((object)address3));

            //== operator
            Assert.False(address1 == address2);
            Assert.False(address1 == address3);

            //!= operator
            Assert.True(address1 != address2);
            Assert.True(address1 != address3);

            //hashcode
            Assert.False(address1.GetHashCode() == address2.GetHashCode());
            Assert.False(address1.GetHashCode() == address3.GetHashCode());
        }

        [Fact]
        public void Equals_WithSameAddressValuesAndDifferentStreet_ShouldReturnFalse()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            var address2 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street2");
            var address3 = new Address("Country1", "Suburb1", "State1", "PostCode1", null);

            //Equals
            Assert.False(address1.Equals(address2));
            Assert.False(address1.Equals(address3));

            //object.Equals
            Assert.False(address1.Equals((object)address2));
            Assert.False(address1.Equals((object)address3));

            //== operator
            Assert.False(address1 == address2);
            Assert.False(address1 == address3);

            //!= operator
            Assert.True(address1 != address2);
            Assert.True(address1 != address3);

            //hashcode
            Assert.False(address1.GetHashCode() == address2.GetHashCode());
            Assert.False(address1.GetHashCode() == address3.GetHashCode());
        }

        [Fact]
        public void Equals_WithNullAddress_ShouldReturnFalse()
        {
            var address1 = new Address("Country1", "Suburb1", "State1", "PostCode1", "Street1");
            Address? address2 = null;

            //Equals
            Assert.False(address1.Equals(address2));

            //== operator
            Assert.False(address1 == address2);

            //!= operator
            Assert.True(address1 != address2);

        }
    }
}
