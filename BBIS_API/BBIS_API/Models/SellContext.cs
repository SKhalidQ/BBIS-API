using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BBIS_API.Models
{
    public class SellContext : DbContext
    {
        public SellContext() : base() { }
        public SellContext(DbContextOptions<SellContext> options) : base(options) { }
        public DbSet<SellItem> SellItems { get; set; }
    }
}
