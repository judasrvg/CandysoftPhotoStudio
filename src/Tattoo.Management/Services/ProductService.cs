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
namespace App.Client.Services
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

        public async Task<ResponseAdapterDto> UpdateStockAsync(long id, int quantity)
        {
            var client = _http.CreateClient("ProductHttpClient");
            var content = new StringContent(JsonSerializer.Serialize(quantity), Encoding.UTF8, "application/json");
            var response = await client.PatchAsync($"UpdateStock/{id}", content);
            return await HandleResponseAsync<int>(response);
        }
    }
}
