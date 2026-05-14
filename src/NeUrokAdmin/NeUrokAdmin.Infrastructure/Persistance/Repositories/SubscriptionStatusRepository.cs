using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class SubscriptionStatusRepository : ISubscriptionStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SubscriptlonStatus>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SubscriptlonStatuses.ToListAsync(cancellationToken);
        }

        public async Task<SubscriptlonStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.SubscriptlonStatuses.FindAsync(id, cancellationToken);
        }
    }
}
