using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBIS_API.Models
{
    public class OrderContext : DbContext
    {
        public OrderContext() : base() { }

        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
