using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThAmCo.CheapestProducts.Services.CheapestProduct;

namespace ThAmCo.CheapestProduct.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DebugController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ILowestPriceService _lowestPriceService;

        public DebugController(ILogger<DebugController> logger, 
                                    ILowestPriceService lowestPriceService)
        {
            _logger = logger;
            _lowestPriceService = lowestPriceService;
        }

        [HttpGet("CheapestProducts")]
        [Authorize]
        public async Task<IActionResult> Products()
        {
            IEnumerable<LowestProductDto> products = null!;
            try
            {
                // products = await _lowestPriceService.GetLowestPriceAsync(0);
                return Ok(await _lowestPriceService.GetLowestPriceAsync());

            }
            catch (Exception ex)
            {
                    _logger.LogWarning(ex, "Failed to get products");
                    products = Array.Empty<LowestProductDto>();
                    return StatusCode(505, products);
            }
        }
        //get single product
        [HttpGet("CheapestProducts/{id}")]
        [Authorize]
        public async Task<IActionResult> Product(int id)
        {
            LowestProductDto product = null!;
            try
            {
                // product = await _lowestPriceService.GetLowestPriceAsync(id);
                return Ok(await _lowestPriceService.GetLowestPriceAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get product");
                product = new LowestProductDto();
                return StatusCode(505, product);
            }
        }
    }
}
