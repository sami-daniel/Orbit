var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//Habilitando página de erros para desenvolvedor
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//Habilitando middleware para lidar com requisicoes por arquivos estaticos
app.UseStaticFiles();

//Habilitando roteamento
app.UseRouting();

//Mapeando endpoints para metodos IAction com rota padrão
app.MapDefaultControllerRoute();

app.Run();
