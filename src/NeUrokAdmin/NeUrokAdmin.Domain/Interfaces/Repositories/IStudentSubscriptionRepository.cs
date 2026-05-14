using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IStudentSubscriptionRepository
    {
        Task AddAsync(StudentSubscription studentSubscription, CancellationToken cancellationToken = default);
        Task RemoveAsync(StudentSubscription studentSubscription, CancellationToken cancellationToken = default);
        Task UpdateAsync(StudentSubscription studentSubscription, CancellationToken cancellationToken = default);
        Task<int> GetNextIdAsync(CancellationToken cancellationToken = default);
        Task<StudentSubscription?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<StudentSubscription>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default);
    }
}
