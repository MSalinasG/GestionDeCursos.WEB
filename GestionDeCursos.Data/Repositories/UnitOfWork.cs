using AutoMapper;
using GestionDeCursos.Data.Database;
using System.Data;

namespace GestionDeCursos.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly IDbConnection _dbDapper;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private readonly ICoursesRepository _courseRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IApplicantsRepository _applicantsRepository;

        public UnitOfWork(ApplicationDbContext context, IMapper mapper, IDbConnection dbDapper)
        {
            _db = context;
            _dbDapper = dbDapper;
            _mapper = mapper;
            _studentRepository = new StudentRepository(_db, _mapper, _dbDapper);
            _courseRepository = new CoursesRepository(_db, _mapper, _dbDapper);
            _instructorRepository = new InstructorRepository(_db, _mapper, _dbDapper);
            _applicantsRepository = new ApplicantsRepository(_db, _mapper, _dbDapper);
        }

        public IStudentRepository StudentRepository => _studentRepository;

        public ICoursesRepository CourseRepository => _courseRepository;

        public IInstructorRepository InstructorRepository => _instructorRepository;

        public IApplicantsRepository ApplicantsRepository => _applicantsRepository;

        public ApplicationDbContext DatabaseContext => _db;

        public async Task Complete()
        {
            await _db.SaveChangesAsync();
        }
    }
}
