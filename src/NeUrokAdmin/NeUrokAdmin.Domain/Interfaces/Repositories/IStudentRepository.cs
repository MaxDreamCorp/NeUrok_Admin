using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IStudentRepository
    {
        Task AddAsync(Student student, CancellationToken cancellationToken = default);
        Task RemoveAsync(Student student, CancellationToken cancellationToken = default);
        Task UpdateAsync(Student student, CancellationToken cancellationToken = default);
        Task<int> GetNextIdAsync(CancellationToken cancellationToken = default);
        Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Student>> GetAllAsync(CancellationToken cancellationToken = default);
        //Task<List<Student>> SearchAsync(StudentSearchDTO request, CancellationToken cancellationToken = default);
    }
}
