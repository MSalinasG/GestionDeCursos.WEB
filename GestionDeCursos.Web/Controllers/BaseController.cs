using GestionDeCursos.Data.Repositories;
using GestionDeCursos.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionDeCursos.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async Task<bool> ValidateUserRole(string[] roles)
        {
            string? userId = HttpContext.Session.GetUserId();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                var objUser = await _unitOfWork.DatabaseContext.Users
                    .Include(x => x.Role)
                    .FirstOrDefaultAsync(x => x.Id == new Guid(userId));

                if (objUser != null && roles.Contains(objUser.Role?.Name))
                {
                    return true;
                }
            }

            TempData["ErrorMessage"] = "You don't have access to this page.";
            return false;
        }
    }
}
