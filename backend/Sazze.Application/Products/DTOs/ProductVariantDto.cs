namespace Sazze.Application.Products.DTOs;

public class ProductVariantDto
{
    public Guid Id { get; set; }
    public string Size { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
}
