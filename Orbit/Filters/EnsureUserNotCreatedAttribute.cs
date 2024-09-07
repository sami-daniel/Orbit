using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Orbit.Filters;

public class EnsureUserNotCreatedAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity!.IsAuthenticated && !context.ActionDescriptor.DisplayName!.Contains("LogOut"))
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
