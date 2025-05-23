using FinTrackerAPI.Domain.Core.Entities;
using FinTrackerAPI.Infrastructure.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace FinTrackerAPI.Infrastructure.Data.Repository
{
    public class UserRepository : BaseRepository<Guid, User, FinDbContext>
    {
        public UserRepository(FinDbContext ctx) : base(ctx) { }

        public async override Task<User> GetAsync(Guid id)
        {
            var user = await table.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await table.FirstOrDefaultAsync(u => u.Email.ToLower() == email.Trim().ToLower());

            return user;
        }

        public async Task<User> GetByEmailLoginDataAsync(string email, string password)
        {
            var updatedEmail = email.Trim().ToLower();

            //CONVERT HASH TO Password

            var user = await table.FirstOrDefaultAsync(u => u.Email.ToLower() == updatedEmail && u.PasswordHash == password);

            return user;
        }

        public async override Task UpdateAsync(User value)
        {
            var user = await GetAsync(value.Id);

            user.Name = value.Name;
            user.Email = value.Email;
            user.PasswordHash = value.PasswordHash;
            user.PreferredCurrencyCode = value.PreferredCurrencyCode;
            user.AvatarUrl = value.AvatarUrl;
            user.CreatedAt = value.CreatedAt;
            user.UpdatedAt = value.UpdatedAt;

            ctx.Entry(user).State = EntityState.Modified;
            await ctx.SaveChangesAsync();
        }
    }
}
