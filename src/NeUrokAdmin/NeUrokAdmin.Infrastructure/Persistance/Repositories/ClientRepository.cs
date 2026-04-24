using Microsoft.EntityFrameworkCore;
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

        public async Task UpdateAsync(Client client, CancellationToken cancellationToken = default)
        {
            var exestingClient = await GetByIdAsync(client.Id, cancellationToken);
            if (exestingClient == null)
                throw new ArgumentException();

            exestingClient.ChildFullname = client.ChildFullname;
            exestingClient.BirthDate = client.BirthDate;
            exestingClient.RegistrationDate = client.RegistrationDate;
            exestingClient.Status = client.Status;
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
