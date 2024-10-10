using GestionDeCursos.Data.Models;

namespace GestionDeCursos.Data.Repositories
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<IEnumerable<Instructor>> GetInstructors();

        Task<Instructor?> GetInstructorById(int? id);
    }
}
