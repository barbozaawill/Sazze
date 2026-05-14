using Sazze.Application.Customers.DTOs;

namespace Sazze.Application.Customers;

public interface ICustomerService
{
    Task<CustomerResponseDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<CustomerResponseDto>> GetAllAsync();
    Task DeleteAsync(Guid id);
}
