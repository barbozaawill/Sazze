using Sazze.Application.Coupons.DTOs;

namespace Sazze.Application.Coupons;

public interface ICouponService
{
    Task<CouponResponseDto?> GetByIdAsync(Guid id);
    Task<CouponResponseDto?> GetByCodeAsync(string code);
    Task<IEnumerable<CouponResponseDto>> GetAllAsync();
    Task<CouponResponseDto> CreateAsync(CreateCouponRequestDto request);
    Task ToggleActiveAsync(Guid id);
    Task DeleteAsync(Guid id);
}
