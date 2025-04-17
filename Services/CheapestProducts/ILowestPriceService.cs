using System;

namespace ThAmCo.CheapestProducts.Services.CheapestProduct
{
    public interface ILowestPriceService
    {
        public Task<IEnumerable<LowestProductDto>> GetLowestPriceAsync();
        public Task<LowestProductDto> GetLowestPriceAsync(int id);
    }
}