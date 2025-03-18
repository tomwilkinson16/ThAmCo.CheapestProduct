using System;

namespace ThAmCo.CheapestProducts.Services.CheapestProduct
{
    public interface ILowestPriceService
    {
        Task<IEnumerable<LowestProductDto>> GetLowestPriceAsync(int? price);
    }
}