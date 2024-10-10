using GestionDeCursos.Data.Helpers;
using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using GestionDeCursos.Data.Services;
using GestionDeCursos.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MongoDB.Bson;
using OfficeOpenXml;

namespace GestionDeCursos.Web.Controllers
{
    [TypeFilter(typeof(UserAuthFilter))]
    public class ApplicantsController : BaseController
    {
        private readonly IPdfFileServices _pdfFileServices;
        private readonly IBreadcrumbService _breadcrumbService;
        private string controllerName => nameof(ApplicantsController);
        private string applicantsLabel => "Applicants";

        public ApplicantsController(
            IBreadcrumbService breadcrumbService,
            IPdfFileServices pdfFileServices,
            IUnitOfWork _unitOfWork) : base(_unitOfWork)
        {
            _breadcrumbService = breadcrumbService;
            _pdfFileServices = pdfFileServices;
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
                var applicants = await _unitOfWork.ApplicantsRepository.GetApplicants();
                return View(applicants);
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
                ViewBag.ControllerName = applicantsLabel;

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
        public async Task<IActionResult> Create(Applicants applicants)
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
                    //Valida dni repetido

                    var dniRepetido = await _unitOfWork.ApplicantsRepository.GetApplicantsByDni(applicants.Dni);

                    if (dniRepetido != null)
                    {
                        TempData["ErrorMessage"] = "Dni already exists";
                    }
                    else
                    {
                        string? mongoFileId = null;
                        if (applicants.PdfFileData != null && applicants.PdfFileData.Length > 0)
                        {
                            mongoFileId = ObjectId.GenerateNewId().ToString();
                        }
                        applicants.FichaPdfMongoFileId = mongoFileId;

                        // Crear un nuevo GUID para el ID
                        Guid newGuid = Guid.NewGuid();
                        applicants.Id = newGuid;

                        await _unitOfWork.ApplicantsRepository.Add(applicants);
                        await _unitOfWork.Complete();

                        if (!string.IsNullOrWhiteSpace(mongoFileId))
                        {
                            await CreateFileCourse(applicants.PdfFileData, mongoFileId);
                        }

                        TempData["ResultMessage"] = "The applicants was created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                     
                        
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Create)}");
                ViewBag.ControllerName = applicantsLabel;

                return View(applicants);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Details(Guid? id)
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
                ViewBag.ControllerName = applicantsLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var applicants = await _unitOfWork.ApplicantsRepository.GetApplicantsById(id);
                if (applicants == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(applicants);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Edit(Guid? id)
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
                ViewBag.ControllerName = applicantsLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var applicants = await _unitOfWork.ApplicantsRepository.Get(id);
                if (applicants == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(applicants);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Applicants applicants)
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
                    var applicantsToEdit = await _unitOfWork.ApplicantsRepository.Get(id);
                    if (applicantsToEdit != null)
                    {
                        string? mongoFileId = null;
                        string? mongoFileIdToDelete = null;

                        if (applicants.OverwriteFile)
                        {
                            if (applicants.PdfFileData != null && applicants.PdfFileData.Length > 0)
                            {
                                mongoFileId = ObjectId.GenerateNewId().ToString();
                            }

                            mongoFileIdToDelete = applicantsToEdit.FichaPdfMongoFileId;
                            applicantsToEdit.FichaPdfMongoFileId = mongoFileId;

                        }

                        applicantsToEdit.Nombre = applicants.Nombre;
                        applicantsToEdit.Apellido = applicants.Apellido;
                        applicantsToEdit.Dni = applicants.Dni;
                        applicantsToEdit.Nacimiento = applicants.Nacimiento;
                        await _unitOfWork.Complete();

                        if (applicants.OverwriteFile)
                        {
                            if (!string.IsNullOrWhiteSpace(mongoFileIdToDelete))
                            {
                                await _pdfFileServices.Delete(mongoFileIdToDelete);
                            }

                            if (applicants.PdfFileData != null && applicants.PdfFileData.Length > 0)
                            {
                                await CreateFileCourse(applicants.PdfFileData, mongoFileId);
                            }
                        }

                        TempData["ResultMessage"] = "The applicants was updated successfully.";
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    return RedirectToAction(nameof(Index));
                }

                ViewBag.PrimaryBreadcrumb = _breadcrumbService.GetPrimaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.SecondaryBreadcrumb = _breadcrumbService.GetSecondaryBreadcrumb($"{controllerName}{nameof(Edit)}");
                ViewBag.ControllerName = applicantsLabel;

                return View(applicants);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Delete(Guid? id)
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
                ViewBag.ControllerName = applicantsLabel;

                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var applicants = await _unitOfWork.ApplicantsRepository.GetApplicantsById(id);
                if (applicants == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(applicants);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
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


                var applicants = await _unitOfWork.ApplicantsRepository.Get(id);
                if (applicants == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {                    
                    _unitOfWork.ApplicantsRepository.Remove(applicants);
                    await _unitOfWork.Complete();

                    if (!string.IsNullOrWhiteSpace(applicants.FichaPdfMongoFileId))
                    {
                        await _pdfFileServices.Delete(applicants.FichaPdfMongoFileId);
                    }

                    TempData["ResultMessage"] = "The applicants was deleted successfully.";
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
        public async Task<IActionResult> DownloadPDF(Guid id)
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

                var applicants = await _unitOfWork.ApplicantsRepository.Get(id);

                if (applicants == null || string.IsNullOrWhiteSpace(applicants.FichaPdfMongoFileId))
                {
                    return RedirectToAction("Index", "Home");
                }

                var file = await _pdfFileServices.GetById(applicants.FichaPdfMongoFileId);

                if (file == null || file.PdfFileData == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                string timestamp = DateTime.Now.ToString("ddMMyyyyHHmmss");
                return File(file.PdfFileData, "application/pdf",
                    $"{timestamp}_{file.FileName}.pdf");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error has occurred. Your request cannot be completed.";
                return RedirectToAction("Index", "Home");
            }
        }

        private async Task CreateFileCourse(IFormFile pdfFileData, string mongoFileId)
        {
            using (var stream = new MemoryStream())
            {
                await pdfFileData.CopyToAsync(stream);
                var fileBytes = stream.ToArray();

                await _pdfFileServices.Create(new Data.MongoModels.PdfFile
                {
                    Id = ObjectId.Parse(mongoFileId),
                    FileName = pdfFileData.FileName,
                    PdfFileData = fileBytes
                });
            }
        }
    }
}
