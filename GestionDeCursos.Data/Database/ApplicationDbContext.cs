using GestionDeCursos.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionDeCursos.Data.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Applicants> Applicants { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
    }
}
