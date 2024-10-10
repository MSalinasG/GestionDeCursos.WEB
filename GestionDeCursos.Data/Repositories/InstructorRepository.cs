using AutoMapper;
using GestionDeCursos.Data.Database;
using GestionDeCursos.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeCursos.Data.Repositories
{
    public class InstructorRepository : Repository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(ApplicationDbContext context,
            IMapper mapper, IDbConnection dbDapper) : base(context, mapper, dbDapper)
        {
        }

        public async Task<Instructor?> GetInstructorById(int? id)
        {
            return await Context.Instructors
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Instructor>> GetInstructors()
        {
            return await Context.Instructors
                .ToListAsync();
        }
    }
}
