using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using App.Application.Abstractions;
using App.Application.Services.Command;
using App.Application.Services.Query;
using App.Application.DTOs;
using App.Application.Abstractions.Services;
using App.Domain.Enum;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductCommandService _productCommandService;
        private readonly IProductQueryService _productQueryService;

        public ProductController(IProductCommandService productCommandService, IProductQueryService productQueryService)
        {
            _productCommandService = productCommandService;
            _productQueryService = productQueryService;
        }

        // GET: api/Product/{id}
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProduct(long id)
        {
            var product = await _productQueryService.GetProductAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productQueryService.GetProductsAsync();
            return Ok(products);
        }

        // POST: api/Product/Add
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            var idResult = await _productCommandService.AddOrUpdateProductAsync(productDto);
            productDto.Id = idResult;
            return CreatedAtAction(nameof(GetProduct), new { id = idResult }, productDto);
        }

        // PUT: api/Product/Update
        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            await _productCommandService.AddOrUpdateProductAsync(productDto);
            return CreatedAtAction(nameof(GetProduct), new { id = productDto.Id }, productDto);
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            await _productCommandService.DeleteProductAsync(id);
            return Ok(id);
        }

        // Opcional: Endpoint para manejar acciones específicas de subtipos
        // Ejemplo: Actualizar la cantidad de stock de un producto de tipo Merchandise
        [HttpPatch("UpdateStock/{id:long}")]
        public async Task<IActionResult> UpdateStock(long id, [FromBody] int quantity)
        {
            var product = await _productQueryService.GetProductAsync(id);
            if (product == null || product.ProductType != ProductType.Merchandise)
                return BadRequest("Product not found or not a Merchandise type");

            var merchandiseDto = product.MerchandiseDto;
            merchandiseDto.StockQuantity = quantity;
            await _productCommandService.AddOrUpdateProductAsync(product);

            return Ok(merchandiseDto);
        }
    }
}
