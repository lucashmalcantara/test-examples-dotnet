using Microsoft.AspNetCore.Mvc;
using TestExamples.ViaCep.Domain.Entities;
using TestExamples.ViaCep.Domain.Repositories;

namespace TestExamples.Api.Controllers
{
    [ApiController]
    [Route("addresses")]
    public class AddressesController : ControllerBase
    {
        [HttpGet("{zipCode}")]
        [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Address), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Address), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync(string zipCode, [FromServices] IAddressRepository addressRepository)
        {
            try
            {
                var address = await addressRepository.GetAddressByZipCodeAsync(zipCode);
                return Ok(address);
            }
            catch (Exception ex)
            {
                var isGenericException = ex.GetType() == typeof(Exception);

                if (isGenericException)
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

                return BadRequest(ex.Message);
            }
        }
    }
}
