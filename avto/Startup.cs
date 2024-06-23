using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using avto.DataBase; // Для использования CarDealershipDbContext
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace avto.Pages
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CarDealershipDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            // Ensure database is created and apply migrations
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<CarDealershipDbContext>();
                context.Database.Migrate();
                AddInitialData(context);
            }
        }

        private void AddInitialData(CarDealershipDbContext context)
        {
            // Добавьте начальные данные в таблицы здесь
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { RoleName = "Admin" },
                    new Role { RoleName = "User" }
                );
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Username = "admin",
                        Password = "adminpass",
                        Email = "admin@example.com",
                        Phone = "1234567890",
                        Address = "Admin Address"
                    },
                    new User
                    {
                        Username = "user",
                        Password = "userpass",
                        Email = "user@example.com",
                        Phone = "0987654321",
                        Address = "User Address"
                    }
                );
                context.SaveChanges();
            }

            // Добавьте другие начальные данные по аналогии
        }
    }
}