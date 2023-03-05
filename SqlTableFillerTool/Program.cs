using EfCoreDataAccessLayer;
using EfCoreDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace SqlTableFillerTool
{
    internal class Program
    {
        public static void FillDatabaseWithData()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WarehouseEngineDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER01;Database=WarehouseEngineDb;Trusted_Connection=True; TrustServerCertificate=True;");
            using (WarehouseEngineDbContext dbContext = new WarehouseEngineDbContext(optionsBuilder.Options))
            {


                var random = new Random();

                // Generate 1000 random details
                var details = Enumerable.Range(1, 1000).Select(i => new Detail
                {
                    Name = $"Detail {i}",
                    Price = random.Next(100, 1000),
                    SupplierId = random.Next(1, 100),
                    Description = $"DESC {i}",
                    Quantity = random.Next(100, 1000),                   
                });

                // Generate 100 random suppliers
                var suppliers = Enumerable.Range(100, 300).Select(i => new Supplier
                {
                    Name = $"Supplier {i}",
                    Address = $"Address {i}",
                    Email = $"email{i}@gmail.com",
                    Phone =$"3809541{i}"
                    
                });

               // foreach (var item in details) { Console.WriteLine(item.Name); }
                foreach (var item in suppliers) { Console.WriteLine(item.Name); }


                dbContext.Suppliers.AddRange(suppliers);
                dbContext.SaveChanges();
            //    dbContext.Details.AddRange(details);
            //    dbContext.SaveChanges();
            }
        }
        static void Main(string[] args)
        {
            FillDatabaseWithData();
        }
    }
}