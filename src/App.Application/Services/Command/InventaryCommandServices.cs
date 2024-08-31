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
            var existingProduct = await _readRepository.GetByIdAsync(productDto.Id);

            if (existingProduct != null)
            {
                _mapper.Map(productDto, existingProduct);

                // Actualizar el tipo específico
                UpdateSpecificType(existingProduct, productDto);

                _writeRepository.Update(existingProduct);
            }
            else
            {
                var newProduct = _mapper.Map<Product>(productDto);

                // Crear el tipo específico
                CreateSpecificType(newProduct, productDto);

                await _writeRepository.AddAsync(newProduct);
            }

            await _unitOfWork.CompleteAsync();
            return productDto.Id;
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

        private void CreateSpecificType(Product product, ProductDto productDto)
        {
            switch (productDto.ProductType)
            {
                case ProductType.FixedAsset:
                    var fixedAsset = _mapper.Map<FixedAsset>(productDto.FixedAssetDto);
                    fixedAsset.ProductId = product.Id;
                    product.FixedAsset = fixedAsset;
                    break;
                case ProductType.Merchandise:
                    var merchandise = _mapper.Map<Merchandise>(productDto.MerchandiseDto);
                    merchandise.ProductId = product.Id;
                    product.Merchandise = merchandise;
                    break;
                case ProductType.RawMaterial:
                    var rawMaterial = _mapper.Map<RawMaterial>(productDto.RawMaterialDto);
                    rawMaterial.ProductId = product.Id;
                    product.RawMaterial = rawMaterial;
                    break;
            }
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
