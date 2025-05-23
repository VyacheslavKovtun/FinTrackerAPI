using FinTrackerAPI.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinTrackerAPI.Infrastructure.Data.Database
{
    public class FinDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        public FinDbContext(DbContextOptions<FinDbContext> options) : base(options) { }
    }
}
