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
using Microsoft.AspNetCore.Http;

namespace Tattoo.Management.Services
{
    public class ImagesService : BaseApiService, IImagesService
    {
        public ImagesService(IHttpClientFactory http) : base(http) { }

         public async Task<ResponseAdapterDto> UploadImagesAsync(MultipartFormDataContent content)
        {
            var client = _http.CreateClient("ImagesHttpClient");
            var response = await client.PostAsync("upload", content);
            return await HandleResponseAsync<List<string>>(response);
        }
    }

}
