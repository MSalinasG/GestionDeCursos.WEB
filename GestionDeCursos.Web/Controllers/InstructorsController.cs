using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using GestionDeCursos.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeCursos.Web.Controllers
{
    [TypeFilter(typeof(UserAuthFilter))]
    public class InstructorsController : BaseController
    {
        private readonly IBreadcrumbService _breadcrumbService;
        private string controllerName => nameof(InstructorsController);
        private string instructorLabel => "Instructor";

        public InstructorsController(
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
                        GlobalHelper.Role.Administrator
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Index)}");
                var instructors = await _unitOfWork.InstructorRepository.GetInstructors();
                return View(instructors);

                 
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
                        GlobalHelper.Role.Administrator
               });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.ControllerName = instructorLabel;

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
        public async Task<IActionResult> Create(Instructor instructor)
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (ModelState.IsValid)
                {
                    if(instructor.Experience > 50)
                    {
                        TempData["ErrorMessage"] = "Years of experience must not exceed 50 years";
                    }
                    else
                    {
                        await _unitOfWork.InstructorRepository.Add(instructor);
                        await _unitOfWork.Complete();

                        TempData["ResultMessage"] = "The instructor was created successfully.";
                        return RedirectToAction(nameof(Index));
                    }

                    
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.ControllerName = instructorLabel;

                return View(instructor);
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
                        GlobalHelper.Role.Administrator
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.ControllerName = instructorLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                 
                var instructor = await _unitOfWork.InstructorRepository.Get(id);
                if (instructor == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(instructor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Instructor instructor)
        {
            try
            {
                bool hasAccess = await ValidateUserRole(
                    new string[] {
                        GlobalHelper.Role.Administrator
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (ModelState.IsValid)
                {                  
                    var instructorToEdit = await _unitOfWork.InstructorRepository.Get(id);

                    if (instructorToEdit != null)
                    {
                        if (instructor.Experience > 50)
                        {
                            TempData["ErrorMessage"] = "Years of experience must not exceed 50 years";
                        }
                        else
                        {
                            instructorToEdit.InstructorName = instructor.InstructorName;
                            instructorToEdit.Qualification = instructor.Qualification;
                            instructorToEdit.Experience = instructor.Experience;

                            await _unitOfWork.Complete();
                            TempData["ResultMessage"] = "The instructor was updated successfully.";
                            return RedirectToAction(nameof(Index));
                        }
                        
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.ControllerName = instructorLabel;

                return View(instructor);
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
                        GlobalHelper.Role.Administrator
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Delete)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Delete)}");
                ViewBag.ControllerName = instructorLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var instructor = await _unitOfWork.InstructorRepository.GetInstructorById(id);
                if (instructor == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(instructor);
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
                        GlobalHelper.Role.Administrator
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var instructor = await _unitOfWork.InstructorRepository.Get(id);
                if (instructor == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _unitOfWork.InstructorRepository.Remove(instructor);
                    await _unitOfWork.Complete();
                    TempData["ResultMessage"] = "The instructor was deleted successfully.";
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
                        GlobalHelper.Role.Administrator
                });
                if (!hasAccess)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Details)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Details)}");
                ViewBag.ControllerName = instructorLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var instructor = await _unitOfWork.InstructorRepository.GetInstructorById(id);
                if (instructor == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(instructor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
