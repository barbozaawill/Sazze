using Sazze.Application.Products.DTOs;

namespace Sazze.Application.Products;

public interface IProductService
{
    Task<ProductResponseDto?> GetByIdAsync(Guid productId);
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    Task<IEnumerable<ProductResponseDto>> GetByCategoryAsync(Guid categoryId);
    Task<ProductResponseDto> CreateAsync(CreateProductRequestDto request);
    Task<ProductResponseDto> UpdateAsync(Guid id, UpdateProductRequestDto request);
    Task DeleteAsync(Guid id);

}
