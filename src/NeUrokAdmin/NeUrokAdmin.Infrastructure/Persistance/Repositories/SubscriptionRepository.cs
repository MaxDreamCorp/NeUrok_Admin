using Microsoft.EntityFrameworkCore;
using NeUrokAdmin.Domain.DTOs.SearchDTOs;
using NeUrokAdmin.Domain.Entities;
using NeUrokAdmin.Domain.Interfaces.Repositories;

namespace NeUrokAdmin.Infrastructure.Persistance.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
        {
            await _context.Subscribtions.AddAsync(subscription, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Subscription>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Subscribtions
                .Include(s => s.ClassesType)
                .ToListAsync(cancellationToken);
        }

        public async Task<Subscription?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Subscribtions
                .Include(s => s.ClassesType)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Subscribtions.AnyAsync(cancellationToken)
                ? await _context.Subscribtions.MaxAsync(s => s.Id, cancellationToken) + 1
                : 1;
        }

        public async Task RemoveAsync(Subscription subscription, CancellationToken cancellationToken = default)
        {
            _context.Subscribtions.Remove(subscription);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<List<Subscription>> SearchAsync(SubscriptionSearchDTO request, CancellationToken cancellationToken = default)
        {
            IQueryable<Subscription> query = _context.Subscribtions
                .Include(s => s.ClassesType)
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

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(c => c.Name.Contains(request.Name));

            if (!string.IsNullOrWhiteSpace(request.ClassesType))
                query = query.Where(c => c.ClassesType.Type.Contains(request.ClassesType));

            if (request.Cost != null)
                query = query.Where(c => c.Cost == request.Cost);
            else
            {
                if (request.CostFrom != null)
                    query = query.Where(c => c.Cost >= request.CostFrom);

                if (request.CostTo != null)
                    query = query.Where(c => c.Cost <= request.CostTo);
            }

            if (request.ClassesAmount != null)
                query = query.Where(c => c.ClassesAmount == request.ClassesAmount);
            else
            {
                if (request.ClassesAmountFrom != null)
                    query = query.Where(c => c.ClassesAmount >= request.ClassesAmountFrom);

                if (request.ClassesAmountTo != null)
                    query = query.Where(c => c.ClassesAmount <= request.ClassesAmountTo);
            }

            if (request.ClassesTypeIds != null && request.ClassesTypeIds.Any())
                query = query.Where(c => request.ClassesTypeIds.Contains(c.ClassesTypeId));

            return query.ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default)
        {
            var existingSubscription = await GetByIdAsync(subscription.Id, cancellationToken);
            if (existingSubscription == null)
                throw new Exception("Данного абонемента не существует");

            existingSubscription.Name = subscription.Name;
            existingSubscription.ClassesTypeId = subscription.ClassesTypeId;
            existingSubscription.Cost = subscription.Cost;
            existingSubscription.ClassesAmount = subscription.ClassesAmount;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
