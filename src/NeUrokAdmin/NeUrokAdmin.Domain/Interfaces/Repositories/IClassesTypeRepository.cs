using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IClassesTypeRepository
    {
        Task<ClassType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<ClassType>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
