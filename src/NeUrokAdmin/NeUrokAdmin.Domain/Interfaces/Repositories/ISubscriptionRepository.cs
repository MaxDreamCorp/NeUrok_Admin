using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface ISubscriptionRepository
    {
        Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default);
        Task RemoveAsync(Subscription subscription, CancellationToken cancellationToken = default);
        Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default);
        Task<int> GetNextIdAsync(CancellationToken cancellationToken = default);
        Task<Subscription?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Subscription>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
