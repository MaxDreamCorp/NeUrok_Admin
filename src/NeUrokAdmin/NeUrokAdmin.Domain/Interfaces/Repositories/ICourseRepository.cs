using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface ICourseRepository
    {
        Task AddAsync(Course course, CancellationToken cancellationToken = default);
        Task RemoveAsync(Course course, CancellationToken cancellationToken = default);
        Task UpdateAsync(Course course, CancellationToken cancellationToken = default);
        Task<int> GetNextIdAsync(CancellationToken cancellationToken = default);
        Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Course>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
