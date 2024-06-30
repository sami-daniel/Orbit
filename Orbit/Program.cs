using Microsoft.EntityFrameworkCore;
using Orbit.Application.Interfaces;
using Orbit.Application.Services;
using Orbit.Infrastructure.Data.Contexts;
using Orbit.Infrastructure.Repositories;
using Orbit.Infrastructure.Repositories.Implementations;
using Orbit.Infrastructure.Repositories.Interfaces;

namespace Orbit
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            _ = builder.Services.AddControllersWithViews();

            _ = builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseMySql(connectionString: builder.Configuration.GetConnectionString("OrbitConnection"),
             serverVersion: new MySqlServerVersion(new Version(8, 4, 0))));

            _ = builder.Services.AddScoped<IUserRepository, UserRepository>();

            _ = builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            _ = builder.Services.AddScoped<IUserService, UserService>();

            _ = builder.Services.AddSession();

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }
            else
            {
                _ = app.UseExceptionHandler("Home");

                _ = app.UseStatusCodePagesWithReExecute("Home", "?statusCode={0}");

                _ = app.UseHsts();
            }

            _ = app.UseStaticFiles();

            _ = app.UseRouting();

            _ = app.UseSession();

            _ = app.MapControllerRoute(name: "default",
                                       pattern: "{controller=Account}/{action=Index}");

            app.Run();

        }
    }
}