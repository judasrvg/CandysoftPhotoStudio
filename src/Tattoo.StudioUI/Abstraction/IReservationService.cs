﻿
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
    public interface IReservationService
    {
        Task<ResponseAdapterDto> GetReservationAsync(long id);
        Task<ResponseAdapterDto> AddReservationAsync(ReservationDto reservation);
        Task<ResponseAdapterDto> UpdateReservationAsync(ReservationDto reservation);
        Task<ResponseAdapterDto> DeleteReservationAsync(long id);
        Task<ResponseAdapterDto> GetAllReservationsAsync();
        Task<int> GetTotalSolicitedReservation();
    }

}
