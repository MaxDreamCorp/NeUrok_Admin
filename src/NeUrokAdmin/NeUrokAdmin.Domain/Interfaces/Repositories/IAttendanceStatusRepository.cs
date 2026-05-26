using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IAttendanceStatusRepository
    {
        Task<AttendanceStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<AttendanceStatus>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
