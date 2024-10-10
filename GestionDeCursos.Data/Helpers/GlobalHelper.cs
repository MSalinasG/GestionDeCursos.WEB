namespace GestionDeCursos.Data.Helpers
{
    public static class GlobalHelper
    {
        public static class General
        {
            public const bool UseSeeder = false;
            public const bool UseSps = false;
            public const bool UseDapper = false;

            public const string AppUserIdClaim = "AppUserId";
        }

        public static class CustomClaim
        {
            public const string AppUserIdClaim = "AppUserId";

        }

        public static class Language
        {
            public const string English = "en";
            public const string Spanish = "es";
            public const string ResourcesFolderName = "Resources";
        }

        public static class Role
        {
            public const string Administrator = "Administrator";
            public const string Student = "Student";
            public const string Instructor = "Instructor";
        }
    }
}
