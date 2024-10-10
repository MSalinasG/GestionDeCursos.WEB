using AutoMapper;
using Dapper;
using GestionDeCursos.Data.CustomModel;
using GestionDeCursos.Data.Database;
using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace GestionDeCursos.Data.Repositories
{
    public class ApplicantsRepository : Repository<Applicants>, IApplicantsRepository
    {
        public ApplicantsRepository(ApplicationDbContext context,
            IMapper mapper, IDbConnection dbDapper) : base(context, mapper, dbDapper)
        {
        }

        public async Task<IEnumerable<Applicants>> GetApplicants()
        {
            string getApplicantsSp = "[Enrollment].[uspGetApplicants]";

            var applicantsDapper = await DbDapper.QueryAsync<ApplicantsDTO>
                 (getApplicantsSp, null, commandType: CommandType.StoredProcedure);

            var convertedApplicants = Mapper.Map<IEnumerable<Applicants>>(applicantsDapper);
            return convertedApplicants;
        }

        public async Task<Applicants?> GetApplicantsByDni(string? dni)
        {
            return await Context.Applicants
               .FirstOrDefaultAsync(c => c.Dni == dni);
        }

        public async Task<Applicants?> GetApplicantsById(Guid? id)
        {
            return await Context.Applicants
                      .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}

