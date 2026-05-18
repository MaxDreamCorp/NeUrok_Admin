using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class GroupDateRepository : IGroupDateRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupDateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(GroupDate groupDate, CancellationToken cancellationToken = default)
        {
            await _context.GroupDates.AddAsync(groupDate, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<GroupDate?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.GroupDates.FindAsync(id, cancellationToken);
        }

        public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
        {
            return await _context.GroupDates.AnyAsync(cancellationToken) ?
                await _context.GroupDates.MaxAsync(g => g.Id, cancellationToken) + 1: 1;
        }

        public async Task RemoveAsync(GroupDate groupDate, CancellationToken cancellationToken = default)
        {
            _context.GroupDates.Remove(groupDate);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(GroupDate groupDate, CancellationToken cancellationToken = default)
        {
            var existingGroupDate = await GetByIdAsync(groupDate.Id, cancellationToken);
            if (existingGroupDate == null)
                throw new ArgumentNullException("Данной группы не существует");

            existingGroupDate.Datetime = groupDate.Datetime;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
