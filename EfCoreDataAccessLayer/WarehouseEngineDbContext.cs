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
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public WarehouseEngineDbContext(DbContextOptions<WarehouseEngineDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                 .HasOne(d => d.Supplier)
                 .WithMany(s => s.Products)
                 .HasForeignKey(d => d.SupplierId);
        }
    }
}
