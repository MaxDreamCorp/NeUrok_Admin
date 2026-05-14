using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface ISubscriptionStatusRepository
    {
        Task<SubscriptlonStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<SubscriptlonStatus>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
