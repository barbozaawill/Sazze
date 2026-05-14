using Sazze.Application.Coupons.DTOs;
using Sazze.Domain.Entities;
using Sazze.Domain.Enums;
using Sazze.Domain.Interfaces;

namespace Sazze.Application.Coupons.Services;

public class CouponService : ICouponService
{
    private readonly ICouponRepository _couponRepository;

    public CouponService(ICouponRepository couponRepository)
    {
        _couponRepository = couponRepository;
    }

    public async Task<CouponResponseDto?> GetByIdAsync(Guid id)
    {
        var coupon = await _couponRepository.GetByIdAsync(id);
        if (coupon is null) return null;

        return MapToDto(coupon);
    }

    public async Task<CouponResponseDto?> GetByCodeAsync(string code)
    {
        var coupon = await _couponRepository.GetByCodeAsync(code);
        if (coupon is null) return null;

        return MapToDto(coupon);
    }

    public async Task<IEnumerable<CouponResponseDto>> GetAllAsync()
    {
        var coupons = await _couponRepository.GetAllAsync();
        return coupons.Select(MapToDto);
    }

    public async Task<CouponResponseDto> CreateAsync(CreateCouponRequestDto request)
    {
        if (!Enum.TryParse<DiscountType>(request.DiscountType, true, out var discountType))
            throw new Exception("Tipo de desconto inválido. Use 'Percentage' ou 'Fixed'.");

        var coupon = new Coupon
        {
            Id = Guid.NewGuid(),
            Code = request.Code.ToUpper().Trim(),
            DiscountType = discountType,
            DiscountValue = request.DiscountValue,
            IsActive = true
        };

        await _couponRepository.AddAsync(coupon);
        return MapToDto(coupon);
    }

    public async Task ToggleActiveAsync(Guid id)
    {
        var coupon = await _couponRepository.GetByIdAsync(id);
        if (coupon is null)
            throw new Exception("Cupom não encontrado.");

        coupon.IsActive = !coupon.IsActive;
        await _couponRepository.UpdateAsync(coupon);
    }

    public async Task DeleteAsync(Guid id)
    {
        var coupon = await _couponRepository.GetByIdAsync(id);
        if (coupon is null)
            throw new Exception("Cupom não encontrado.");

        await _couponRepository.DeleteAsync(id);
    }

    private static CouponResponseDto MapToDto(Coupon coupon) => new()
    {
        Id = coupon.Id,
        Code = coupon.Code,
        DiscountType = coupon.DiscountType.ToString(),
        DiscountValue = coupon.DiscountValue,
        IsActive = coupon.IsActive
    };
}
