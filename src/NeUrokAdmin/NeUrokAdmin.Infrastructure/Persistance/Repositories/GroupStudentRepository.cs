using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class GroupStudentRepository : IGroupStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public GroupStudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(GroupStudent groupStudent, CancellationToken cancellationToken = default)
        {
            await _context.GroupStudents.AddAsync(groupStudent, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveAsync(GroupStudent groupStudent, CancellationToken cancellationToken = default)
        {
            _context.GroupStudents.Remove(groupStudent);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateSubscription(int groupId, int studentId, int newSubscriptionId, CancellationToken cancellationToken = default)
        {
            var groupStudent = await _context.GroupStudents
                .FirstOrDefaultAsync(gs =>
                    gs.GroupId == groupId &&
                    gs.StudentId == studentId, cancellationToken);
            if (groupStudent == null)
                throw new ArgumentNullException("Данной записи не существует");

            var newSub = await _context.StudentSubscriptions
                .FirstOrDefaultAsync(ss => ss.Id == newSubscriptionId, cancellationToken);
            if (newSub == null)
                throw new ArgumentNullException("Данной записи не существует");

            groupStudent.ActiveSubscriptionId = newSub.Id;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
