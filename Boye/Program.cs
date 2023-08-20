using AspNetCore.SEOHelper;
using Boye.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Boye
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //builder.Services.AddDbContext<StoreContext>(
            //  o => o.UseMySql(config["ConnectionString"], ServerVersion.AutoDetect(config["ConnectionString"]))
            //   );
            builder.Services.AddHttpClient();

            DependecyInjection.Register(builder.Services);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    scope.ServiceProvider.GetService<StoreContext>().Database.Migrate();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseXMLSitemap(app.Environment.ContentRootPath);

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}