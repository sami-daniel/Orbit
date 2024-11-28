using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Orbit.Filters;

/// <summary>
/// Custom action filter to ensure that authenticated users cannot access routes where they are not supposed to be,
/// redirecting them to the "panel" controller's "index" action unless the action is "LogOut".
/// </summary>
public class EnsureUserNotCreatedAttribute : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// Executes before the action method runs, ensuring that authenticated users are redirected if they try to access
    /// routes where they are not allowed (except "LogOut").
    /// </summary>
    /// <param name="context">The context for the action being executed.</param>
    /// <param name="next">The delegate to execute the next action filter in the pipeline.</param>
    /// <returns>A task representing the asynchronous operation of the action execution.</returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Check if the user is authenticated and the action is not "LogOut"
        if (context.HttpContext.User.Identity!.IsAuthenticated && !context.ActionDescriptor.DisplayName!.Contains("LogOut"))
        {
            // Redirect the authenticated user to the "panel" controller's "index" action
            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = "panel", // Controller to redirect to
                action = "index"     // Action to redirect to
            }));
            return; // Stop further execution of the action and apply the redirect
        }

        // Continue with the next action filter or the action itself if the user is not authenticated or is allowed
        await next();
    }
}
