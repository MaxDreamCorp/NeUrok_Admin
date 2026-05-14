using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Student student, CancellationToken cancellationToken = default)
        {
            await _context.Students.AddAsync(student, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Student>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Students
                .Include(s => s.StudentSubscriptions)
                .ToListAsync(cancellationToken);
        }

        public async Task<Student?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Students
                .Include(s => s.StudentSubscriptions)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Students.AnyAsync(cancellationToken)
                ? await _context.Students.MaxAsync(s => s.Id, cancellationToken) + 1
                : 1;
        }

        public async Task RemoveAsync(Student student, CancellationToken cancellationToken = default)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Student student, CancellationToken cancellationToken = default)
        {
            var existingStudent = await GetByIdAsync(student.Id, cancellationToken);
            if (existingStudent == null)
                throw new InvalidOperationException($"Студент с ID {student.Id} не найден.");

            existingStudent.ClientId = student.ClientId;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
