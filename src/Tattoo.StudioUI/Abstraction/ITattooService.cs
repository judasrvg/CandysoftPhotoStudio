﻿using Tattoo.StudioUI.Models.Requests;
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
    public interface ITattooService
    {
        Task<ResponseAdapterDto> GetTattooAsync(long id);
        Task<ResponseAdapterDto> GetTattoosAsync(FormFilterGetTattoos filters);
        Task<ResponseAdapterDto> AddTattooAsync(TattooDto tattoo);
        Task<ResponseAdapterDto> UpdateTattooAsync(TattooDto tattoo);
        Task<ResponseAdapterDto> DeleteTattooAsync(long id);
        Task<ResponseAdapterDto> IncrementRatingAsync(long id);
    }
}
