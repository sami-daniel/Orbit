using Microsoft.AspNetCore.Authentication.Cookies;
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

            //Registrando controllers e views como serviços
            _ = builder.Services.AddControllersWithViews();

            //Adicionando contexto de banco de dados como serviço
            _ = builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseMySql(connectionString: builder.Configuration.GetConnectionString("OrbitConnection"), 
             serverVersion: new MySqlServerVersion(new Version(8, 4, 0))));

            //Registrando Cokkie de autenticação e configurando-o
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.LoginPath = "/Account/Login";
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                });


            //Registrando UserRepository como servico
            _ = builder.Services.AddScoped<IUserRepository, UserRepository>();

            //Registrando UnitOfWork como servico
            _ = builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IUserService, UserService>();

            WebApplication app = builder.Build();

            //Habilitando página de erros para desenvolvedor
            if (app.Environment.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
            }

            //Habilitando middleware para lidar com requisicoes por arquivos estaticos
            _ = app.UseStaticFiles();

            //Habilitando roteamento
            _ = app.UseRouting();

            //Habilitando autenticacao
            _ = app.UseAuthentication();

            //Habilitando autorização
            _ = app.UseAuthorization();

            //Mapeando endpoints para metodos IAction com rota padrão
            _ = app.MapDefaultControllerRoute();

            app.Run();

        }
    }
}