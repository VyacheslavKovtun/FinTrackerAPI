using FinTrackerAPI.Domain.Core.Entities;
using FinTrackerAPI.Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace FinTrackerAPI.Infrastructure.Data.Repository
{
    public class CurrencyRepository
    {
        protected FinDbContext ctx;
        protected DbSet<Currency> table => ctx.Set<Currency>();

        public CurrencyRepository(FinDbContext ctx) 
        {
            this.ctx = ctx;
        }

        public async Task CreateAsync(Currency value)
        {
            ctx.Entry(value).State = EntityState.Added;
            await ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<Currency>> GetAllAsync()
        {
            return await table.ToListAsync().ConfigureAwait(false);
        }

        public async Task<Currency> GetAsync(string code)
        {
            var currency = await table.FirstOrDefaultAsync(c => c.Code == code);

            return currency;
        }

        public async Task UpdateAsync(Currency value)
        {
            var currency = await GetAsync(value.Code);

            currency.Name = value.Name;
            currency.Symbol = value.Symbol;

            ctx.Entry(currency).State = EntityState.Modified;
            await ctx.SaveChangesAsync();
        }

        public async Task DeleteAsync(string code)
        {
            var item = await GetAsync(code);
            ctx.Entry(item).State = EntityState.Deleted;
            await ctx.SaveChangesAsync();
        }
    }
}
