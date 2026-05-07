using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class ClassesTypeRepository : IClassesTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassesTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClassType>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.ClassTypes.ToListAsync(cancellationToken);
        }

        public async Task<ClassType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ClassTypes.FindAsync(id, cancellationToken);
        }
    }
}
