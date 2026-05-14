using Sazze.Application.Orders.DTOs;
using Sazze.Domain.Entities;
using Sazze.Domain.Enums;
using Sazze.Domain.Interfaces;

namespace Sazze.Application.Orders.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICouponRepository _couponRepository;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICouponRepository couponRepository)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _couponRepository = couponRepository;
    }

    public async Task<OrderResponseDto?> GetByIdAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null) return null;

        return MapToDto(order);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetByUserIdAsync(Guid userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);
        return orders.Select(MapToDto);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Select(MapToDto);
    }

    public async Task<OrderResponseDto> CreateAsync(Guid userId, CreateOrderRequestDto request)
    {
        decimal total = 0;

        var items = new List<OrderItem>();

        foreach (var itemDto in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(itemDto.ProductVariantId);
            if (product is null)
                throw new Exception("Produto não encontrado.");

            var variant = product.Variants?.FirstOrDefault(v => v.Id == itemDto.ProductVariantId);
            if (variant is null || !variant.IsAvailable)
                throw new Exception("Variante não disponível.");

            var unitPrice = product.Price;
            total += unitPrice * itemDto.Quantity;

            items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductVariantId = itemDto.ProductVariantId,
                Quantity = itemDto.Quantity,
                UnitPrice = unitPrice
            });
        }

        Coupon? coupon = null;
        if (!string.IsNullOrEmpty(request.CouponCode))
        {
            coupon = await _couponRepository.GetByCodeAsync(request.CouponCode);
            if (coupon is null || !coupon.IsActive)
                throw new Exception("Cupom inválido ou inativo.");

            total = coupon.DiscountType == DiscountType.Percentage
                ? total - (total * coupon.DiscountValue / 100)
                : total - coupon.DiscountValue;
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = OrderStatus.AwaitingPayment,
            TotalAmount = total,
            CouponId = coupon?.Id,
            CreatedAt = DateTime.UtcNow,
            Items = items
        };

        await _orderRepository.AddAsync(order);
        return MapToDto(order);
    }

    public async Task UpdateStatusAsync(Guid id, string status)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        if (order is null)
            throw new Exception("Pedido não encontrado.");

        if (!Enum.TryParse<OrderStatus>(status, true, out var newStatus))
            throw new Exception("Status inválido.");

        order.Status = newStatus;
        await _orderRepository.UpdateAsync(order);
    }

    private static OrderResponseDto MapToDto(Order order) => new()
    {
        Id = order.Id,
        Status = order.Status.ToString(),
        TotalAmount = order.TotalAmount,
        CreatedAt = order.CreatedAt,
        Items = order.Items?.Select(i => new OrderItemDto
        {
            ProductVariantId = i.ProductVariantId,
            ProductName = i.ProductVariant?.Product?.Name ?? string.Empty,
            Size = i.ProductVariant?.Size ?? string.Empty,
            Color = i.ProductVariant?.Color ?? string.Empty,
            Quantity = i.Quantity,
            UnitPrice = i.UnitPrice
        }).ToList() ?? new()
    };
}
