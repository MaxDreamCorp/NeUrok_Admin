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
            group.Id = await GetNextIdAsync(cancellationToken);

            var trackedEntity = _context.Groups.Local.FirstOrDefault(g => g.Id == group.Id);

            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            if (await _context.Groups.AnyAsync(g => g.Id == group.Id, cancellationToken))
                throw new Exception("Такой ID уже реально есть в самой БД!");

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
                .Include(g => g.GroupStudents)
                    .ThenInclude(gs => gs.Student)
                .ToListAsync(cancellationToken);
        }

        public async Task<Group?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Groups
                .Include(g => g.Course)
                .Include(g => g.Teacher)
                .Include(g => g.GroupStatus)
                .Include(g => g.GroupDates)
                .Include(g => g.GroupStudents)
                    .ThenInclude(gs => gs.Student)
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

        public async Task SetStudentsAsync(int groupId, Dictionary<Student, StudentSubscription> students, CancellationToken cancellationToken = default)
        {
            var group = await _context.Groups
        .Include(g => g.GroupStudents)
        .FirstOrDefaultAsync(g => g.Id == groupId, cancellationToken);

            if (group == null)
                throw new KeyNotFoundException($"Группа с ID {groupId} не найдена.");

            var toRemove = group.GroupStudents.Where(s => !students.Any(newS => newS.Key.Id == s.StudentId)).ToList();
            foreach (var student in toRemove)
            {
                group.GroupStudents.Remove(student);
                _context.GroupStudents.Remove(student);
            }

            foreach (var student in students)
            {
                if (!group.GroupStudents.Any(s => s.StudentId == student.Key.Id))
                {
                    GroupStudent groupStudent = GroupStudent.Create(
                        groupId,
                        student.Key.Id,
                        student.Value.Id);
                    group.GroupStudents.Add(groupStudent);
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
