using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Orbit.Application.Interfaces;
using Orbit.Application.Services;
using Orbit.Hubs;
using Orbit.Infrastructure.Data.Contexts;
using Orbit.Infrastructure.Repository.Implementations;
using Orbit.Infrastructure.Repository.Interfaces;
using Orbit.Infrastructure.UnitOfWork.Implementations;
using Orbit.Infrastructure.UnitOfWork.Interfaces;
using Orbit.Profiles;

namespace Orbit
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews().AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseMySql(connectionString: builder.Configuration.GetConnectionString("OrbitConnection"),
             serverVersion: new MySqlServerVersion(new Version(8, 4, 0)))
            .LogTo(m => Debug.WriteLine(m))
            .EnableSensitiveDataLogging());

            builder.Services.AddAutoMapper(opt =>
            {
                opt.AddProfile<UserProfile>();
            });

            builder.Services
                .AddSession(opt =>
                {
                    opt.Cookie.Name = "session";
                });

            builder.Services.AddAuthentication(opt =>
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

            builder.Services.AddAuthorization();

            builder.Services.AddAntiforgery(opt =>
            {
                opt.Cookie.Name = "Antiforgery";
            });

            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddSignalR();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("Home");

                app.UseStatusCodePagesWithReExecute("Home", "?statusCode={0}");

                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.MapDefaultControllerRoute();

            app.MapHub<ChatHub>("/chathub");

            app.MapHub<NotificationHub>("/notification");

            app.Run();

        }
    }
}
