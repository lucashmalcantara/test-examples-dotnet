using FluentAssertions;
using TestExamples.ViaCep.Domain.Entities;
using Xunit;

namespace TestExamples.ViaCep.UnitTests.Domain.Entities
{
    public class AddressTests
    {
        [Fact]
        public void Should_set_properties_correctly()
        {
            // Arrange - Act
            var address = new Address(zipCode: "30130-010",
                street: "Praça Sete de Setembro",
                complement: "",
                neighborhood: "Centro",
                city: "Belo Horizonte",
                state: "MG");

            // Assert
            address.ZipCode.Should().Be("30130-010");
            address.Street.Should().Be("Praça Sete de Setembro");
            address.Complement.Should().Be("");
            address.Neighborhood.Should().Be("Centro");
            address.City.Should().Be("Belo Horizonte");
            address.State.Should().Be("MG");
        }
    }
}
