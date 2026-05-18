using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class AttendanceTypeRepository : IAttendanceTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AttendanceType>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.AttendanceTypes.ToListAsync(cancellationToken);
        }

        public async Task<AttendanceType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.AttendanceTypes.FindAsync(id, cancellationToken);
        }
    }
}
