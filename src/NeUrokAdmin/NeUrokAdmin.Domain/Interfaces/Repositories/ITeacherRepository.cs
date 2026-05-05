using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface ITeacherRepository
    {
        Task AddAsync(Teacher teacher, CancellationToken cancellationToken = default);
        Task RemoveAsync(Teacher teacher, CancellationToken cancellationToken = default);
        Task UpdateAsync(Teacher teacher, CancellationToken cancellationToken = default);
        Task<int> GetNextIdAsync(CancellationToken cancellationToken = default);
        Task<Teacher?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Teacher>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
