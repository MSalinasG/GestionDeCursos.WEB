namespace GestionDeCursos.Web.Helpers
{
    public static class HttpExtensionHelper
    {
        public static string? GetUserId(this ISession session)
        {
            return session.GetString("UserId");
        }

        public static string? GetUsername(this ISession session)
        {
            return session.GetString("Username");
        }

        public static string? GetRole(this ISession session)
        {
            return session.GetString("Role");
        }

        public static bool HasRole(this ISession session, string role)
        {
            return session.GetRole() == role;
        }
    }
}
