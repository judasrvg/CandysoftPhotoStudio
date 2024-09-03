using App.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Abstractions.Services
{
    public interface IProductQueryService
    {
        Task<ProductDto?> GetProductAsync(long id);
        Task<IEnumerable<ProductDto>> GetProductsAsync();

    }

    public interface ITransactionQueryService
    {
        Task<TransactionDto?> GetTransactionAsync(long id);
        Task<IEnumerable<TransactionDto>> GetTransactionsAsync();
    }
    public interface IProductCommandService
    {
        Task<long> AddOrUpdateProductAsync(ProductDto product);
        Task DeleteProductAsync(long id);
        Task AdjustStockAsync(long productId, int quantity, string reason);

    }

    public interface ITransactionCommandService
    {
        Task<long> AddTransactionAsync(TransactionDto transaction);
        Task DeleteTransactionAsync(long id);
        Task DecrementStockAsync(long productId, int quantity, decimal purchaseCost);
        Task IncrementStockAsync(long productId, int quantity, decimal salePrice);
    }

   

}
