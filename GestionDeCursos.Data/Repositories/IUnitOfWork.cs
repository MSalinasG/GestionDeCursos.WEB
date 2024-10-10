using GestionDeCursos.Data.Database;
using GestionDeCursos.Data.Models;

namespace GestionDeCursos.Data.Repositories
{
    public interface IUnitOfWork
    {
        ApplicationDbContext DatabaseContext { get; }
        IStudentRepository StudentRepository { get; }
        ICoursesRepository CourseRepository { get; }
        IInstructorRepository InstructorRepository { get; }
        IApplicantsRepository ApplicantsRepository { get; }
        Task Complete();
    }
}
