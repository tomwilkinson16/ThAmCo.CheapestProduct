using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThAmCo.CheapestProducts.Services.CheapestProduct;

namespace ThAmCo.CheapestProduct.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DebugController : ControllerBase
    {
        private readonly ILowestPriceService _lowestPriceService;

        public DebugController(ILowestPriceService lowestPriceService)
        {
            _lowestPriceService = lowestPriceService;
        }

        [HttpGet("CheapestProducts")]
        [Authorize]
        public async Task<IActionResult> Products()
        {
            IEnumerable<LowestProductDto> products = null!;
            try
            {
                return Ok(await _lowestPriceService.GetLowestPriceAsync());
            }
            catch
            {
                products = Array.Empty<LowestProductDto>();
                return StatusCode(505, products);
            }
        }

        [HttpGet("CheapestProducts/{id}")]
        [Authorize]
        public async Task<IActionResult> Product(int id)
        {
            LowestProductDto product = null!;
            try
            {
                return Ok(await _lowestPriceService.GetLowestPriceAsync(id));
            }
            catch
            {
                product = new LowestProductDto();
                return StatusCode(505, product);
            }
        }
    }
}