using Sazze.Application.Products.DTOs;
using Sazze.Domain.Entities;
using Sazze.Domain.Interfaces;

namespace Sazze.Application.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null) return null;

            return MapToDto(product);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(MapToDto);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return products.Select(MapToDto);
        }

        public async Task<ProductResponseDto> CreateAsync(CreateProductRequestDto request)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                CreatedAt = DateTime.UtcNow,
                Variants = request.Variants.Select(v => new ProductVariant
                {
                    Id = Guid.NewGuid(),
                    Size = v.Size,
                    Color = v.Color,
                    IsAvailable = true
                }).ToList()
            };

            await _productRepository.AddAsync(product);
            return MapToDto(product);
        }

        public async Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductRequestDto request)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                throw new Exception("Produto não encontrado.");

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;
            product.IsActive = request.IsActive;

            await _productRepository.UpdateAsync(product);
            return MapToDto(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
                throw new Exception("Produto não encontrado.");

            await _productRepository.DeleteAsync(id);
        }

        private static ProductResponseDto MapToDto(Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryName = product.Category?.Name ?? string.Empty,
            IsActive = product.IsActive,
            ImageUrls = product.Images?.Select(i => i.Url).ToList() ?? new(),
            Variants = product.Variants?.Select(v => new ProductVariantDto
            {
                Id = v.Id,
                Size = v.Size,
                Color = v.Color,
                IsAvailable = v.IsAvailable
            }).ToList() ?? new()
        };
    }
}
