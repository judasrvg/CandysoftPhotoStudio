using System.IO.Compression;
using System.Text.Json;
using Tattoo.InkVibesTattoo.Models.Requests;

namespace Tattoo.InkVibesTattoo.Services
{
    public abstract class BaseApiService
    {
        protected readonly IHttpClientFactory _http;
        protected readonly JsonSerializerOptions _options;

        public BaseApiService(IHttpClientFactory http)
        {
            _http = http;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        protected async Task<ResponseAdapterDto> HandleResponseAsync<T>(HttpResponseMessage response, bool mostReturnData=false)
        {
            var responseAdapterDto = new ResponseAdapterDto();
            try
            {
                string content = await ReadContent(response);
                if (response.IsSuccessStatusCode)
                {
                    responseAdapterDto.SetSuccess((int)response.StatusCode, string.IsNullOrWhiteSpace(content) ? null : JsonSerializer.Deserialize<T>(content, _options), "Request successful");
                }
                else
                {
                    responseAdapterDto.SetError((int)response.StatusCode, $"{response.ReasonPhrase}");
                }
            }
            catch (JsonException ex)
            {
                responseAdapterDto.SetError(500, $"JSON parsing error: {ex.Message}");
            }
            catch (Exception ex)
            {
                responseAdapterDto.SetError(500, $"An error occurred: {ex.Message}");
            }
            return responseAdapterDto;
        }

        protected async Task<string> ReadContent(HttpResponseMessage response)
        {
            if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            {
                using (var decompressedStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress))
                {
                    var reader = new StreamReader(decompressedStream);
                    return await reader.ReadToEndAsync();
                }
            }
            else
            {
                return await response.Content.ReadAsStringAsync();
            }
        }
    }

}
