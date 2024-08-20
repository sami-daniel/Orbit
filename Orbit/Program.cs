using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Orbit.Application.Interfaces;
using Orbit.Application.Services;
using Orbit.Infrastructure.Data.Contexts;
using Orbit.Infrastructure.Repositories;
using Orbit.Infrastructure.Repositories.Implementations;
using Orbit.Infrastructure.Repositories.Interfaces;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Orbit
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            _ = builder.Services.AddControllersWithViews().AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            _ = builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseMySql(connectionString: builder.Configuration.GetConnectionString("OrbitConnection"),
             serverVersion: new MySqlServerVersion(new Version(8, 4, 0)))
            .LogTo(m => Debug.WriteLine(m)));

            _ = builder.Services
                .AddSession(opt =>
                {
                    opt.Cookie.Name = "session";
                });

            _ = builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.RequireAuthenticatedSignIn = false;
            }).AddCookie(opt =>
            {
                opt.LoginPath = "/Account/";
                opt.Cookie.Name = "auth";
            });

            _ = builder.Services.AddAuthorization();

            _ = builder.Services.AddAntiforgery(opt =>
            {
                opt.Cookie.Name = "Antiforgery";
            });

            _ = builder.Services.AddScoped<IUserRepository, UserRepository>();

            _ = builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            _ = builder.Services.AddScoped<IUserService, UserService>();

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

            _ = app.UseAuthentication();

            _ = app.UseAuthorization();

            _ = app.UseSession();

            _ = app.MapControllerRoute(name: "default",
                                       pattern: "{controller=Account}/{action=Index}");

            app.Run();

        }
    }
}