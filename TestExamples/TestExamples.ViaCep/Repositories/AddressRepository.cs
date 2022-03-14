using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TestExamples.ViaCep.Domain.Entities;
using TestExamples.ViaCep.Domain.Repositories;

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

            var resource = $"ws/{zipCode}/json/";

            var response = await _httpClient.GetAsync(resource);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(responseContent);
                throw new HttpRequestException(responseContent);
            }

            if (IsError(responseContent))
                throw new ArgumentException($"There is no address for the zip code {zipCode}.");

#pragma warning disable CS8603 // Possible null reference return.
            return JsonSerializer.Deserialize<Address>(responseContent);
#pragma warning restore CS8603 // Possible null reference return.
        }

        private bool IsError(string responseContent)
        {
            var errorPattern = @"""erro"": true";
            return responseContent.Contains(errorPattern);
        }
    }
}
