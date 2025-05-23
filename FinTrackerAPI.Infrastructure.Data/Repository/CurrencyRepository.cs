using FinTrackerAPI.Domain.Core.Entities;
using FinTrackerAPI.Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace FinTrackerAPI.Infrastructure.Data.Repository
{
    public class CurrencyRepository : BaseRepository<int, Currency, FinDbContext>
    {
        public CurrencyRepository(FinDbContext ctx) : base(ctx) { }

        public async override Task<Currency> GetAsync(int id)
        {
            var currency = await table.FirstOrDefaultAsync(c => c.Id == id);

            return currency;
        }

        public async Task<Currency> GetByCodeAsync(string code)
        {
            var currency = await table.FirstOrDefaultAsync(c => c.Code == code);

            return currency;
        }

        public async override Task UpdateAsync(Currency value)
        {
            var currency = await GetAsync(value.Id);

            currency.Code = value.Code;
            currency.Name = value.Name;
            currency.Symbol = value.Symbol;

            ctx.Entry(currency).State = EntityState.Modified;
            await ctx.SaveChangesAsync();
        }
    }
}
