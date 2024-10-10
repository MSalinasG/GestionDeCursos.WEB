namespace GestionDeCursos.Web.Helpers
{
    public class BreadcrumbService : IBreadcrumbService
    {
        private readonly Dictionary<string, string> _primaryBreadcrumb = new Dictionary<string, string>
        {
            { "StudentsControllerIndex", "Students List" },
            { "StudentsControllerDetails", "Student Details" },
            { "StudentsControllerCreate", "Create Student" },
            { "StudentsControllerEdit", "Edit Student" },
            { "StudentsControllerDelete", "Delete Student" },

            { "CoursesControllerIndex", "Courses List" },
            { "CoursesControllerDetails", "Course Details" },
            { "CoursesControllerCreate", "Create Course" },
            { "CoursesControllerEdit", "Edit Course" },
            { "CoursesControllerDelete", "Delete Course" },

            { "InstructorsControllerIndex", "Instructors List" },
            { "InstructorsControllerDetails", "Instructor Details" },
            { "InstructorsControllerCreate", "Create Instructor" },
            { "InstructorsControllerEdit", "Edit Instructor" },
            { "InstructorsControllerDelete", "Delete Instructor" },

            { "ApplicantsControllerIndex", "Applicants List" },
            { "ApplicantsControllerDetails", "Applicant Details" },
            { "ApplicantsControllerCreate", "Create Applicant" },
            { "ApplicantsControllerEdit", "Edit Applicant" },
            { "ApplicantsControllerDelete", "Delete Applicant" },


        };

        private readonly Dictionary<string, string> _secondaryBreadcrumb = new Dictionary<string, string>
        {
            { "StudentsControllerCreate", "Students List" },
            { "StudentsControllerEdit", "Students List" },
            { "StudentsControllerDelete", "Students List" },
            { "StudentsControllerDetails", "Students List" },

            { "CoursesControllerCreate", "Courses List" },
            { "CoursesControllerEdit", "Courses List" },
            { "CoursesControllerDelete", "Courses List" },
            { "CoursesControllerDetails", "Courses List" },

            { "InstructorsControllerCreate", "Instructors List" },
            { "InstructorsControllerEdit", "Instructors List" },
            { "InstructorsControllerDelete", "Instructors List" },
            { "InstructorsControllerDetails", "Instructors List" },

            { "ApplicantsControllerCreate", "Applicants List" },
            { "ApplicantsControllerEdit", "Applicants List" },
            { "ApplicantsControllerDelete", "Applicants List" },
            { "ApplicantsControllerDetails", "Applicants List" },
        };

        public string? GetPrimaryBreadcrumb(string actionName)
        {
            return _primaryBreadcrumb.ContainsKey(actionName) ? _primaryBreadcrumb[actionName] : "Home";
        }

        public string? GetSecondaryBreadcrumb(string actionName)
        {
            return _secondaryBreadcrumb.ContainsKey(actionName) ? _secondaryBreadcrumb[actionName] : null;
        }
    }
}
