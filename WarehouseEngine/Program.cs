using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using EfCoreDataAccessLayer;
using Microsoft.EntityFrameworkCore;
using WarehouseEngineBusinessLayer.Interfaces;
using WarehouseEngineBusinessLayer.Mappings;
using WarehouseEngineBusinessLayer.Services;

namespace WarehouseEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<WarehouseEngineDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["ConnectionString"]));
            builder.Services.AddAutoMapper(typeof(WarehouseEngineMappingProfile));
            builder.Services.AddScoped(typeof(ICrudService<,>), typeof(CrudService<,>));
            builder.Services.AddTransient<IExcelExportService, ExcelExportService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}