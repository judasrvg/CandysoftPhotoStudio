using Tattoo.Management.Models.Forms;
using Tattoo.Management.Models.Requests;

namespace Tattoo.Management.Abstraction
{
    public interface ITransactionService
    {
        Task<ResponseAdapterDto> GetFilteredTransactionsAsync(TransactionFilterDto filter);
    }
}
