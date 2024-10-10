using GestionDeCursos.Data.Models;

namespace GestionDeCursos.Data.Repositories
{
    public interface IApplicantsRepository : IRepository<Applicants>
    {
        Task<IEnumerable<Applicants>> GetApplicants();
        Task<Applicants?> GetApplicantsById(Guid? id);        
        Task<Applicants?> GetApplicantsByDni(string? dni);

    }
}
