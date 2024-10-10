namespace GestionDeCursos.Web.Models
{
    public class JsonCustomResponse
    {
        public string? Status { get; set; }
        public string? Message {  get; set; }
        public object? Data { get; set; }
    }
}
