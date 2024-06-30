using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Orbit.Filters
{
    /// <summary>
    /// Filter para o controlador Account, para protege-lo do usuario tentar logar quando já está logado.
    /// </summary>
    public class EnsureProfileNotCreatedAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.User.Identity!.IsAuthenticated) 
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Profile",
                    action = "Index"
                }));
                return;
            }

            await next();
        }
    }
}
