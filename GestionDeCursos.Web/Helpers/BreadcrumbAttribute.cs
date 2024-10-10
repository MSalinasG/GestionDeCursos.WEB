namespace GestionDeCursos.Web.Helpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BreadcrumbAttribute : Attribute
    {
        public string Title { get; set; }

        public BreadcrumbAttribute(string title)
        {
            Title = title;
        }
    }
}
