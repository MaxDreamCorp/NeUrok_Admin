using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IGroupStudentRepository
    {
        Task AddAsync(GroupStudent groupStudent, CancellationToken cancellationToken = default);
        Task RemoveAsync(GroupStudent groupStudent, CancellationToken cancellationToken = default);
        Task UpdateSubscription(int groupId, int studentId, int newSubscriptionId, CancellationToken cancellationToken = default);
    }
}
