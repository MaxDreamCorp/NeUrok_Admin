using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            return await _context.Subscribtions.ToListAsync(cancellationToken);
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
