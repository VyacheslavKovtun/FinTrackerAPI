using FinTrackerAPI.Domain.Core.Entities;
using FinTrackerAPI.Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace FinTrackerAPI.Infrastructure.Data.Repository
{
    public class TransactionRepository : BaseRepository<Guid, Transaction, FinDbContext>
    {
        public TransactionRepository(FinDbContext ctx) : base(ctx) { }

        public async override Task<Transaction> GetAsync(Guid id)
        {
            var transaction = await table.FirstOrDefaultAsync(t => t.Id == id);

            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(Guid userId)
        {
            var transactions = table.Where(t => t.UserId.Equals(userId));

            return transactions;
        }

        public async Task<IEnumerable<Transaction>> GetByCategoryAsync(int categoryId)
        {
            var transactions = table.Where(t => t.CategoryId == categoryId);

            return transactions;
        }

        public async override Task UpdateAsync(Transaction value)
        {
            var transaction = await GetAsync(value.Id);

            transaction.UserId = value.UserId;
            transaction.CategoryId = value.CategoryId;
            transaction.Amount = value.Amount;
            transaction.CurrencyId = value.CurrencyId;
            transaction.Date = value.Date;
            transaction.Notes = value.Notes;

            ctx.Entry(transaction).State = EntityState.Modified;
            await ctx.SaveChangesAsync();
        }
    }
}
