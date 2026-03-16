using Microsoft.AspNetCore.Mvc;

namespace EmployeeAccessSystem.Controllers
{
    public class EmployeeDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyProfile()
        {
            return View();
        }
        public IActionResult CreateRequest()
        {
            return View();
        }
        public IActionResult MyRequests()
        {
            return View();
        }
    }
}
