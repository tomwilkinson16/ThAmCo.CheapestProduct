using System;

namespace ThAmCo.CheapestProducts.Services.CheapestProduct
{
    public class LowestPriceServiceFake : ILowestPriceService
    {
        private readonly LowestProductDto[] _products =
        {
            new LowestProductDto { Id = 1, Name = "Product 1", Description = "Description 1", Price = 1.99m, StockLevel = 10 }, 
            new LowestProductDto { Id = 2, Name = "Product 2", Description = "Description 2", Price = 2.99m, StockLevel = 20 },
            new LowestProductDto { Id = 3, Name = "Product 3", Description = "Description 3", Price = 3.99m, StockLevel = 30 },
            new LowestProductDto { Id = 4, Name = "Product 4", Description = "Description 4", Price = 4.99m, StockLevel = 40 },
            new LowestProductDto { Id = 5, Name = "Product 5", Description = "Description 5", Price = 5.99m, StockLevel = 50 }
        };

        public Task<IEnumerable<LowestProductDto>> GetLowestPriceAsync()
        {
            IEnumerable<LowestProductDto> products = _products;
            return Task.FromResult(products);
        }

        Task<LowestProductDto> ILowestPriceService.GetLowestPriceAsync(int id)
        {
            LowestProductDto product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);        
        }
    }
}