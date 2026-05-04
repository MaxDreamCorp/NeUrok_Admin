using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IGroupStatusRepository
    {
        Task<GroupStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<GroupStatus>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
