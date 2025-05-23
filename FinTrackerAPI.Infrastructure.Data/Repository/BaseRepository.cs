using FinTrackerAPI.Domain.Interfaces.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackerAPI.Infrastructure.Data.Repository
{
    public abstract class BaseRepository<TKey, TValue, TContext>: IRepository<TKey, TValue>
        where TKey: struct
        where TValue : class
        where TContext : DbContext
    {
        protected TContext ctx;
        protected DbSet<TValue> table => ctx.Set<TValue>();

        public BaseRepository(TContext ctx) 
        {
            this.ctx = ctx;
        }

        public async Task CreateAsync(TValue value)
        {
            ctx.Entry(value).State = EntityState.Added;
            await ctx.SaveChangesAsync();
        }

        public async Task<IEnumerable<TValue>> GetAllAsync()
        {
            return await table.ToListAsync().ConfigureAwait(false);
        }

        public abstract Task<TValue> GetAsync(TKey id);

        public abstract Task UpdateAsync(TValue value);

        public async Task DeleteAsync(TKey id)
        {
            var item = await GetAsync(id);
            ctx.Entry(item).State = EntityState.Deleted;
            await ctx.SaveChangesAsync();
        }
    }
}
