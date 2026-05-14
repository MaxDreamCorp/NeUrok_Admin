using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Course course, CancellationToken cancellationToken = default)
        {
            await _context.Courses.AddAsync(course, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Course>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Courses.ToListAsync(cancellationToken);
        }

        public async Task<Course?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Courses.FindAsync(id, cancellationToken);
        }

        public async Task<Course?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Courses.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Courses.AnyAsync() ? await _context.Courses.MaxAsync(x => x.Id) + 1 : 1;
        }

        public async Task RemoveAsync(Course course, CancellationToken cancellationToken = default)
        {
            if (await _context.StudentSubscriptions.AnyAsync(x => x.CourseId == course.Id, cancellationToken))
                throw new InvalidOperationException("Невозможно удалить курс, так как на него подписаны ученики");
            if (await _context.Attendances.AnyAsync(x => x.CourseId == course.Id, cancellationToken))
                throw new InvalidOperationException("Невозможно удалить курс, так как на него есть/были посещения");
            if (await _context.Groups.AnyAsync(x => x.CourseId == course.Id, cancellationToken))
                throw new InvalidOperationException("Невозможно удалить курс, так как на него есть/были группы");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Course course, CancellationToken cancellationToken = default)
        {
            var existingCourse = await GetByIdAsync(course.Id, cancellationToken);
            if (existingCourse == null)
                throw new ArgumentNullException("Данного курса не существует");

            existingCourse.Name = course.Name;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
