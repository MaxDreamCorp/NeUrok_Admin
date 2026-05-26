using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class AttendanceStatusRepository : IAttendanceStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AttendanceStatus>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.AttendanceStatuses.ToListAsync(cancellationToken);
        }

        public async Task<AttendanceStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.AttendanceStatuses.FindAsync(id, cancellationToken);
        }
    }
}
