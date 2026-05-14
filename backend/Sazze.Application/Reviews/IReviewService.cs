using Sazze.Application.Reviews.DTOs;

namespace Sazze.Application.Reviews;

public interface IReviewService
{
    Task<IEnumerable<ReviewResponseDto>> GetByProductIdAsync(Guid productId);
    Task<ReviewResponseDto> CreateAsync(Guid userId, CreateReviewRequestDto request);
    Task ToggleVisibilityAsync(Guid id);
    Task DeleteAsync(Guid id);
}
