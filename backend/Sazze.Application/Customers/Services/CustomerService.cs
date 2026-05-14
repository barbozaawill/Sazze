using Sazze.Application.Customers.DTOs;
using Sazze.Domain.Interfaces;

namespace Sazze.Application.Customers.Services;

public class CustomerService : ICustomerService
{
    private readonly IUserRepository _userRepository;

    public CustomerService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<CustomerResponseDto?> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null) return null;

        return MapToDto(user);
    }

    public async Task<IEnumerable<CustomerResponseDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
            throw new Exception("Cliente não encontrado.");

        await _userRepository.DeleteAsync(id);
    }

    private static CustomerResponseDto MapToDto(Sazze.Domain.Entities.User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Phone = user.Phone,
        CreatedAt = user.CreatedAt
    };
}
