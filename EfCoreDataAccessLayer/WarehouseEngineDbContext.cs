using EfCoreDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfCoreDataAccessLayer
{
    public class WarehouseEngineDbContext : DbContext
    {
        public DbSet<Detail> Details { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public WarehouseEngineDbContext(DbContextOptions<WarehouseEngineDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Detail>()
                 .HasOne(d => d.Supplier)
                 .WithMany(s => s.Details)
                 .HasForeignKey(d => d.SupplierId);
        }
    }
}
