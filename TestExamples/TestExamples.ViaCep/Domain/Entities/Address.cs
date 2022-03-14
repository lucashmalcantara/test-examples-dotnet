namespace TestExamples.ViaCep.Domain.Entities
{
    public class Address
    {
        public Address(string zipCode, string street, string complement, string neighborhood, string city, string state)
        {
            //ZipCode = zipCode;
            //Street = street;
            //Complement = complement;
            //Neighborhood = neighborhood;
            //City = city;
            //State = state;
        }

        public string ZipCode { get; set; }
        public string Street { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
