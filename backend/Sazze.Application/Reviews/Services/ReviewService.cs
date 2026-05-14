using Sazze.Application.Reviews.DTOs;
using Sazze.Domain.Entities;
using Sazze.Domain.Interfaces;

namespace Sazze.Application.Reviews.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<IEnumerable<ReviewResponseDto>> GetByProductIdAsync(Guid productId)
    {
        var reviews = await _reviewRepository.GetByProductIdAsync(productId);
        return reviews.Select(MapToDto);
    }

    public async Task<ReviewResponseDto> CreateAsync(Guid userId, CreateReviewRequestDto request)
    {
        var existing = await _reviewRepository.GetByUserAndProductAsync(userId, request.ProductId);
        if (existing is not null)
            throw new Exception("Você já avaliou este produto.");

        if (request.Rating < 1 || request.Rating > 5)
            throw new Exception("A nota deve ser entre 1 e 5.");

        var review = new Review
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ProductId = request.ProductId,
            Rating = request.Rating,
            Comment = request.Comment,
            IsVisible = true,
            CreatedAt = DateTime.UtcNow
        };

        await _reviewRepository.AddAsync(review);
        return MapToDto(review);
    }

    public async Task ToggleVisibilityAsync(Guid id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review is null)
            throw new Exception("Avaliação não encontrada.");

        review.IsVisible = !review.IsVisible;
        await _reviewRepository.UpdateAsync(review);
    }

    public async Task DeleteAsync(Guid id)
    {
        var review = await _reviewRepository.GetByIdAsync(id);
        if (review is null)
            throw new Exception("Avaliação não encontrada.");

        await _reviewRepository.DeleteAsync(id);
    }

    private static ReviewResponseDto MapToDto(Review review) => new()
    {
        Id = review.Id,
        UserName = review.User?.Name ?? string.Empty,
        Rating = review.Rating,
        Comment = review.Comment,
        CreatedAt = review.CreatedAt
    };
}
