using Sazze.Application.Auth.DTOs;
using Sazze.Domain.Entities;
using Sazze.Domain.Interfaces;
using BCrypt.Net;

namespace Sazze.Application.Auth.Services
{
    public class AuthServices : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser is not null)
                throw new Exception("E-mail já cadastrado.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone = request.Phone,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            return new AuthResponseDto
            {
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new Exception("E-mail ou senha inválidos.");

            return new AuthResponseDto
            {
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }
    }
}
