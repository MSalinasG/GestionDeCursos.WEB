using AutoMapper;
using GestionDeCursos.Data.CustomModel;
using GestionDeCursos.Data.Models;

namespace GestionDeCursos.Web.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<StudentDto, Student>()
                .ForPath(dest => dest.Course.Id, opt => opt.MapFrom(src => src.CourseId))
                .ForPath(dest => dest.Course.CourseName, opt => opt.MapFrom(src => src.CourseName))
                .ForPath(dest => dest.Instructor.Id, opt => opt.MapFrom(src => src.InstructorId))
                .ForPath(dest => dest.Instructor.InstructorName, opt => opt.MapFrom(src => src.InstructorName));

            CreateMap<ApplicantsDTO, Applicants>();
        }
    }
}
