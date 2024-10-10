using GestionDeCursos.Data.Models;

namespace GestionDeCursos.Data.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<IEnumerable<Student>> GetStudentWithCourse();
        Task<Student?> GetStudentWithCourseById(int? id);


        Task<int> InsertStudentEf(Student student); //Entity Framework
        Task InsertStudentSp(Student student);
        Task UpdateStudentSp(Student student);
        Task DeleteStudentSp(int? id);

    }
}
