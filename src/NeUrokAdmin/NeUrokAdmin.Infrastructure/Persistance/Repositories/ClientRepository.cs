using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.DTOs.SearchDTOs;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Client client, CancellationToken cancellationToken = default)
        {
            await _context.Clients.AddAsync(client, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Clients
                .Include(c => c.Status)
                .Include(c => c.Courses)
                .ToListAsync(cancellationToken);
        }

        public async Task<Client?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Clients
                .Include(c => c.Status)
                .Include(c => c.Courses)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Clients.AnyAsync() ? await _context.Clients.MaxAsync(x => x.Id) + 1 : 1;
        }

        public async Task RemoveAsync(Client client, CancellationToken cancellationToken = default)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<List<Client>> SearchAsync(ClientSearchDTO request, CancellationToken cancellationToken = default)
        {
            IQueryable<Client> query = _context.Clients
                .Include(c => c.Status)
                .Include(c => c.Courses)
                .AsQueryable();

            if (request.Id != null)
                query = query.Where(c => c.Id == request.Id);
            else
            {
                if (request.IdFrom != null)
                    query = query.Where(c => c.Id >= request.IdFrom);

                if (request.IdTo != null)
                    query = query.Where(c => c.Id <= request.IdTo);
            }

            if (!string.IsNullOrEmpty(request.ChildFullname))
                query = query.Where(c => c.ChildFullname.Contains(request.ChildFullname));

            if (!string.IsNullOrEmpty(request.ParentName))
                query = query.Where(c => c.ParentName.Contains(request.ParentName));

            if (!string.IsNullOrEmpty(request.Phone))
                query = query.Where(c => c.Phone.Contains(request.Phone));

            if (!string.IsNullOrEmpty(request.AdditionalPhone))
                query = query.Where(c => c.AdditionalPhones != null && c.AdditionalPhones.Contains(request.AdditionalPhone));

            if (!string.IsNullOrEmpty(request.Notes))
                query = query.Where(c => c.Notes != null && c.Notes.Contains(request.Notes));

            if (!string.IsNullOrEmpty(request.Status))
                query = query.Where(c => c.Status.Status.Contains(request.Status));

            if (request.Grade != null)
                query = query.Where(c => c.Grade == request.Grade);
            else
            {
                if (request.GradeFrom != null)
                    query = query.Where(c => c.Grade >= request.GradeFrom);

                if (request.GradeTo != null)
                    query = query.Where(c => c.Grade <= request.GradeTo);
            }

            if (request.BirthDate != null)
                query = query.Where(c => c.BirthDate == request.BirthDate);
            else
            {
                if (request.BirthDateFrom != null)
                    query = query.Where(c => c.BirthDate >= request.BirthDateFrom);

                if (request.BirthDateTo != null)
                    query = query.Where(c => c.BirthDate <= request.BirthDateTo);

                if (request.BirthDateDay != null)
                    query = query.Where(c => c.BirthDate.HasValue && c.BirthDate.Value.Day == request.BirthDateDay);

                if (request.BirthDateMonth != null)
                    query = query.Where(c => c.BirthDate.HasValue && c.BirthDate.Value.Month == request.BirthDateMonth);

                if (request.BirthDateYear != null)
                    query = query.Where(c => c.BirthDate.HasValue && c.BirthDate.Value.Year == request.BirthDateYear);
            }

            if (request.RegistrationDate != null)
                query = query.Where(c => c.RegistrationDate == request.RegistrationDate);
            else
            {
                if (request.RegistrationDateFrom != null)
                    query = query.Where(c => c.RegistrationDate >= request.RegistrationDateFrom);

                if (request.RegistrationDateTo != null)
                    query = query.Where(c => c.RegistrationDate <= request.RegistrationDateTo);

                if (request.RegistrationDateDay != null)
                    query = query.Where(c => c.RegistrationDate.Day == request.RegistrationDateDay);

                if (request.RegistrationDateMonth != null)
                    query = query.Where(c => c.RegistrationDate.Month == request.RegistrationDateMonth);

                if (request.RegistrationDateYear != null)
                    query = query.Where(c => c.RegistrationDate.Year == request.RegistrationDateYear);
            }

            if (request.WishedCourseIds != null && request.WishedCourseIds.Any())
                query = query.Where(c => c.Courses.Any(course => request.WishedCourseIds.Contains(course.Id)));

            if (request.StatusIds != null && request.StatusIds.Any())
                query = query.Where(c => request.StatusIds.Contains(c.StatusId));

            return query.ToListAsync(cancellationToken);

        }

        public async Task UpdateAsync(Client client, CancellationToken cancellationToken = default)
        {
            var exestingClient = await GetByIdAsync(client.Id, cancellationToken);
            if (exestingClient == null)
                throw new ArgumentException();

            exestingClient.ChildFullname = client.ChildFullname;
            exestingClient.BirthDate = client.BirthDate;
            exestingClient.RegistrationDate = client.RegistrationDate;
            exestingClient.StatusId = client.StatusId;
            exestingClient.Grade = client.Grade;
            exestingClient.ParentName = client.ParentName;
            exestingClient.Phone = client.Phone;
            exestingClient.Notes = client.Notes;
            exestingClient.AdditionalPhones = client.AdditionalPhones;
            exestingClient.Courses = client.Courses;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
