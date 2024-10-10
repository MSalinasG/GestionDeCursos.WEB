using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionDeCursos.Web.Helpers
{
    public static class HtmlExtensionHelper
    {
        public static string? GetUserId(this IHtmlHelper htmlHelper)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;
            return httpContext.Session.GetUserId();
        }

        public static string? GetUsername(this IHtmlHelper htmlHelper)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;
            return httpContext.Session.GetUsername();
        }

        public static string? GetRole(this IHtmlHelper htmlHelper)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;
            return httpContext.Session.GetRole();
        }

        public static bool HasRole(this IHtmlHelper htmlHelper, string role)
        {
            var httpContext = htmlHelper.ViewContext.HttpContext;
            return httpContext.Session.HasRole(role);
        }
    }
}
