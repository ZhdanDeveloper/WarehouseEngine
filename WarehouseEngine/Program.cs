using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using EfCoreDataAccessLayer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
            builder.Services.AddServerSideBlazor();
            builder.Services.AddDbContext<WarehouseEngineDbContext>(options =>


            options.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["ConnectionString"]));


            builder.Services.AddAutoMapper(typeof(WarehouseEngineMappingProfile));

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped(typeof(ICrudService<,>), typeof(CrudService<,>));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddTransient<IExcelExportService, ExcelExportService>();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.Cookie.HttpOnly = true;
                   options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                   options.LoginPath = "/User/Login";
                   options.AccessDeniedPath = "/User/AccessDenied";
               });

            // Добавление авторизации
            builder.Services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapBlazorHub();
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}