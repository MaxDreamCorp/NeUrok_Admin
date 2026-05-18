using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Attendance attendance, CancellationToken cancellationToken = default)
        {
            await _context.Attendances.AddAsync(attendance, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Attendance>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
        {
            return await _context.Attendances
                .Include(a => a.Course)
                .Include(a => a.ClassType)
                .Include(a => a.Teacher)
                .Include(a => a.AttendanceStatus)
                .Include(a => a.AttendanceType)
                .Where(a => a.ClientId == clientId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Attendance>> GetByGroupAndClientIdAsync(int groupId, int clientId, CancellationToken cancellationToken = default)
        {
            return await _context.Attendances
                .Include(a => a.Course)
                .Include(a => a.ClassType)
                .Include(a => a.Teacher)
                .Include(a => a.AttendanceStatus)
                .Include(a => a.AttendanceType)
                .Where(a => a.GroupId == groupId && a.ClientId == clientId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Attendance>> GetByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
        {
            return await _context.Attendances
                .Include(a => a.Course)
                .Include(a => a.ClassType)
                .Include(a => a.Teacher)
                .Include(a => a.AttendanceStatus)
                .Include(a => a.AttendanceType)
                .Where(a => a.GroupId == groupId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Attendance?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Attendances
                .Include(a => a.Course)
                .Include(a => a.ClassType)
                .Include(a => a.Teacher)
                .Include(a => a.AttendanceStatus)
                .Include(a => a.AttendanceType)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Attendances.AnyAsync() ?
                await _context.Attendances.MaxAsync(a => a.Id, cancellationToken) + 1 : 1;
        }

        public async Task RemoveAsync(Attendance attendance, CancellationToken cancellationToken = default)
        {
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Attendance attendance, CancellationToken cancellationToken = default)
        {
            var existingAttendance = await GetByIdAsync(attendance.Id, cancellationToken);
            if (existingAttendance == null)
                throw new ArgumentNullException("Данной записи не существует");

            existingAttendance.ClientId = attendance.ClientId;
            existingAttendance.CourseId = attendance.CourseId;
            existingAttendance.ClassTypeId = attendance.ClassTypeId;
            existingAttendance.TeacherId = attendance.TeacherId;
            existingAttendance.GroupId = attendance.GroupId;
            existingAttendance.IsCompleted = attendance.IsCompleted;
            existingAttendance.AttendanceStatusId = attendance.AttendanceStatusId;
            existingAttendance.AttendanceTypeId = attendance.AttendanceTypeId;
            existingAttendance.Price = attendance.Price;
            existingAttendance.TeacherShare = attendance.TeacherShare;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
