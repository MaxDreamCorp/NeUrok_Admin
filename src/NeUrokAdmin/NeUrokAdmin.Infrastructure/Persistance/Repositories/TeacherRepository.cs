using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;

        public TeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Teacher teacher, CancellationToken cancellationToken = default)
        {
            await _context.Teachers.AddAsync(teacher, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Teacher>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Teachers.ToListAsync(cancellationToken);
        }

        public async Task<Teacher?> GetByFullnameAsync(string fullname, CancellationToken cancellationToken = default)
        {
            return await _context.Teachers.FirstOrDefaultAsync(t => t.Fullname == fullname, cancellationToken);
        }

        public async Task<Teacher?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Teachers.AnyAsync(cancellationToken)
                ? await _context.Teachers.MaxAsync(t => t.Id, cancellationToken) + 1
                : 1;
        }

        public async Task RemoveAsync(Teacher teacher, CancellationToken cancellationToken = default)
        {
            if (await _context.Groups.AnyAsync(g => g.TeacherId == teacher.Id, cancellationToken))
                throw new InvalidOperationException("Невозможно удалить учителя, так как он ведет группы");
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Teacher teacher, CancellationToken cancellationToken = default)
        {
            var existingTeacher = await GetByIdAsync(teacher.Id, cancellationToken);
            if (existingTeacher == null)
                throw new Exception("Такого учителя не существует");

            existingTeacher.Fullname = teacher.Fullname;
            existingTeacher.IndividualLessonsShare = teacher.IndividualLessonsShare;
            existingTeacher.Notes = teacher.Notes;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
