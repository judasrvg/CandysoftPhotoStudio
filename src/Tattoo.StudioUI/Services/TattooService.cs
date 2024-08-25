using Tattoo.StudioUI.Models.Requests;
using System.Dynamic;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Web;
using Tattoo.StudioUI.Models.Configuration;
using System.Text.Json.Serialization;
using Tattoo.StudioUI.Models.Forms;

namespace Tattoo.StudioUI.Services
{
    public class TattooService : BaseApiService, ITattooService
    {
        public TattooService(IHttpClientFactory http) : base(http) { }

        public async Task<ResponseAdapterDto> GetTattooAsync(long id)
        {
            var client = _http.CreateClient("TattooHttpClient");
            var response = await client.GetAsync($"{id}");
            return await HandleResponseAsync<TattooDto>(response);
        }

        public async Task<ResponseAdapterDto> GetTattoosAsync(FormFilterGetTattoos filters)
        {
            var client = _http.CreateClient("TattooHttpClient");
            var response = await client.PostAsJsonAsync("search", filters);
            return await HandleResponseAsync<List<TattooDto>>(response);
        }

        public async Task<ResponseAdapterDto> AddTattooAsync(TattooDto tattoo)
        {
            var client = _http.CreateClient("TattooHttpClient");
            var content = new StringContent(JsonSerializer.Serialize(tattoo), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("AddTattoo", content);
            return await HandleResponseAsync<TattooDto>(response);
        }

        public async Task<ResponseAdapterDto> UpdateTattooAsync(TattooDto tattoo)
        {
            var client = _http.CreateClient("TattooHttpClient");
            var content = new StringContent(JsonSerializer.Serialize(tattoo), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("UpdateTattoo", content);
            return await HandleResponseAsync<TattooDto>(response);
        }

        public async Task<ResponseAdapterDto> DeleteTattooAsync(long id)
        {
            var client = _http.CreateClient("TattooHttpClient");
            var response = await client.DeleteAsync($"{id}");
            return await HandleResponseAsync<long>(response);
        }

        public async Task<ResponseAdapterDto> IncrementRatingAsync(long id)
        {
            var client = _http.CreateClient("TattooHttpClient");
            var response = await client.PatchAsync($"IncrementRating/{id}", null);
            return await HandleResponseAsync<string>(response);
        }
    }
}
