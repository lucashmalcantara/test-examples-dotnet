using AutoBogus;
using TestExamples.ViaCep.Domain.Entities;

namespace TestExamples.ViaCep.Builders.Domain.Entities
{
    public class AddressBuilder : AutoFaker<Address>
    {
        public AddressBuilder()
        {
            CustomInstantiator(faker => 
            new Address(zipCode: faker.Address.ZipCode(),
                street: faker.Address.StreetName(),
                complement: "Some complement...",
                neighborhood: faker.Address.StreetName(),
                city: faker.Address.City(),
                state: faker.Address.State()));
        }
    }
}
