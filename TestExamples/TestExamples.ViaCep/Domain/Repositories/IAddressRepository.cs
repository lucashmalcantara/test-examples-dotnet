using System.Threading.Tasks;
using TestExamples.ViaCep.Domain.Entities;

namespace TestExamples.ViaCep.Domain.Repositories
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressByZipCodeAsync(string zipCode);
    }
}
