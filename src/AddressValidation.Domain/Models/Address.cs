namespace AddressValidation.Domain.Models
{
    public sealed class Address : IEquatable<Address>
    {
        public string? Street { get; private set; }
        public string? Suburb { get; private set; }
        public string? State { get; private set; }
        public string? PostCode { get; private set; }
        public string Country { get; private set; }

        private Address() { }

        public Address(string country, string? suburb = null, string? state = null, string? postcode = null, string? street = null)
        {
            Street = street;
            Suburb = suburb;
            State = state;
            PostCode = postcode;
            Country = country;
        }

        public bool Equals(Address other)
        {
            if (other == null)
                return false;

            if ((Street != null ? Street.ToUpper() : Street) != (other.Street != null ? other.Street.ToUpper() : other.Street))
                return false;

            if ((Suburb != null ? Suburb.ToUpper() : Suburb) != (other.Suburb != null ? other.Suburb.ToUpper() : other.Suburb))
                return false;

            if ((State != null ? State.ToUpper() : State) != (other.State != null ? other.State.ToUpper() : other.State))
                return false;

            if ((PostCode != null ? PostCode.ToUpper() : PostCode) != (other.PostCode != null ? other.PostCode.ToUpper() : other.PostCode))
                return false;

            if ((Country != null ? Country.ToUpper() : Country) != (other.Country != null ? other.Country.ToUpper() : other.Country))
                return false;

            return true;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            Address addressObj = obj as Address;
            if (addressObj == null)
                return false;
            else
                return Equals(addressObj);
        }

        public override int GetHashCode()
        {
            //ignore overflow check
            unchecked
            {
                //oversimplified hashcode
                var hashCode = string.IsNullOrWhiteSpace(Street) ? 0 : Street.GetHashCode();
                hashCode ^= string.IsNullOrWhiteSpace(Suburb) ? 0 : Suburb.GetHashCode();
                hashCode ^= string.IsNullOrWhiteSpace(State) ? 0 : State.GetHashCode();
                hashCode ^= string.IsNullOrWhiteSpace(PostCode) ? 0 : PostCode.GetHashCode();
                hashCode ^= string.IsNullOrWhiteSpace(Country) ? 0 : Country.GetHashCode();

                return hashCode;
            }
        }

        public static bool operator ==(Address address1, Address address2)
        {
            if (((object)address1) == null || ((object)address2) == null)
                return Object.Equals(address1, address2);

            return address1.Equals(address2);
        }

        public static bool operator !=(Address address1, Address address2)
        {
            if (((object)address1) == null || ((object)address2) == null)
                return !Object.Equals(address1, address2);

            return !(address1.Equals(address2));
        }

        public override string ToString()
        {
            return $"Street: {Street ?? "-"}; "
                + $"Suburb: {Suburb ?? "-"}; "
                + $"State: {State ?? "-"}; "
                + $"PostCode: {PostCode ?? "-"}; "
                + $"Country: {Country};";
        }
    }
}
