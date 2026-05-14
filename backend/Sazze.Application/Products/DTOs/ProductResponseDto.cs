namespace Sazze.Application.Products.DTOs;

public class ProductResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<string> ImageUrls { get; set; } = new();   
    public List<ProductVariantDto> Variants { get; set; } = new();
}
