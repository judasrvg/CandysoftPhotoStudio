using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Management.Models.Requests;
using Tattoo.Management.Models.Forms;
using Tattoo.Management.Abstraction;
using static System.Net.WebRequestMethods;
using Tattoo.Management.Services;

public class TransactionService :  BaseApiService, ITransactionService
{
    public TransactionService(IHttpClientFactory http) : base(http) { }

    public async Task<ResponseAdapterDto> GetFilteredTransactionsAsync(TransactionFilterDto filter)
    {
        var client = _http.CreateClient("TransactionHttpClient");

        // Serializar el DTO a JSON
        var content = new StringContent(JsonSerializer.Serialize(filter), Encoding.UTF8, "application/json");

        // Hacer la solicitud POST al endpoint
        var response = await client.PostAsync("GetFilteredTransactions", content);

        if (response.IsSuccessStatusCode)
        {
            // Manejar la respuesta exitosa, deserializando a una lista de transacciones
            var transactions = await response.Content.ReadFromJsonAsync<IEnumerable<TransactionDto>>();
            return new ResponseAdapterDto
            {
                IsSuccess = true,
                Data = transactions
            };
        }
        else
        {
            // Manejar el error en la solicitud
            var error = await response.Content.ReadAsStringAsync();
            return new ResponseAdapterDto
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }
    }

    public async Task<ResponseAdapterDto> DeleteProductAsync(long id)
    {
        var client = _http.CreateClient("ProductHttpClient");
        var response = await client.DeleteAsync($"{id}");
        return await HandleResponseAsync<long>(response);
    }
}
