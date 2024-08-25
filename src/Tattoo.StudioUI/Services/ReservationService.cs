
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
    public class ReservationService : BaseApiService, IReservationService
    {
        public ReservationService(IHttpClientFactory http) : base(http) { }

        public async Task<ResponseAdapterDto> GetReservationAsync(long id)
        {
            var client = _http.CreateClient("ReservationHttpClient");
            var response = await client.GetAsync($"{id}");
            return await HandleResponseAsync<ReservationDto>(response);
        }

        public async Task<int> GetTotalSolicitedReservation()
        {
            var client = _http.CreateClient("ReservationHttpClient");
            var response = await client.GetAsync($"GetTotalSolicitedReservation");
            var responseHandled = await HandleResponseAsync<int>(response);
            return responseHandled?.Data ?? 0;
        }

        public async Task<ResponseAdapterDto> GetAllReservationsAsync()
        {
            var client = _http.CreateClient("ReservationHttpClient");
            var response = await client.GetAsync("");
            return await HandleResponseAsync<List<ReservationDto>>(response);
        }

        public async Task<ResponseAdapterDto> AddReservationAsync(ReservationDto reservation)
        {
            var client = _http.CreateClient("ReservationHttpClient");
            var content = new StringContent(JsonSerializer.Serialize(reservation), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("AddReservation", content);
            return await HandleResponseAsync<ReservationDto>(response);
        }

        public async Task<ResponseAdapterDto> UpdateReservationAsync(ReservationDto reservation)
        {
            var client = _http.CreateClient("ReservationHttpClient");
            var content = new StringContent(JsonSerializer.Serialize(reservation), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("UpdateReservation", content);
            return await HandleResponseAsync<ReservationDto>(response);
        }

        public async Task<ResponseAdapterDto> DeleteReservationAsync(long id)
        {
            var client = _http.CreateClient("ReservationHttpClient");
            var response = await client.DeleteAsync($"{id}");
            return await HandleResponseAsync<long>(response);
        }
    }
}
