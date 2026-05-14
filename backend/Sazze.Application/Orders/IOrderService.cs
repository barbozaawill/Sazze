using Sazze.Application.Orders.DTOs;

namespace Sazze.Application.Orders;

public interface IOrderService
{
    Task<OrderResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<OrderResponseDto>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<OrderResponseDto>> GetAllAsync();
    Task<OrderResponseDto> CreateAsync(Guid userId, CreateOrderRequestDto request);
    Task UpdateStatusAsync(Guid id, string status);
}
