
using System.Collections.Generic;
using System.Threading.Tasks;
using Tattoo.Management.Models.Forms;
using Tattoo.Management.Models.Requests;

namespace App.Client.Services
{
    public interface IProductService
    {
        Task<ResponseAdapterDto> GetProductAsync(long id);
        Task<ResponseAdapterDto> GetAllProductsAsync();
        Task<ResponseAdapterDto> AddProductAsync(ProductDto productDto);
        Task<ResponseAdapterDto> UpdateProductAsync(ProductDto productDto);
        Task<ResponseAdapterDto> DeleteProductAsync(long id);
        Task<ResponseAdapterDto> UpdateStockAsync(long id, int quantity);
    }
}
