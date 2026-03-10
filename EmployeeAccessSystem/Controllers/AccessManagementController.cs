using Microsoft.AspNetCore.Mvc;

namespace EmployeeAccessSystem.Controllers
{
    public class AccessManagementController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
