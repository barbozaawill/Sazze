namespace Sazze.Application.Coupons.DTOs;

public class CreateCouponRequestDto
{
    public string Code { get; set; } = string.Empty;
    public string DiscountType { get; set; } = string.Empty;
    public decimal DiscountValue { get; set; }
}
