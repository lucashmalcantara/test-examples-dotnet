using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TestExamples.ViaCep.Domain.Entities;
using TestExamples.ViaCep.Domain.Repositories;
using TestExamples.ViaCep.Repositories.Models;

namespace TestExamples.ViaCep.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ILogger<AddressRepository> _logger;
        private readonly HttpClient _httpClient;

        public AddressRepository(ILogger<AddressRepository> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<Address> GetAddressByZipCodeAsync(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
                throw new ArgumentNullException(nameof(zipCode), "Zip code cannot be null or empty");

            var formattedZipCode = Regex.Replace(zipCode, "[^0-9]", "");

            var resource = $"ws/{formattedZipCode}/json/";

            var response = await _httpClient.GetAsync(resource);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(responseContent);
                throw new HttpRequestException(responseContent);
            }

            if (IsError(responseContent))
                throw new ArgumentException($"There is no address for the zip code {zipCode}.");

            var viaCepAddress =  JsonSerializer.Deserialize<ViaCepAddress>(responseContent);

            var domainAddress = ParseToAddress(viaCepAddress);

            return domainAddress;
        }

        private Address ParseToAddress(ViaCepAddress viaCepAddress) =>
            new Address(viaCepAddress.Cep,
                viaCepAddress.Logradouro,
                viaCepAddress.Complemento,
                viaCepAddress.Bairro,
                viaCepAddress.Localidade,
                viaCepAddress.Uf);

        private bool IsError(string responseContent)
        {
            var errorPattern = @"""erro"": true";
            return responseContent.Contains(errorPattern);
        }
    }
}
