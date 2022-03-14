using Microsoft.Extensions.Logging;
using System.Net.Http;
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

        public Task<Address> GetAddressByZipCodeAsync(string zipCode)
        {
            throw new System.NotImplementedException();
        }
    }
}
