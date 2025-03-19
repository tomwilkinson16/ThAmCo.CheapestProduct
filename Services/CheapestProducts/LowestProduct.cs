using System;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using ThAmCo.CheapestProduct.Dtos;
using ThAmCo.CheapestProducts.Services.CheapestProduct;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging; // Add this using directive

namespace ThAmCo.CheapestProduct.Services.CheapestProducts
{
    public class LowestProducts : ILowestPriceService
    {
        private readonly HttpClient _httpclient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LowestProducts> _logger; // Add this field

        public LowestProducts(HttpClient httpclient, IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<LowestProducts> logger)
        {
            _httpclient = httpclient;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger; // Initialize the logger
        }

        public async Task<IEnumerable<LowestProductDto>> GetLowestPriceAsync(int? price)
        {
            if (price == null)
            {
                _logger.LogError("Price parameter is null."); // Log error
                throw new ArgumentNullException(nameof(price));
            }

            _logger.LogInformation("Starting GetLowestPriceAsync with price: {Price}", price);

            var totalClient = _httpClientFactory.CreateClient();
            totalClient.BaseAddress = new Uri(_configuration["TokenAuthority"]);
            var tokenParams = new Dictionary<string, string>
            {
                {"client_id", _configuration["ClientId"]},
                {"client_secret", _configuration["ClientSecret"]},
                {"grant_type", "client_credentials"},
                {"audience", _configuration["Audience"]}
            };

            _logger.LogInformation("Requesting token from {TokenAuthority}", _configuration["TokenAuthority"]);

            var tokenForm = new FormUrlEncodedContent(tokenParams);
            var tokenResponse = await totalClient.PostAsync("oauth/token", tokenForm);
            var contentString = await tokenResponse.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenDto>(contentString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _logger.LogInformation("Received token: {Token}", token.AccessToken);

            _httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            var requestUrl = "debug/repo";
            _logger.LogInformation("Requesting data from URL: {RequestUrl}", requestUrl);

            var response = await _httpclient.GetAsync(requestUrl);
            var productcontextcontentString = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<IEnumerable<LowestProductDto>>(productcontextcontentString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (products == null)
            {
                _logger.LogError("Failed to deserialize products."); // Log error
                throw new InvalidOperationException("Failed to deserialize products.");
            }

            _logger.LogInformation("Deserialized products successfully.");

            // Dictionary to store grouped results
            Dictionary<string, List<LowestProductDto>> lowestProduct = new Dictionary<string, List<LowestProductDto>>();

            // Iterate over the list and add to the dictionary
            foreach (var lowestProductDto in products)
            {
                if (!lowestProduct.ContainsKey(lowestProductDto.Name))
                {
                    lowestProduct[lowestProductDto.Name] = new List<LowestProductDto>();
                }
                lowestProduct[lowestProductDto.Name].Add(lowestProductDto);
            }
            List<LowestProductDto> lowestProductList = new List<LowestProductDto>();

            foreach (var key in lowestProduct.Keys)
            {
                lowestProductList.Add(lowestProduct[key].OrderBy(p => p.Price).First());
            }

            _logger.LogInformation("Returning {Count} lowest priced products.", lowestProductList.Count);

            return lowestProductList;
        }
    }
}