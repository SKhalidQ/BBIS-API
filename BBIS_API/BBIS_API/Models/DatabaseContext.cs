using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BBIS_API.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<ProductItem> ProductItems { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<SellItem> SellItems { get; set; }
    }
}
