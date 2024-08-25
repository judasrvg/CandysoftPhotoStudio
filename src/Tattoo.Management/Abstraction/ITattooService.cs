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

namespace Tattoo.Management.Services
{
    public interface ITattooService
    {
        Task<ResponseAdapterDto> GetTattooAsync(long id);
        Task<ResponseAdapterDto> GetTattoosAsync(FormFilterGetTattoos filters);
        Task<ResponseAdapterDto> AddTattooAsync(TattooDto tattoo);
        Task<ResponseAdapterDto> UpdateTattooAsync(TattooDto tattoo);
        Task<ResponseAdapterDto> DeleteTattooAsync(long id);
        Task<ResponseAdapterDto> IncrementRatingAsync(long id);
        Task<ResponseAdapterDto> ReorderTattooAsync(TattooDto tattoo);
    }
}
