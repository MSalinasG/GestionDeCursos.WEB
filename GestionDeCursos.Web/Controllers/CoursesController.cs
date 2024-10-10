using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using GestionDeCursos.Data.Services;
using GestionDeCursos.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace GestionDeCursos.Web.Controllers
{
    [TypeFilter(typeof(UserAuthFilter))]
    public class CoursesController : BaseController
    {
        private readonly IExcelFileServices _excelFileServices;
        private readonly IBreadcrumbService _breadcrumbService;
        private string controllerName => nameof(CoursesController);
        private string courseLabel => "Course";

        public CoursesController(
            IBreadcrumbService breadcrumbService,
            IExcelFileServices excelFileServices,
            IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {
            this._breadcrumbService = breadcrumbService;
            this._excelFileServices = excelFileServices;
        }

        public async Task<IActionResult> Index()
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
                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Index)}");
                var courses = await _unitOfWork.CourseRepository.GetCourses();
                return View(courses);
            }
            catch (Exception)
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
                ViewBag.ControllerName = courseLabel;

                return View();

            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
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

                    var courseRepetid = await _unitOfWork.CourseRepository.GetCourseByCourseName(course.CourseName);

                    if (courseRepetid != null)
                    {
                        TempData["ErrorMessage"] = "Course already exists";
                    }
                    else
                    {
                        string? mongoFileId = null;
                        if (course.ExcelFileData != null && course.ExcelFileData.Length > 0)
                        {
                            mongoFileId = ObjectId.GenerateNewId().ToString();
                        }
                        course.MongoFileId = mongoFileId;
                        await _unitOfWork.CourseRepository.Add(course);
                        await _unitOfWork.Complete();

                        if (!string.IsNullOrWhiteSpace(mongoFileId))
                        {
                            await CreateFileCourse(course.ExcelFileData, mongoFileId);
                        }

                        TempData["ResultMessage"] = "The course was created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                    
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.ControllerName = courseLabel;                 

                return View(course);
            }
            catch (Exception)
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
                ViewBag.ControllerName = courseLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                                 
                var course = await _unitOfWork.CourseRepository.Get(id);
                if (course == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (string.IsNullOrWhiteSpace(course.MongoFileId))
                {
                    course.OverwriteFile = true;
                }


                return View(course);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
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
                    var courseToEdit = await _unitOfWork.CourseRepository.Get(id);

                    if (courseToEdit != null)
                    {

                        string? mongoFileId = null;
                        string? mongoFileIdToDelete = null;

                        if (course.OverwriteFile)
                        {
                            if (course.ExcelFileData != null && course.ExcelFileData.Length > 0)
                            {
                                mongoFileId = ObjectId.GenerateNewId().ToString();
                            }

                            mongoFileIdToDelete = courseToEdit.MongoFileId;
                            courseToEdit.MongoFileId = mongoFileId;
                            
                        }

                        courseToEdit.CourseName = course.CourseName;
                        courseToEdit.CourseDescription = course.CourseDescription;
                        await _unitOfWork.Complete();

                        if (course.OverwriteFile)
                        {
                            if (!string.IsNullOrWhiteSpace(mongoFileIdToDelete))
                            {
                                await _excelFileServices.Delete(mongoFileIdToDelete);
                            }

                            if (course.ExcelFileData != null && course.ExcelFileData.Length > 0)
                            {
                                await CreateFileCourse(course.ExcelFileData, mongoFileId);                                
                            }
                        }

                        TempData["ResultMessage"] = "The course was updated successfully.";
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.ControllerName = courseLabel;

                return View(course);
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
                ViewBag.ControllerName = courseLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                } 
                
                var course = await _unitOfWork.CourseRepository.GetCourseById(id);
                if(course == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(course);
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

                var course = await _unitOfWork.CourseRepository.Get(id);
                if (course == null) 
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                { 
                    _unitOfWork.CourseRepository.Remove(course);
                    await _unitOfWork.Complete();

                    if (!string.IsNullOrWhiteSpace(course.MongoFileId))
                    {
                        await _excelFileServices.Delete(course.MongoFileId);
                    }

                    TempData["ResultMessage"] = "The course was deleted successfully.";
                }

                return RedirectToAction(nameof(Index));
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
                        GlobalHelper.Role.Instructor
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Details)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Details)}");
                ViewBag.ControllerName = courseLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                
                var course = await _unitOfWork.CourseRepository.GetCourseById(id);
                if (course == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(course);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadExcel(int id)
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

                var course = await _unitOfWork.CourseRepository.Get(id);
                if (course == null || string.IsNullOrWhiteSpace(course.MongoFileId))
                {
                    return RedirectToAction("Index", "Home");
                }

                var file = await _excelFileServices.GetById(course.MongoFileId);

                if (file == null || file.ExcelFileData == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                string timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss");
                return File(file.ExcelFileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{timestamp}_{file.FileName}.xlsx");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task CreateFileCourse(IFormFile excelFileData, string mongoFileId)
        {
            using (var stream = new MemoryStream())
            {
                await excelFileData.CopyToAsync(stream);
                var fileBytes = stream.ToArray();

                await _excelFileServices.Create(new Data.MongoModels.ExcelFile
                {
                    Id = ObjectId.Parse(mongoFileId),
                    FileName = excelFileData.FileName,
                    ExcelFileData = fileBytes
                });
            }
        }
    }
}
