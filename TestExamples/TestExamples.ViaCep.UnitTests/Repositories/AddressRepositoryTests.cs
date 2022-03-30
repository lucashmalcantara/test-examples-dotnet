using Autofac.Extras.FakeItEasy;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TestExamples.ViaCep.Domain.Entities;
using TestExamples.ViaCep.Repositories;
using Xunit;
using TestExamples.TestUtilities.FakeItEasy.Extensions;

namespace TestExamples.ViaCep.UnitTests.Repositories
{
    public class AddressRepositoryTests
    {
        [Theory]
        [InlineData("30130010", "30130010")]
        [InlineData("30130-010", "30130010")]
        public async Task Should_return_an_Address_when_zip_code_exists(string validZipCode, string formattedZipCode)
        {
            // Arrange
            var apiJsonResult =
            @"{
                ""cep"": ""30130-010"",
                ""logradouro"": ""Praça Sete de Setembro"",
                ""complemento"": """",
                ""bairro"": ""Centro"",
                ""localidade"": ""Belo Horizonte"",
                ""uf"": ""MG"",
                ""ibge"": ""3106200"",
                ""gia"": """",
                ""ddd"": ""31"",
                ""siafi"": ""4123""
            }";

            using var mockHttp = new MockHttpMessageHandler();
            _ = mockHttp.When(HttpMethod.Get, $"https://viacep.com.br/ws/{formattedZipCode}/json/")
                    .Respond(HttpStatusCode.OK, "application/json", apiJsonResult);

            using var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("https://viacep.com.br");

            using var autoFake = new AutoFake();

            autoFake.Provide(httpClient);

            var addressRepository = autoFake.Resolve<AddressRepository>();

            var expectedAddress = new Address(zipCode: "30130-010",
                street: "Praça Sete de Setembro",
                complement: "",
                neighborhood: "Centro",
                city: "Belo Horizonte",
                state: "MG");

            // Act
            var address = await addressRepository.GetAddressByZipCodeAsync(validZipCode);

            // Assert
            address.Should().BeEquivalentTo(expectedAddress);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Should_throw_an_ArgumentNullException_when_zip_code_is_null_or_empty(string zipCode)
        {
            // Arrange
            using var autoFake = new AutoFake();

            var addressRepository = autoFake.Resolve<AddressRepository>();

            // Act
            var func = async () => await addressRepository.GetAddressByZipCodeAsync(zipCode);

            // Assert
            await func.Should().ThrowExactlyAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task Should_throw_an_ArgumentException_when_zip_code_is_invalid()
        {
            // Arrange
            var zipCodeOfANonExistentPlace = "99999999";

            var apiViaCepErrorResult =
            @"{
                ""erro"": true
            }";

            using var mockHttp = new MockHttpMessageHandler();
            _ = mockHttp.When(HttpMethod.Get, $"https://viacep.com.br/ws/{zipCodeOfANonExistentPlace}/json/")
                    .Respond(HttpStatusCode.OK, "application/json", apiViaCepErrorResult);

            using var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("https://viacep.com.br");

            using var autoFake = new AutoFake();

            autoFake.Provide(httpClient);

            var addressRepository = autoFake.Resolve<AddressRepository>();

            // Act
            var func = async () => await addressRepository.GetAddressByZipCodeAsync(zipCodeOfANonExistentPlace);

            // Assert
            await func.Should().ThrowExactlyAsync<ArgumentException>();
        }

        [Theory]
        [InlineData("30130010", "30130010")]
        [InlineData("30130-010", "30130010")]
        public async Task Should_call_the_external_API_with_correct_parameters(string validZipCode, string formattedZipCode)
        {
            // Arrange
            var expectedEndpoint = $"https://viacep.com.br/ws/{formattedZipCode}/json/";

            var apiJsonResult =
            @"{
                ""cep"": ""30130-010"",
                ""logradouro"": ""Praça Sete de Setembro"",
                ""complemento"": """",
                ""bairro"": ""Centro"",
                ""localidade"": ""Belo Horizonte"",
                ""uf"": ""MG"",
                ""ibge"": ""3106200"",
                ""gia"": """",
                ""ddd"": ""31"",
                ""siafi"": ""4123""
            }";

            using var mockHttp = new MockHttpMessageHandler();
            _ = mockHttp.Expect(HttpMethod.Get, expectedEndpoint)
                    .Respond(HttpStatusCode.OK, "application/json", apiJsonResult);

            using var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("https://viacep.com.br");

            using var autoFake = new AutoFake();

            autoFake.Provide(httpClient);

            var addressRepository = autoFake.Resolve<AddressRepository>();

            // Act
            var address = await addressRepository.GetAddressByZipCodeAsync(validZipCode);

            // Assert
            mockHttp.VerifyNoOutstandingExpectation();
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.RequestTimeout)]
        public async Task Should_throw_an_HttpRequestException_when_the_external_API_returns_an_unpredicted_error(HttpStatusCode errorStatusCode)
        {
            // Arrange
            var someZipCode = "30130-010";
            var expectedEndpoint = $"https://viacep.com.br/ws/{someZipCode}/json/";

            using var mockHttp = new MockHttpMessageHandler();
            _ = mockHttp.When(HttpMethod.Get, expectedEndpoint)
                    .Respond(errorStatusCode, "application/json", "ERROR");

            using var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("https://viacep.com.br");

            using var autoFake = new AutoFake();

            autoFake.Provide(httpClient);
            var addressRepository = autoFake.Resolve<AddressRepository>();

            // Act
            var func = async () => await addressRepository.GetAddressByZipCodeAsync(someZipCode);

            // Assert
            await func.Should().ThrowExactlyAsync<HttpRequestException>();
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.RequestTimeout)]
        public async Task Should_log_a_message_when_the_external_API_returns_an_unpredicted_error(HttpStatusCode errorStatusCode)
        {
            // Arrange
            var someZipCode = "30130010";
            var expectedEndpoint = $"https://viacep.com.br/ws/{someZipCode}/json/";

            using var mockHttp = new MockHttpMessageHandler();
            _ = mockHttp.When(HttpMethod.Get, expectedEndpoint)
                    .Respond(errorStatusCode, "application/json", "ERROR");

            using var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("https://viacep.com.br");

            using var autoFake = new AutoFake();

            autoFake.Provide(httpClient);

            var logger = A.Fake<ILogger<AddressRepository>>();
            autoFake.Provide(logger); // TIP: In addition to the Resolve method, it is possible to create an object and provide it to AutoFake.
                                      // So the log object will be injected when the AddressRepository object is created by the Resolve method.

            var addressRepository = autoFake.Resolve<AddressRepository>();

            var expectedLogMessage = $"There is no address for the zip code {someZipCode}.";

            // Act
            try
            {
                _ = await addressRepository.GetAddressByZipCodeAsync(someZipCode);
            }
            catch
            { }

            // Assert
            logger.VerifyLogObject(LogLevel.Error, "ERROR")
                .MustHaveHappenedOnceExactly();
        }
    }
}
