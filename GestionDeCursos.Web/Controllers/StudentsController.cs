using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using GestionDeCursos.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

namespace GestionDeCursos.Web.Controllers
{
    [TypeFilter(typeof(UserAuthFilter))]
    public class StudentsController : BaseController
    {
        private readonly IBreadcrumbService _breadcrumbService;
        private string controllerName => nameof(StudentsController);
        private string studentsLabel => "Students";

        public StudentsController(
            IBreadcrumbService breadcrumbService,
            IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {
            _breadcrumbService = breadcrumbService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator,
                        GlobalHelper.Role.Instructor,
                        GlobalHelper.Role.Student,
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Index)}");
                var students = await _unitOfWork.StudentRepository.GetStudentWithCourse();
                return View(students);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator,
                        GlobalHelper.Role.Instructor
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.ControllerName = studentsLabel;

                var listCourses = await _unitOfWork.CourseRepository.GetAll();
                var listInstructors = await _unitOfWork.InstructorRepository.GetAll();

                ViewData["CourseId"] = new SelectList(listCourses, "Id", "CourseName");
                ViewData["InstructorId"] = new SelectList(listInstructors, "Id", "InstructorName");

                return View();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator,
                        GlobalHelper.Role.Instructor
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (ModelState.IsValid)
                {

                    if (GlobalHelper.General.UseSps)
                    {
                        await _unitOfWork.StudentRepository.InsertStudentSp(student);
                    }
                    else
                    {
                        await _unitOfWork.StudentRepository.Add(student);
                        await _unitOfWork.Complete();
                    }

                   

                    TempData["ResultMessage"] = "The student was created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.ControllerName = studentsLabel;

                var listCourses = await _unitOfWork.CourseRepository.GetAll();
                var listInstructors = await _unitOfWork.InstructorRepository.GetAll();

                ViewData["CourseId"] = new SelectList(listCourses, "Id", "CourseName");
                ViewData["InstructorId"] = new SelectList(listInstructors, "Id", "InstructorName");

                return View(student);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator,
                        GlobalHelper.Role.Instructor,
                        GlobalHelper.Role.Student
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Details)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Details)}");
                ViewBag.ControllerName = studentsLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var student = await _unitOfWork.StudentRepository.GetStudentWithCourseById(id);
                if (student == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(student);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator,
                        GlobalHelper.Role.Instructor
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.ControllerName = studentsLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var student = await _unitOfWork.StudentRepository.Get(id);
                if (student == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var listCourses = await _unitOfWork.CourseRepository.GetAll();
                var listInstructors = await _unitOfWork.InstructorRepository.GetAll();

                ViewData["CourseId"] = new SelectList(listCourses, "Id", "CourseName");
                ViewData["InstructorId"] = new SelectList(listInstructors, "Id", "InstructorName");

                return View(student);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator,
                        GlobalHelper.Role.Instructor
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (ModelState.IsValid)
                {
                    var studentToEdit = await _unitOfWork.StudentRepository.Get(id);
                    if (studentToEdit != null)
                    {

                        if (GlobalHelper.General.UseSps)
                        {
                            await _unitOfWork.StudentRepository.UpdateStudentSp(student);
                        }
                        else
                        {
                            studentToEdit.StudentName = student.StudentName;
                            studentToEdit.CourseId = student.CourseId;
                            studentToEdit.CourseFee = student.CourseFee;
                            studentToEdit.CourseDuration = student.CourseDuration;
                            studentToEdit.CourseStartDate = student.CourseStartDate;
                            studentToEdit.BatchTime = student.BatchTime;
                            studentToEdit.InstructorId = student.InstructorId;

                            await _unitOfWork.Complete();
                        }
                        

                           
                        TempData["ResultMessage"] = "The student was updated successfully.";
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.ControllerName = studentsLabel;

                var listCourses = await _unitOfWork.CourseRepository.GetAll();
                var listInstructors = await _unitOfWork.InstructorRepository.GetAll();

                ViewData["CourseId"] = new SelectList(listCourses, "Id", "CourseName");
                ViewData["InstructorId"] = new SelectList(listInstructors, "Id", "InstructorName");

                return View(student);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator,
                        GlobalHelper.Role.Instructor
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Delete)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Delete)}");
                ViewBag.ControllerName = studentsLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var student = await _unitOfWork.StudentRepository.GetStudentWithCourseById(id);
                if (student == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(student);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator,
                        GlobalHelper.Role.Instructor
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var student = await _unitOfWork.StudentRepository.Get(id);
                if (student == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (GlobalHelper.General.UseSps)
                    {
                        await _unitOfWork.StudentRepository.DeleteStudentSp(id);
                    }
                    else
                    {
                        _unitOfWork.StudentRepository.Remove(student);
                        await _unitOfWork.Complete();
                    }
                    
                    TempData["ResultMessage"] = "The student was deleted successfully.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadStudentsExcel()
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator,
                        GlobalHelper.Role.Instructor
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }
               
                var students = (await _unitOfWork.StudentRepository.GetStudentWithCourse()).ToList();

                using (var package = new ExcelPackage())
                {
                    var worksheets = package.Workbook.Worksheets.Add("Students");

                    //Agregar cabeceras
                    worksheets.Cells[1, 1].Value = "Student Name";
                    worksheets.Cells[1, 2].Value = "Course Name";
                    worksheets.Cells[1, 3].Value = "Course Fee";
                    worksheets.Cells[1, 4].Value = "Course Duration";
                    worksheets.Cells[1, 5].Value = "Course Start Date";
                    worksheets.Cells[1, 6].Value = "Course Time";
                    worksheets.Cells[1, 7].Value = "Instructor Name";

                    //Agregar datos
                    for (int i = 0; i < students.Count(); i++)
                    {
                        var objStudent = students[i];
                        int index = i + 2;

                        worksheets.Cells[index, 1].Value = objStudent.StudentName;
                        worksheets.Cells[index, 2].Value = objStudent.Course?.CourseName;
                        worksheets.Cells[index, 3].Value = objStudent.CourseFee;
                        worksheets.Cells[index, 4].Value = objStudent.CourseDuration;
                        worksheets.Cells[index, 5].Value = objStudent.CourseStartDate.ToString("dd-MM-yyyy");
                        worksheets.Cells[index, 6].Value = objStudent.BatchTime.ToString("hh\\:mm\\ tt");
                        worksheets.Cells[index, 7].Value = objStudent.Instructor?.InstructorName;
                    }

                    string timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss");
                    var stream = new MemoryStream(package.GetAsByteArray());

                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"{timestamp}_StudentsList.xlsx");

                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
