using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Group group, CancellationToken cancellationToken = default)
        {
            await _context.Groups.AddAsync(group, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Group>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Groups
                .Include(g => g.Course)
                .Include(g => g.Teacher)
                .Include(g => g.GroupStatus)
                .ToListAsync(cancellationToken);
        }

        public async Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Groups
                .Include(g => g.Course)
                .Include(g => g.Teacher)
                .Include(g => g.GroupStatus)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Groups.AnyAsync(cancellationToken) ?
                await _context.Groups.MaxAsync(g => g.Id, cancellationToken) : 1;
        }

        public async Task RemoveAsync(Group group, CancellationToken cancellationToken = default)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Group group, CancellationToken cancellationToken = default)
        {
            var existingGroup = await GetByIdAsync(group.Id, cancellationToken);
            if (existingGroup == null)
                throw new ArgumentNullException("Данной группы не существует");

            existingGroup.Name = group.Name;
            existingGroup.CourseId = group.CourseId;
            existingGroup.TeacherId = group.TeacherId;
            existingGroup.GroupStatusId = group.GroupStatusId;
            existingGroup.WeekDays = group.WeekDays;
            existingGroup.Time = group.Time;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
