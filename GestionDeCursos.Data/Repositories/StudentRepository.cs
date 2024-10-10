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
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(ApplicationDbContext context, 
            IMapper mapper, IDbConnection dbDapper) : base(context, mapper, dbDapper)
        {
        }

        public async Task<IEnumerable<Student>> GetStudentWithCourse()
        {
            string getStudentsSp = "[Management].[uspGetStudents]";

            if (GlobalHelper.General.UseDapper)
            {
               var studentsDapper = await DbDapper.QueryAsync<StudentDto>
                    (getStudentsSp, null, commandType: CommandType.StoredProcedure);

                var convertedStudents = Mapper.Map<IEnumerable<Student>>(studentsDapper);
                return convertedStudents;
            }

            if (GlobalHelper.General.UseSps)
            {
                var studentsSp = await ExecuteStoredProcedureAsync<StudentDto>(cmd => {                   
                    cmd.CommandText = getStudentsSp;
                });

                var convertedStudents = Mapper.Map<IEnumerable<Student>>(studentsSp);
                return convertedStudents;
            }

            return await Context.Students
                .Include(x => x.Course)
                .Include(x => x.Instructor)
                .OrderBy(x => x.StudentName)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentWithCourseById(int? id)
        {
            string getStudentsSp = "[Management].[uspGetStudents]";

            if (GlobalHelper.General.UseDapper)
            {
                var studentDapper = (await DbDapper.QueryAsync<StudentDto>
                     (getStudentsSp, new { pId = id }, commandType: CommandType.StoredProcedure))
                     .FirstOrDefault();                     

                var convertedStudent = Mapper.Map<Student>(studentDapper);
                return convertedStudent;
            }


            if (GlobalHelper.General.UseSps)
            {
                var studentsSp = await ExecuteStoredProcedureAsync<StudentDto>(cmd => {
                    SetParameterCommand(cmd, "pId", id);
                    cmd.CommandText = getStudentsSp;
                });

                var convertedStudent = Mapper.Map<Student>(studentsSp.FirstOrDefault());
                return convertedStudent;
            }

            return await Context.Students
                .Include(x => x.Course)
                .Include(x => x.Instructor)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> InsertStudentEf(Student student)
        {
            await Context.Students.AddAsync(student);
            await Context.SaveChangesAsync();

            return student.Id;
        }


        public async Task InsertStudentSp(Student student)
        {
            var insertStudentsSp = "[Management].[uspInsertStudent]";
            if (GlobalHelper.General.UseDapper)
            {
                var parameters = new {
                    pName = student.StudentName,
                    pCourseId = student.CourseId,
                    pCourseFee = student.CourseFee,
                    pCourseDuration = student.CourseDuration,
                    pCourseStartDate = student.CourseStartDate,
                    pBatchTime = student.BatchTime,
                    pInstructorId = student.InstructorId
                };

                await DbDapper.ExecuteAsync(insertStudentsSp, parameters, commandType: CommandType.StoredProcedure);

            }
            else
            {
                await ExecuteStoredProcedureNoResultsAsync(cmd => {
                    SetParameterCommand(cmd, "pName", student.StudentName);
                    SetParameterCommand(cmd, "pCourseId", student.CourseId);
                    SetParameterCommand(cmd, "pCourseFee", student.CourseFee);
                    SetParameterCommand(cmd, "pCourseDuration", student.CourseDuration);
                    SetParameterCommand(cmd, "pCourseStartDate", student.CourseStartDate);
                    SetParameterCommand(cmd, "pBatchTime", student.BatchTime);
                    SetParameterCommand(cmd, "pInstructorId", student.InstructorId);

                    cmd.CommandText = insertStudentsSp;
                });
            }
               
        }

        public async Task UpdateStudentSp(Student student)
        {
            var updateStudentsSp = "[Management].[uspUpdateStudent]";
            if (GlobalHelper.General.UseDapper)
            {
                var parameters = new
                {
                    pId = student.Id,
                    pName = student.StudentName,
                    pCourseId = student.CourseId,
                    pCourseFee = student.CourseFee,
                    pCourseDuration = student.CourseDuration,
                    pCourseStartDate = student.CourseStartDate,
                    pBatchTime = student.BatchTime,
                    pInstructorId = student.InstructorId
                };

                await DbDapper.ExecuteAsync(updateStudentsSp, parameters, commandType: CommandType.StoredProcedure);

            }
            else
            {
                await ExecuteStoredProcedureNoResultsAsync(cmd => {
                    SetParameterCommand(cmd, "pId", student.Id);
                    SetParameterCommand(cmd, "pName", student.StudentName);
                    SetParameterCommand(cmd, "pCourseId", student.CourseId);
                    SetParameterCommand(cmd, "pCourseFee", student.CourseFee);
                    SetParameterCommand(cmd, "pCourseDuration", student.CourseDuration);
                    SetParameterCommand(cmd, "pCourseStartDate", student.CourseStartDate);
                    SetParameterCommand(cmd, "pBatchTime", student.BatchTime);
                    SetParameterCommand(cmd, "pInstructorId", student.InstructorId);

                    cmd.CommandText = updateStudentsSp;
                });
            }
                
        }

        public async Task DeleteStudentSp(int? id)
        {
            var deleteStudentsSp = "[Management].[uspDeleteStudent]";
            if (GlobalHelper.General.UseDapper)
            {
                var parameters = new
                {
                    pId = id
                   
                };

                await DbDapper.ExecuteAsync(deleteStudentsSp, parameters, commandType: CommandType.StoredProcedure);

            }
            else
            {
                await ExecuteStoredProcedureNoResultsAsync(cmd => {
                    SetParameterCommand(cmd, "pId", id);

                    cmd.CommandText = "[Management].[uspDeleteStudent]";
                });
            }
               
        }

        
    }
}
