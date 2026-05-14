using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class StudentSubscriptionRepository : IStudentSubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentSubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(StudentSubscription studentSubscription, CancellationToken cancellationToken = default)
        {
            await _context.StudentSubscriptions.AddAsync(studentSubscription, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<StudentSubscription?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.StudentSubscriptions
                .Include(ss => ss.Subscription)
                    .ThenInclude(s => s.ClassesType)
                .Include(ss => ss.Course)
                .Include(ss => ss.SubscriptlonStatus)
                .FirstOrDefaultAsync(ss => ss.Id == id);
        }

        public async Task<List<StudentSubscription>> GetByStudentIdAsync(int studentId, CancellationToken cancellationToken = default)
        {
            return await _context.StudentSubscriptions
                .Include(ss => ss.Subscription)
                    .ThenInclude(s => s.ClassesType)
                .Include(ss => ss.Course)
                .Include(ss => ss.SubscriptlonStatus)
                .Where(ss => ss.StudentId == studentId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
        {
            return await _context.StudentSubscriptions.AnyAsync(cancellationToken)
                ? await _context.StudentSubscriptions.MaxAsync(ss => ss.Id, cancellationToken) + 1
                : 1;
        }

        public async Task RemoveAsync(StudentSubscription studentSubscription, CancellationToken cancellationToken = default)
        {
            _context.StudentSubscriptions.Remove(studentSubscription);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(StudentSubscription studentSubscription, CancellationToken cancellationToken = default)
        {
            var existingStudentSubscription = await GetByIdAsync(studentSubscription.Id, cancellationToken);
            if (existingStudentSubscription == null)
                throw new Exception("Данной записи не существует");

            existingStudentSubscription.SubscriptionId = studentSubscription.SubscriptionId;
            existingStudentSubscription.IsPaid = studentSubscription.IsPaid;
            existingStudentSubscription.CourseId = studentSubscription.CourseId;
            existingStudentSubscription.SubscriptlonStatusId = studentSubscription.SubscriptlonStatusId;
            existingStudentSubscription.SubscriptionStartDate = studentSubscription.SubscriptionStartDate;
            existingStudentSubscription.SubscriptionFinishDate = studentSubscription.SubscriptionFinishDate;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
