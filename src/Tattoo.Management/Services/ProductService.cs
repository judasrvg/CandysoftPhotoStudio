using Tattoo.Management.Models.Requests;
using System.Dynamic;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Web;
using Tattoo.Management.Models.Configuration;
using System.Text.Json.Serialization;
using Tattoo.Management.Models.Forms;
using Tattoo.Management.Services;
namespace Tattoo.Management.Services
{
    public class ProductService : BaseApiService, IProductService
    {
        public ProductService(IHttpClientFactory http) : base(http) { }

        public async Task<ResponseAdapterDto> GetProductAsync(long id)
        {
            var client = _http.CreateClient("ProductHttpClient");
            var response = await client.GetAsync($"{id}");
            return await HandleResponseAsync<ProductDto>(response);
        }

        public async Task<ResponseAdapterDto> GetAllProductsAsync()
        {
            var client = _http.CreateClient("ProductHttpClient");
            var response = await client.GetAsync("");
            return await HandleResponseAsync<List<ProductDto>>(response);
        }

        public async Task<ResponseAdapterDto> AddProductAsync(ProductDto productDto)
        {
            var client = _http.CreateClient("ProductHttpClient");
            var content = new StringContent(JsonSerializer.Serialize(productDto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("AddProduct", content);
            return await HandleResponseAsync<ProductDto>(response);
        }

        public async Task<ResponseAdapterDto> UpdateProductAsync(ProductDto productDto)
        {
            var client = _http.CreateClient("ProductHttpClient");
            var content = new StringContent(JsonSerializer.Serialize(productDto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("UpdateProduct", content);
            return await HandleResponseAsync<ProductDto>(response);
        }

        public async Task<ResponseAdapterDto> DeleteProductAsync(long id)
        {
            var client = _http.CreateClient("ProductHttpClient");
            var response = await client.DeleteAsync($"{id}");
            return await HandleResponseAsync<long>(response);
        }

        public async Task<ResponseAdapterDto> AdjustStockAsync(long productId, int quantityAdjustment)
        {
            var client = _http.CreateClient("ProductHttpClient");

            var requestData = new StockRequest
            {
                Quantity = quantityAdjustment
            };

            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            var response = await client.PatchAsync($"AdjustStock/{productId}", content);

            return await HandleResponseAsync<string>(response); // Asumimos que el endpoint retorna un mensaje de éxito
        }


        public async Task<ResponseAdapterDto> IncrementStockAsync(long productId, int quantity, decimal purchaseCost)
        {
            var client = _http.CreateClient("ProductHttpClient");

            var requestData = new StockRequest
            {
                Quantity = quantity,
                Value = purchaseCost
            };

            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"IncrementStock/{productId}", content);

            return await HandleResponseAsync<string>(response);
        }


        public async Task<ResponseAdapterDto> DecrementStockAsync(long productId, int quantity, decimal salePrice)
        {
            var client = _http.CreateClient("ProductHttpClient");

            var requestData = new StockRequest
            {
                Quantity = quantity,
                Value = salePrice
            };

            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"DecrementStock/{productId}", content);

            return await HandleResponseAsync<string>(response);
        }



    }
}
