using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeUrokAdmin.Domain.Entities;

namespace NeUrokAdmin.Domain.Interfaces.Repositories
{
    public interface IAttendanceTypeRepository
    {
        Task<AttendanceType?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<AttendanceType>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
