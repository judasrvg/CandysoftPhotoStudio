using Tattoo.Management.Models.Requests;

using Microsoft.AspNetCore.Http;

namespace Tattoo.Management.Services
{
    public interface IImagesService
    {
        Task<ResponseAdapterDto> UploadImagesAsync(MultipartFormDataContent files);

    }
}
