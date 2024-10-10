namespace GestionDeCursos.Data.CustomModel
{
    public class ApplicantsDTO
    {        
        public Guid Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Dni { get; set; }
        public DateTime Nacimiento { get; set; }
        public string? FichaPdfMongoFileId { get; set; }
    }
}
