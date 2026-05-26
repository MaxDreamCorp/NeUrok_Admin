using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IGroupDateRepository
    {
        Task AddAsync(GroupDate groupDate, CancellationToken cancellationToken = default);
        Task RemoveAsync(GroupDate groupDate, CancellationToken cancellationToken = default);
        Task UpdateAsync(GroupDate groupDate, CancellationToken cancellationToken = default);
        Task<int> GetNextIdAsync(CancellationToken cancellationToken = default);
        Task<GroupDate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
