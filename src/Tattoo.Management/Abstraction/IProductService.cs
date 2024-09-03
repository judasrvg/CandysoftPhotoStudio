
using System.Collections.Generic;
using System.Threading.Tasks;
using Tattoo.Management.Models.Forms;
using Tattoo.Management.Models.Requests;

namespace Tattoo.Management.Services
{
    public interface IProductService
    {
        Task<ResponseAdapterDto> GetProductAsync(long id);
        Task<ResponseAdapterDto> GetAllProductsAsync();
        Task<ResponseAdapterDto> AddProductAsync(ProductDto productDto);
        Task<ResponseAdapterDto> UpdateProductAsync(ProductDto productDto);
        Task<ResponseAdapterDto> DeleteProductAsync(long id);
        Task<ResponseAdapterDto> IncrementStockAsync(long productId, int quantity, decimal purchaseCost);
        Task<ResponseAdapterDto> DecrementStockAsync(long productId, int quantity, decimal salePrice);
        Task<ResponseAdapterDto> AdjustStockAsync(long productId, int quantityAdjustment);
    }
}
