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
        // Create a web application builder to configure services and middlewares
        var builder = WebApplication.CreateBuilder(args);

        // Configure JSON serializer settings and ensure reference handling for circular references
        builder.Services.AddControllersWithViews()
            .AddJsonOptions(opt => opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

        // Configure routing to use lowercase URLs
        builder.Services.AddRouting(opt =>
        {
            opt.LowercaseUrls = true;
        });

        // Configure database context for MySQL, with connection string and version details
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        opt.UseMySql(connectionString: builder.Configuration.GetConnectionString("OrbitConnection") ??
        throw new InvalidOperationException("Connection string not found"),
         serverVersion: new MySqlServerVersion(new Version(8, 4, 0)))
        .LogTo(m => Debug.WriteLine(m)));

        // Register message service as a singleton
        builder.Services.AddSingleton<IMessageService, MessageService>();

        // Configure AutoMapper to use user and post profiles
        builder.Services.AddAutoMapper(opt =>
        {
            opt.AddProfile<UserProfile>();
            opt.AddProfile<PostProfile>();
        });

        // Configure session handling with custom cookie name
        builder.Services
            .AddSession(opt =>
            {
                opt.Cookie.Name = "session";
            });

        // Configure authentication with cookie-based authentication scheme
        builder.Services.AddAuthentication(opt =>
        {
            opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            opt.RequireAuthenticatedSignIn = false;
        }).AddCookie(opt =>
        {
            // Set login path and cookie name for authentication
            opt.LoginPath = "/account/";
            opt.Cookie.Name = "auth";
        });

        // Add authorization services
        builder.Services.AddAuthorization();

        // Configure anti-forgery settings, with a custom cookie name
        builder.Services.AddAntiforgery(opt =>
        {
            opt.Cookie.Name = "Antiforgery";
        });

        // Register repositories as scoped services
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IPostRepository, PostRepository>();
        builder.Services.AddScoped<ILikeRepository, LikeRepository>();
        builder.Services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();
        builder.Services.AddScoped<IPostPreferenceRepository, PostPreferenceRepository>();

        // Register unit of work implementation and service layer
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork.Implementations.UnitOfWork>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<ILikeService, LikeService>();
        builder.Services.AddScoped<IUserPreferenceService, UserPreferenceService>();

        // Add SignalR services for real-time communication hubs
        builder.Services.AddScoped<ILikeService, LikeService>();

        builder.Services.AddScoped<IUserPreferenceService, UserPreferenceService>();

        builder.Services.AddSignalR();

        // Build the application
        var app = builder.Build();

        // Ensure database migration is applied at the start
        var context = app.Services.CreateScope().ServiceProvider
                                  .GetRequiredService<ApplicationDbContext>();
        
        using (context)
        {
            context.Database.Migrate();
        }

        // Configure the application pipeline for different environments
        if (app.Environment.IsDevelopment())
        {
            // Show developer exception page in development mode
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // Show a generic error page and configure status code pages for production
            app.UseExceptionHandler("/home");
            app.UseStatusCodePagesWithReExecute("/home", "?statusCode={0}");
            app.UseHsts(); // HTTP Strict Transport Security (HSTS)
        }

        // Configure middleware for static files, routing, authentication, and authorization
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession(); // Enable session middleware

        // Map default controller routes
        app.MapDefaultControllerRoute();

        // Map SignalR hubs for chat and notifications
        app.MapHub<ChatHub>("/chathub");
        app.MapHub<NotificationHub>("/notification");

        // Run the application
        app.Run();
    }
}
