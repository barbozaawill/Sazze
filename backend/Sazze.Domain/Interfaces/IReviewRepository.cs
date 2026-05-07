using Sazze.Domain.Entities;

namespace Sazze.Domain.Interfaces;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(Guid id);
    Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId);
    Task<Review?> GetByUserAndProductAsync(Guid userId, Guid productId);
    Task AddAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Guid id);
}
