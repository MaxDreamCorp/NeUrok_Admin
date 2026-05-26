using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IAttendanceRepository
    {
        Task AddAsync(Attendance attendance, CancellationToken cancellationToken = default);
        Task RemoveAsync(Attendance attendance, CancellationToken cancellationToken = default);
        Task RemoveByGroupDateAsync(int groupId, DateTime dateTime, CancellationToken cancellationToken = default);
        Task RemoveFutureAttendanceForStudentsAsync(int groupId, List<int> clientIds, CancellationToken cancellationToken = default);
        Task UpdateAsync(Attendance attendance, CancellationToken cancellationToken = default);
        Task<int> GetNextIdAsync(CancellationToken cancellationToken = default);
        Task<Attendance?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Attendance>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);
        Task<List<Attendance>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
        Task<List<Attendance>> GetByGroupAndClientIdAsync(int groupId, int clientId, CancellationToken cancellationToken = default);
        Task<List<DateTime>> GetDatesWithCompletedAttendanceAsync(int groupId, List<DateTime> datesToCheck, CancellationToken cancellationToken);
    }

}
