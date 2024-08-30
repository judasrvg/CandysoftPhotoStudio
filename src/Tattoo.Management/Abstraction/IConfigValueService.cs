
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
using Tattoo.Management.Models.Forms.Enum;

namespace Tattoo.Management.Services
{
    public interface IConfigValueService
    {
        Task<ResponseAdapterDto> GetBasicsConfigValuesAsync();
        Task<ResponseAdapterDto> GetOfferConfigValuesAsync();
        Task<ResponseAdapterDto> GetConfigValueAsync(CacheValueType configType);
        Task<ResponseAdapterDto> AddConfigValueAsync(ConfigValueDto configValue);
        Task<ResponseAdapterDto> UpdateConfigValueAsync(ConfigValueDto configValue);
        Task<ResponseAdapterDto> DeleteConfigValueAsync(long id, CacheValueType configType);
    }
}
