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
                product = CreateSpecificType(productDto);
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
                    product = _mapper.Map<FixedAsset>(productDto.FixedAssetDto);
                    break;
                case ProductType.Merchandise:
                    product = _mapper.Map<Merchandise>(productDto.MerchandiseDto);
                    break;
                case ProductType.RawMaterial:
                    product = _mapper.Map<RawMaterial>(productDto.RawMaterialDto);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported product type.");
            }
            return product;
        }


        private void UpdateSpecificType(Product product, ProductDto productDto)
        {
            switch (productDto.ProductType)
            {
                case ProductType.FixedAsset:
                    _mapper.Map(productDto.FixedAssetDto, product.FixedAsset);
                    break;
                case ProductType.Merchandise:
                    _mapper.Map(productDto.MerchandiseDto, product.Merchandise);
                    break;
                case ProductType.RawMaterial:
                    _mapper.Map(productDto.RawMaterialDto, product.RawMaterial);
                    break;
            }
        }
    }


    public class TransactionCommandService : ITransactionCommandService
    {
        private readonly IReadRepository<Transaction> _readRepository;
        private readonly IWriteRepository<Transaction> _writeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionCommandService(IReadRepository<Transaction> readRepository, IWriteRepository<Transaction> writeRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
    }

}
