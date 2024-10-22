using App.Application.Abstractions.Services;
using App.Application.DTOs;
using App.Domain.Entities;
using App.Domain.Enum;
using App.Domain.Interfases;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace App.Application.Services.Command
{
    public class ProductCommandService : IProductCommandService
    {
        private readonly IReadRepository<Product> _readRepository;
        private readonly IWriteRepository<Product> _writeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductCommandService(IReadRepository<Product> readRepository, IWriteRepository<Product> writeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<long> AddOrUpdateProductAsync(ProductDto productDto)
        {
            Product product;

            if (productDto.Id > 0)
            {
                // Actualización
                product = await _readRepository.GetByIdAsync(productDto.Id);
                if (product == null) throw new KeyNotFoundException("Product not found.");

                // Mapea las propiedades comunes
                _mapper.Map(productDto, product);

                // Maneja el tipo específico
                product = UpdateSpecificType(productDto, product);
            }
            else
            {
                // Creación
                product = CreateSpecificType(productDto);
                await _writeRepository.AddAsync(product);
            }

            await _unitOfWork.CompleteAsync();
            return product.Id;
        }


        public async Task DeleteProductAsync(long id)
        {
            var product = await _readRepository.GetByIdAsync(id);
            if (product != null)
            {
                _writeRepository.Delete(product);
                await _unitOfWork.CompleteAsync();
            }
        }

        private Product CreateSpecificType(ProductDto productDto)
        {
            Product product;
            switch (productDto.ProductType)
            {
                case ProductType.FixedAsset:
                    product = new FixedAsset
                    {
                        // Mapea las propiedades específicas de FixedAsset
                        PurchaseDate = productDto.FixedAssetDto.PurchaseDate,
                        WarrantyExpiryDate = productDto.FixedAssetDto.WarrantyExpiryDate,
                        DepreciationRate = productDto.FixedAssetDto.DepreciationRate,

                    };
                    break;

                case ProductType.Merchandise:
                    product = new Merchandise
                    {
                        // Mapea las propiedades específicas de Merchandise
                        StockQuantity = productDto.StockQuantity,
                        LastRestockedDate = productDto.MerchandiseDto.LastRestockedDate,
                        TransactionGroup = TransactionGroup.VENTA
                    };
                    break;

                case ProductType.RawMaterial:
                    product = new RawMaterial
                    {
                        // Mapea las propiedades específicas de RawMaterial
                        StockQuantity = productDto.StockQuantity
                    };
                    break;

                default:
                    throw new ArgumentException("Tipo de producto no soportado");
            }

            // Mapea las propiedades comunes
            _mapper.Map(productDto, product);
            product.StockQuantity = productDto.StockQuantity;
            product.TotalQuantity = productDto.StockQuantity;

            return product;
        }

        private Product UpdateSpecificType(ProductDto productDto, Product product)
        {
            switch (productDto.ProductType)
            {
                case ProductType.FixedAsset:
                    var fixedAsset = product as FixedAsset;
                    if (fixedAsset != null)
                    {
                        fixedAsset.PurchaseDate = productDto.FixedAssetDto.PurchaseDate;
                        fixedAsset.WarrantyExpiryDate = productDto.FixedAssetDto.WarrantyExpiryDate;
                        fixedAsset.DepreciationRate = productDto.FixedAssetDto.DepreciationRate;
                    }
                    break;

                case ProductType.Merchandise:
                    var merchandise = product as Merchandise;
                    if (merchandise != null)
                    {
                        merchandise.StockQuantity = productDto.StockQuantity;
                        merchandise.LastRestockedDate = productDto.MerchandiseDto.LastRestockedDate;
                    }
                    break;

                case ProductType.RawMaterial:
                    var rawMaterial = product as RawMaterial;
                    if (rawMaterial != null)
                    {
                        rawMaterial.StockQuantity = productDto.StockQuantity;
                    }
                    break;

                default:
                    throw new ArgumentException("Tipo de producto no soportado");
            }

            // Mapea las propiedades comunes
            _mapper.Map(productDto, product);

            return product;
        }

        public async Task AdjustStockAsync(long productId, int quantity, string reason)
        {
            var product = await _readRepository.GetByIdAsync(productId);
            if (product == null) throw new KeyNotFoundException("Product not found.");

            // Realiza el ajuste de stock
            product.StockQuantity += quantity;

            // No actualiza TotalQuantity ya que es un ajuste, no una compra/venta
            product.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.CompleteAsync();
        }



    }

    public class TransactionCommandService : ITransactionCommandService
    {
        private readonly IReadRepository<Transaction> _readRepository;
        private readonly IWriteRepository<Transaction> _writeRepository;
        private readonly IProductCommandService _productCommandService;
        private readonly IProductQueryService _productQueryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionCommandService(IReadRepository<Transaction> readRepository, IWriteRepository<Transaction> writeRepository, IUnitOfWork unitOfWork, IMapper mapper, IProductCommandService productCommandService, IProductQueryService productQueryService)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productCommandService = productCommandService;
            _productQueryService = productQueryService;
        }

        public async Task<long> AddTransactionAsync(TransactionDto transactionDto)
        {
            var transaction = _mapper.Map<Transaction>(transactionDto);
            await _writeRepository.AddAsync(transaction);
            await _unitOfWork.CompleteAsync();
            return transaction.Id;
        }

        public async Task DeleteTransactionAsync(long id)
        {
            var transaction = await _readRepository.GetByIdAsync(id);
            if (transaction != null)
            {
                _writeRepository.Delete(transaction);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task IncrementStockAsync(long productId, int quantity, decimal purchaseCost)
        {
            var product = await _productQueryService.GetProductAsync(productId);
            if (product == null) throw new KeyNotFoundException("Product not found.");

            // Incrementar el stock y actualizar la cantidad total
            product.StockQuantity += quantity;
            product.TotalQuantity += quantity;

            // Registrar la transacción de gasto
            var transaction = new Transaction
            {
                ProductId = productId,
                TransactionDate = DateTime.UtcNow,
                Quantity = quantity,
                TransactionType = TransactionType.Expense,
                TransactionGroup = product.TransactionGroup,
                TotalAmount =-( purchaseCost * quantity),
                Description = $"GASTO/AJUSTE. Cantidad({quantity}) Producto:{product.Name} (Total:${-(purchaseCost * quantity)})"
            };

            await _writeRepository.AddAsync(transaction);
            await _productCommandService.AddOrUpdateProductAsync(product);
            await _unitOfWork.CompleteAsync();
        }


        public async Task DecrementStockAsync(long productId, int quantity, decimal salePrice, int salePriceCard)
        {
            var product = await _productQueryService.GetProductAsync(productId);
            if (product == null) throw new KeyNotFoundException("Product not found.");

            // Verifica si hay suficiente stock
            if (product.StockQuantity < quantity) throw new InvalidOperationException("Not enough stock to decrement.");

            // Decrementar el stock
            product.StockQuantity -= quantity;

            // Registrar la transacción de ingreso
            var transaction = new Transaction
            {
                ProductId = productId,
                TransactionDate = DateTime.UtcNow,
                Quantity = quantity,
                TransactionType = TransactionType.Income,
                TransactionGroup = product.TransactionGroup,
                TotalAmount = (salePrice) * quantity,
                Description = $"Ingreso-{(product.TransactionGroup == TransactionGroup.VENTA ? new StringBuilder("Venta") : new StringBuilder("Servicio"))}. ({quantity}) {(product.TransactionGroup == TransactionGroup.VENTA ? new StringBuilder("Producto") : new StringBuilder("Servicio"))}:{product.Name} (Efectivo:{((salePrice) * quantity)- salePriceCard} Transferido:{salePriceCard} Total:${(salePrice) * quantity})"

            };

            await _writeRepository.AddAsync(transaction);
            await _productCommandService.AddOrUpdateProductAsync(product);
            await _unitOfWork.CompleteAsync();
        }

    }
}
