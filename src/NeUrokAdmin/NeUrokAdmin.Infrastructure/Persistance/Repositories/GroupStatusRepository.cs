using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class GroupStatusRepository : IGroupStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GroupStatus>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.GroupStatuses.ToListAsync(cancellationToken);
        }

        public async Task<GroupStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.GroupStatuses.FirstOrDefaultAsync(gc => gc.Id == id, cancellationToken);
        }
    }
}
