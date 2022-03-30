using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TestExamples.Api.IntegrationTests.TestUtilities.TestHost;
using TestExamples.ViaCep.Builders.Domain.Entities;
using TestExamples.ViaCep.Domain.Entities;
using TestExamples.ViaCep.Domain.Repositories;
using Xunit;

namespace TestExamples.Api.IntegrationTests.Controllers
{
    public class AddressesControllerTests
    {
        public class GetAddressByZipCode // This is a simple way to group tests in Test Explorer.
        {
            [Fact]
            public async Task Should_call_repository_when_trying_to_get_address_by_zip_code()
            {
                // Arrange
                var zipCode = "30130-010";

                var addressRepository = A.Fake<IAddressRepository>();

                var testServerContainer = new TestServerContainer(serviceCollection =>
                {
                    serviceCollection.AddScoped(factory => addressRepository);
                });

                using var httpClient = testServerContainer.HttpClient;

                var addressBuilder = new AddressBuilder();
                var someAddress = addressBuilder.Generate();

                A.CallTo(() => addressRepository.GetAddressByZipCodeAsync(zipCode))
                     .Returns(someAddress);

                // Act
                var response = await httpClient.GetAsync($"/addresses/{zipCode}");

                // Assert
                A.CallTo(() => addressRepository.GetAddressByZipCodeAsync(zipCode))
                    .MustHaveHappenedOnceExactly();
            }

            [Theory]
            [InlineData("30130010")]
            [InlineData("30130-010")]
            public async Task Should_return_status_code_200_when_trying_to_get_address_by_zip_code_with_correct_parameters(string zipCode)
            {
                // Arrange
                var addressRepository = A.Fake<IAddressRepository>();

                var testServerContainer = new TestServerContainer(serviceCollection =>
                {
                    serviceCollection.AddScoped(factory => addressRepository);
                });

                using var httpClient = testServerContainer.HttpClient;

                var expectedAddress = new Address(zipCode: "30130-010",
                    street: "Praça Sete de Setembro",
                    complement: "",
                    neighborhood: "Centro",
                    city: "Belo Horizonte",
                    state: "MG");

                A.CallTo(() => addressRepository.GetAddressByZipCodeAsync(zipCode))
                     .Returns(expectedAddress);

                // Act
                var response = await httpClient.GetAsync($"/addresses/{zipCode}");

                // Assert
                response.Should().Be200Ok().And.BeAs(expectedAddress);
            }

            [Fact]
            public async Task Should_return_status_code_400_when_trying_to_get_address_by_zip_code_and_a_handled_exception_is_thrown()
            {
                // Arrange
                var zipCode = "30130-010";

                var addressRepository = A.Fake<IAddressRepository>();

                var testServerContainer = new TestServerContainer(serviceCollection =>
                {
                    serviceCollection.AddScoped(factory => addressRepository);
                });

                using var httpClient = testServerContainer.HttpClient;


                A.CallTo(() => addressRepository.GetAddressByZipCodeAsync(zipCode))
                     .Throws(new ArgumentException("Some exception message"));

                // Act
                var response = await httpClient.GetAsync($"/addresses/{zipCode}");
                var responseContent = await response.Content.ReadAsStringAsync();

                // Assert
                response.Should().Be400BadRequest();
                responseContent.Should().Be("Some exception message");
            }

            [Fact]
            public async Task Should_return_status_code_500_when_trying_to_get_address_by_zip_code_and_a_generic_exception_is_thrown()
            {
                // Arrange
                var zipCode = "30130-010";

                var addressRepository = A.Fake<IAddressRepository>();

                var testServerContainer = new TestServerContainer(serviceCollection =>
                {
                    serviceCollection.AddScoped(factory => addressRepository);
                });

                using var httpClient = testServerContainer.HttpClient;


                A.CallTo(() => addressRepository.GetAddressByZipCodeAsync(zipCode))
                     .Throws(new Exception("Some generic exception message"));

                // Act
                var response = await httpClient.GetAsync($"/addresses/{zipCode}");
                var responseContent = await response.Content.ReadAsStringAsync();

                // Assert
                response.Should().Be500InternalServerError();
                responseContent.Should().Be("Some generic exception message");
            }
        }
    }
}
