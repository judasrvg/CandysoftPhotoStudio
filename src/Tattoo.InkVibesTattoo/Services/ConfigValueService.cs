using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Tattoo.InkVibesTattoo.Models.Forms;
using Tattoo.InkVibesTattoo.Models.Forms.Enum;
using Tattoo.InkVibesTattoo.Models.Requests;

namespace Tattoo.InkVibesTattoo.Services
{
    public class ConfigValueService : BaseApiService, IConfigValueService
    {
        public ConfigValueService(IHttpClientFactory http) : base(http) { }

        public async Task<ResponseAdapterDto> GetBasicsConfigValuesAsync()
        {
            var client = _http.CreateClient("ConfigValueHttpClient");
            var response = await client.GetAsync($"GetBasicsConfigTypes");
            return await HandleResponseAsync<List<ConfigValueDto>>(response);
        }

        public async Task<ResponseAdapterDto> GetConfigValueAsync(CacheValueType configType)
        {
            var client = _http.CreateClient("ConfigValueHttpClient");
            var response = await client.GetAsync($"{configType}");
            return await HandleResponseAsync<List<ConfigValueDto>>(response);
        }

        public async Task<ResponseAdapterDto> AddConfigValueAsync(ConfigValueDto configValue)
        {
            var client = _http.CreateClient("ConfigValueHttpClient");
            var content = new StringContent(JsonSerializer.Serialize(configValue), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("AddConfigValue", content);
            return await HandleResponseAsync<ConfigValueDto>(response);
        }

        public async Task<ResponseAdapterDto> UpdateConfigValueAsync(ConfigValueDto configValue)
        {
            var client = _http.CreateClient("ConfigValueHttpClient");
            var content = new StringContent(JsonSerializer.Serialize(configValue), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("UpdateConfigValue", content);
            return await HandleResponseAsync<ConfigValueDto>(response);
        }

        public async Task<ResponseAdapterDto> DeleteConfigValueAsync(long id, CacheValueType configType)
        {
            var client = _http.CreateClient("ConfigValueHttpClient");
            var response = await client.DeleteAsync($"{id}/{configType}");
            return await HandleResponseAsync<long>(response);
        }
    }
}
