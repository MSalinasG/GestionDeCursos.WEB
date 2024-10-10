using AutoMapper;
using GestionDeCursos.Data.Database;
using GestionDeCursos.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GestionDeCursos.Data.Repositories
{
    public class CoursesRepository : Repository<Course>, ICoursesRepository
    {
        public CoursesRepository(ApplicationDbContext context,
            IMapper mapper, IDbConnection dbDapper) : base(context, mapper, dbDapper)
        { 
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await Context.Courses
                .ToListAsync();
        }

        public async Task<Course?> GetCourseById(int? id)
        {
            return await Context.Courses
                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course?> GetCourseByCourseName(string? name)
        {
            return await Context.Courses
                .FirstOrDefaultAsync(c => c.CourseName == name);
        }
    }
}
