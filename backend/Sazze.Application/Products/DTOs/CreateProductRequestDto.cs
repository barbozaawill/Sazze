namespace Sazze.Application.Products.DTOs;

public class CreateProductRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Guid CategoryId { get; set; }
    public List<ProductVariantDto> Variants { get; set; } = new();
}
