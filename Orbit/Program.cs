using System.Diagnostics;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Orbit.Data.Contexts;
using Orbit.Hubs;
using Orbit.Profiles;
using Orbit.Repository.Implementations;
using Orbit.Repository.Interfaces;
using Orbit.Services.Implementations;
using Orbit.Services.Interfaces;
using Orbit.UnitOfWork.Interfaces;
using Orbit.UnitOfWork.Implementations;

namespace Orbit;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews()
            .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
        builder.Services.AddRouting(opt =>
        {
            opt.LowercaseUrls = true;
        });

        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        opt.UseMySql(connectionString: builder.Configuration.GetConnectionString("OrbitConnection") ??
        throw new InvalidOperationException("Connection string not found"),
         serverVersion: new MySqlServerVersion(new Version(8, 4, 0)))
        .LogTo(m => Debug.WriteLine(m)));

        builder.Services.AddSingleton<IMessageService, MessageService>();

        builder.Services.AddAutoMapper(opt =>
        {
            opt.AddProfile<UserProfile>();
            opt.AddProfile<PostProfile>();
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
            opt.LoginPath = "/account/";
            opt.Cookie.Name = "auth";
        });

        builder.Services.AddAuthorization();

        builder.Services.AddAntiforgery(opt =>
        {
            opt.Cookie.Name = "Antiforgery";
        });

        builder.Services.AddScoped<IUserRepository, UserRepository>();

        builder.Services.AddScoped<IPostRepository, PostRepository>();

        builder.Services.AddScoped<ILikeRepository, LikeRepository>();

        builder.Services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();

        builder.Services.AddScoped<IPostPreferenceRepository, PostPreferenceRepository>();

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork.Implementations.UnitOfWork>();

        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddScoped<IPostService, PostService>();

        builder.Services.AddSignalR();

        var app = builder.Build();

        var context = app.Services.CreateScope().ServiceProvider
                                  .GetRequiredService<ApplicationDbContext>();
        
        using (context)
        {
            context.Database.Migrate();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/home");

            app.UseStatusCodePagesWithReExecute("/home", "?statusCode={0}");

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
