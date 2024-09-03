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
        private readonly ITransactionCommandService _transactionCommandService;

        public ProductController(IProductCommandService productCommandService, IProductQueryService productQueryService, ITransactionCommandService transactionCommandService)
        {
            _productCommandService = productCommandService;
            _productQueryService = productQueryService;
            _transactionCommandService = transactionCommandService;
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

        [HttpPatch("AdjustStock/{id:long}")]
        public async Task<IActionResult> AdjustStock(long id, [FromBody] StockRequest quantityAdjustment)
        {
            var product = await _productQueryService.GetProductAsync(id);
            if (product == null)
                return BadRequest("Product not found");

            // Ajuste del stock
            product.StockQuantity += quantityAdjustment.Quantity;
            product.TotalQuantity += quantityAdjustment.Quantity;

            await _productCommandService.AddOrUpdateProductAsync(product);

            return Ok();
        }

        [HttpPost("IncrementStock/{id:long}")]
        public async Task<IActionResult> IncrementStock(long id, [FromBody] StockRequest request)
        {
            var product = await _productQueryService.GetProductAsync(id);
            if (product == null)
            {
                return BadRequest("Product not found or not a Merchandise type");
            }

            await _transactionCommandService.IncrementStockAsync(id, request.Quantity, request.Value);

            return Ok();
        }


        [HttpPost("DecrementStock")]
        public async Task<IActionResult> DecrementStock( [FromBody] StockDecrement request)
        {
            foreach (var req in request.Requests)
            {
                var product = await _productQueryService.GetProductAsync(req.Id);
                if (product == null)
                {
                    return BadRequest("Product not found or not a Merchandise type");
                }

                await _transactionCommandService.DecrementStockAsync(req.Id, req.Quantity, req.Value);
            }
            

            return Ok();
        }

    }
}
