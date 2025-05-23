using FinTrackerAPI.Domain.Core.Entities;
using FinTrackerAPI.Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace FinTrackerAPI.Infrastructure.Data.Repository
{
    public class CategoryRepository : BaseRepository<int, Category, FinDbContext>
    {
        public CategoryRepository(FinDbContext ctx) : base(ctx) { }

        public async override Task<Category> GetAsync(int id)
        {
            var category = await table.FirstOrDefaultAsync(c => c.Id == id);

            return category;
        }

        public async override Task UpdateAsync(Category value)
        {
            var category = await GetAsync(value.Id);

            category.Name = value.Name;
            category.Icon = value.Icon;
            category.Color = value.Color;

            ctx.Entry(category).State = EntityState.Modified;
            await ctx.SaveChangesAsync();
        }
    }
}
