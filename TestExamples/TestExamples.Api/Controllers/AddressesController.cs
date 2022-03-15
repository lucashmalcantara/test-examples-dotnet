using Microsoft.AspNetCore.Mvc;
using TestExamples.ViaCep.Domain.Entities;

namespace TestExamples.Api.Controllers
{
    [ApiController]
    [Route("addresses")]    
    public class AddressesController : ControllerBase
    {
        [HttpGet("{zipCode}")]
        [ProducesResponseType(typeof(Address), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync(string zipCode)
        {
            return Ok();
        }
    }
}
