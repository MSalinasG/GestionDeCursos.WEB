using GestionDeCursos.Data.Models;

namespace GestionDeCursos.Data.Repositories
{
    public interface ICoursesRepository : IRepository<Course>
    {
        Task<IEnumerable<Course>> GetCourses();
        Task<Course?> GetCourseById(int? id);
        Task<Course?> GetCourseByCourseName(string? name);
    }
}
