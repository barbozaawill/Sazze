namespace Sazze.Application.Orders.DTOs;

public class CreateOrderRequestDto
{
    public List<CreateOrderItemDto> Items { get; set; } = new();
    public string? CouponCode { get; set; }
}
