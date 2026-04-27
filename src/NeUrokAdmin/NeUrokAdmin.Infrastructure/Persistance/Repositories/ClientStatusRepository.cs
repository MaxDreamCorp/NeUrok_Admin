using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class ClientStatusRepository : IClientStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientStatus>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.ClientStatuses.ToListAsync(cancellationToken);
        }

        public async Task<ClientStatus?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ClientStatuses.FindAsync(id, cancellationToken);
        }

        public async Task<ClientStatus?> GetByNameAsync(string status, CancellationToken cancellationToken = default)
        {
            return await _context.ClientStatuses.FirstOrDefaultAsync(cs => cs.Status == status, cancellationToken);
        }
    }
}
