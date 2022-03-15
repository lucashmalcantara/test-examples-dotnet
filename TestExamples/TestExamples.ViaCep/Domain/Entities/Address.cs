namespace TestExamples.ViaCep.Domain.Entities
{
    public class Address
    {
        public string ZipCode { get; }
        public string Street { get; }
        public string Complement { get; }
        public string Neighborhood { get; }
        public string City { get; }
        public string State { get; }

        public Address(string zipCode, string street, string complement, string neighborhood, string city, string state)
        {
            ZipCode = zipCode;
            Street = street;
            Complement = complement;
            Neighborhood = neighborhood;
            City = city;
            State = state;
        }
    }
}
