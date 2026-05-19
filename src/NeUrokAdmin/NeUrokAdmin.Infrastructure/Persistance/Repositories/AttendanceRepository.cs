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
            attendance.Id = await GetNextIdAsync(cancellationToken);

            var trackedEntity = _context.Attendances.Local.FirstOrDefault(g => g.Id == attendance.Id);

            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).State = EntityState.Detached;
            }

            if (await _context.Attendances.AnyAsync(g => g.Id == attendance.Id, cancellationToken))
                throw new Exception("Такой ID уже реально есть в самой БД!");

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

        public async Task<List<DateTime>> GetDatesWithCompletedAttendanceAsync(int groupId, List<DateTime> datesToCheck, CancellationToken cancellationToken)
        {
            return await _context.Attendances
                .Where(a => a.GroupId == groupId
                         && datesToCheck.Contains(a.Datetime)
                         && a.IsCompleted == 1)
                .Select(a => a.Datetime)
                .Distinct()
                .ToListAsync(cancellationToken);
        }

        public async Task RemoveByGroupDateAsync(int groupId, DateTime dateTime, CancellationToken cancellationToken = default)
        {
            var attendences = await _context.Attendances
                .Where(gd => gd.GroupId == groupId &&
                                     gd.Datetime == dateTime)
                .ToListAsync(cancellationToken);

            foreach (var item in attendences)
            {
                await RemoveAsync(item, cancellationToken);
            }
        }

        public async Task RemoveFutureAttendanceForStudentsAsync(int groupId, List<int> clientIds, CancellationToken cancellationToken = default)
        {
            await _context.Attendances
        .Where(a => a.GroupId == groupId &&
                    a.Datetime >= DateTime.Now &&
                    a.ClientId.HasValue &&
                    clientIds.Contains(a.ClientId.Value))
        .ExecuteDeleteAsync(cancellationToken);
        }
    }
}
