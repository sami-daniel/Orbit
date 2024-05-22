namespace Orbit
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            //Registrando controllers e views como serviços
            _ = builder.Services.AddControllersWithViews();

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

            //Mapeando endpoints para metodos IAction com rota padrão
            _ = app.MapDefaultControllerRoute();

            app.Run();

        }
    }
}