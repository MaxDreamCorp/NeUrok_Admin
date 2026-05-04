using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IClientStatusRepository
    {
        Task<ClientStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<ClientStatus>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
