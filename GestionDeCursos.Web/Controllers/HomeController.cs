using GestionDeCursos.Data.Models;
using GestionDeCursos.Data.Repositories;
using GestionDeCursos.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GestionDeCursos.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> _logger;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public HomeController(ILogger<HomeController> logger,
            IUnitOfWork unitOfWork,
            IPasswordHasher<AppUser> passwordHasher)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetLoginModal()
        {
            var viewModel = new LoginViewModel();

            viewModel.Username = null;
            viewModel.Password = null;

            return PartialView("_LoginPartial", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> VerifyLogin(LoginViewModel viewModel)
        {
            var response = new JsonCustomResponse();

            try
            {
                if (!ModelState.IsValid)
                {
                    response.Status = "error";
                    response.Message = "The form is invalid.";
                    return Json(response);
                }

                var objUser = await _unitOfWork.DatabaseContext.Users
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.Username == viewModel.Username);

                if (objUser != null)
                {
                    var passwordResult =
                        _passwordHasher.VerifyHashedPassword
                        (objUser, objUser.Password, viewModel.Password);

                    if (passwordResult == PasswordVerificationResult.Success)
                    {
                        if (!objUser.IsActive)
                        {
                            response.Status = "error";
                            response.Message = "Your user is inactive.";
                            return Json(response);
                        }

                        HttpContext.Session.SetString("UserId", objUser.Id.ToString());
                        HttpContext.Session.SetString("Role", objUser.Role.Name);
                        HttpContext.Session.SetString("Username", objUser.Username);

                        response.Status = "success";
                        return Json(response);
                    }
                }

                response.Status = "error";
                response.Message = "The username or password is incorrect.";
                return Json(response);
            }
            catch (Exception ex)
            {
                response.Status = "error";
                response.Message = "An error has occurred. Try again later.";
                return Json(response);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
