using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GestionDeCursos.Web.Helpers
{
    public class UserAuthFilter : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public UserAuthFilter() { }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (string.IsNullOrWhiteSpace(context.HttpContext.Session.GetUserId()))
            {
                context.Result = new RedirectToRouteResult
                    (new RouteValueDictionary(new { controller = "Home", action = "Index" }));
                return;
            }
        }
    }
}
