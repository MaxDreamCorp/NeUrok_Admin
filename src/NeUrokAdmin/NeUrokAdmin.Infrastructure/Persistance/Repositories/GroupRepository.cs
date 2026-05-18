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
                .Include(g => g.GroupDates)
                .Include(g => g.Students)
                .ToListAsync(cancellationToken);
        }

        public async Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Groups
                .Include(g => g.Course)
                .Include(g => g.Teacher)
                .Include(g => g.GroupStatus)
                .Include(g => g.GroupDates)
                .Include(g => g.Students)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task<List<GroupDate>> GetGroupDatesAsync(int groupId, CancellationToken cancellationToken = default)
        {
            return await _context.GroupDates.Where(gd => gd.GroupId == groupId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Groups.AnyAsync(cancellationToken) ?
                await _context.Groups.MaxAsync(g => g.Id, cancellationToken) + 1 : 1;
        }

        public async Task RemoveAsync(Group group, CancellationToken cancellationToken = default)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task SetStudentsAsync(int groupId, List<Student> students, CancellationToken cancellationToken = default)
        {
            var group = await _context.Groups
        .Include(g => g.Students)
        .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);

            if (group == null)
                throw new KeyNotFoundException($"Группа с ID {groupId} не найдена.");

            var toRemove = group.Students.Where(s => !students.Any(newS => newS.Id == s.Id)).ToList();
            foreach (var student in toRemove)
                group.Students.Remove(student);

            foreach (var student in students)
            {
                if (!group.Students.Any(s => s.Id == student.Id))
                {
                    _context.Students.Attach(student);
                    group.Students.Add(student);
                }
            }

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
