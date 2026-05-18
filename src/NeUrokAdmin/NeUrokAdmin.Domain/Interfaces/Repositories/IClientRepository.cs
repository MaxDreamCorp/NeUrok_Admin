using NeUrokAdmin.Domain.DTOs.SearchDTOs;
using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IClientRepository
    {
        Task AddAsync(Client client, CancellationToken cancellationToken = default);
        Task RemoveAsync(Client client, CancellationToken cancellationToken = default);
        Task UpdateAsync(Client client, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(int id, int statusId, CancellationToken cancellationToken = default);
        Task<int> GetNextIdAsync(CancellationToken cancellationToken = default);
        Task<Client?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<Client>> SearchAsync(ClientSearchDTO request, CancellationToken cancellationToken = default);
    }
}
