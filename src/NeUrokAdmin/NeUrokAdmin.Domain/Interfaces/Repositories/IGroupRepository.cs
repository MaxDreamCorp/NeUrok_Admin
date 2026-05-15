using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IGroupRepository
    {
        Task AddAsync(Group group, CancellationToken cancellationToken = default);
        Task RemoveAsync(Group group, CancellationToken cancellationToken = default);
        Task UpdateAsync(Group group, CancellationToken cancellationToken = default);
        Task<int> GetNextIdAsync(CancellationToken cancellationToken = default);
        Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task SetStudentsAsync(int groupId, List<Student> students, CancellationToken cancellationToken = default);
        Task<List<GroupDate>> GetGroupDatesAsync(int groupId, CancellationToken cancellationToken = default);
        Task<List<Group>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
