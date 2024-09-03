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

namespace App.Application.Services.Query
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IReadRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ProductQueryService(IReadRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductDto?> GetProductAsync(long id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;

            var productDto = _mapper.Map<ProductDto>(product);

            // Mapear el tipo específico
            MapSpecificType(product, productDto);

            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            // Mapear los tipos específicos
            foreach (var productDto in productDtos)
            {
                var product = products.First(p => p.Id == productDto.Id);
                MapSpecificType(product, productDto);
            }

            return productDtos;
        }

        private void MapSpecificType(Product product, ProductDto productDto)
        {
            switch (product.ProductType)
            {
                case ProductType.FixedAsset:
                    var fixedAsset = product as FixedAsset;
                    if (fixedAsset != null)
                    {
                        productDto.FixedAssetDto = new FixedAssetDto
                        {
                            PurchaseDate = fixedAsset.PurchaseDate,
                            WarrantyExpiryDate = fixedAsset.WarrantyExpiryDate,
                            DepreciationRate = fixedAsset.DepreciationRate,
                            //Quantity = fixedAsset.Quantity,

                        };
                    }
                    break;

                case ProductType.Merchandise:
                    var merchandise = product as Merchandise;
                    if (merchandise != null)
                    {
                        productDto.MerchandiseDto = new MerchandiseDto
                        {
                            //StockQuantity = merchandise.StockQuantity,
                            LastRestockedDate = merchandise.LastRestockedDate
                        };
                    }
                    break;

                //case ProductType.RawMaterial:
                //    var rawMaterial = product as RawMaterial;
                //    if (rawMaterial != null)
                //    {
                //        productDto.RawMaterialDto = new RawMaterialDto
                //        {
                //            //StockQuantity = rawMaterial.StockQuantity,
                //            ConsumptionRate = rawMaterial.ConsumptionRate,
                //            LastUsedDate = rawMaterial.LastUsedDate
                //        };
                //    }
                //    break;

                default:
                    throw new ArgumentException("Tipo de producto no soportado");
            }
        }
    }


    public class TransactionQueryService : ITransactionQueryService
    {
        private readonly IReadRepository<Transaction> _repository;
        private readonly IMapper _mapper;

        public TransactionQueryService(IReadRepository<Transaction> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TransactionDto?> GetTransactionAsync(long id)
        {
            var transaction = await _repository.GetByIdAsync(id);
            return transaction == null ? null : _mapper.Map<TransactionDto>(transaction);
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactionsAsync()
        {
            var transactions = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }
    }

}
