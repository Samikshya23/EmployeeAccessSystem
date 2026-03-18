using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeAccessSystem.Controllers
{
    public class EmployeeDashboardController : Controller
    {
        private readonly IAccessRequestRepositories _accessRequestRepositories;
        private readonly ICategoryRepositories _categoryRepositories;
        private readonly ISubCategoryRepositories _subCategoryRepositories;
        private readonly IEmployeeRepositories _employeeRepositories;

        public EmployeeDashboardController(
            IAccessRequestRepositories accessRequestRepositories,
            ICategoryRepositories categoryRepositories,
            ISubCategoryRepositories subCategoryRepositories,
            IEmployeeRepositories employeeRepositories)
        {
            _accessRequestRepositories = accessRequestRepositories;
            _categoryRepositories = categoryRepositories;
            _subCategoryRepositories = subCategoryRepositories;
            _employeeRepositories = employeeRepositories;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyProfile()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var employee = await _employeeRepositories.GetByEmailAsync(email);

            if (employee == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(employee);
        }
        [HttpGet]
        public async Task<IActionResult> CreateRequest()
        {
            ViewBag.Categories = await _categoryRepositories.GetAllAsync();
            ViewBag.SubCategories = await _subCategoryRepositories.GetAllAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRequest(AccessRequest request)
        {
            ViewBag.Categories = await _categoryRepositories.GetAllAsync();
            ViewBag.SubCategories = await _subCategoryRepositories.GetAllAsync();

            string email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var employee = await _employeeRepositories.GetByEmailAsync(email);

            if (employee == null)
            {
                ViewBag.Error = "Employee not found";
                return View(request);
            }
            request.EmployeeId = employee.EmployeeId;

            await _accessRequestRepositories.CreateRequestAsync(request);

            TempData["Success"] = "Request submitted successfully";
            return RedirectToAction("MyRequests");
        }

        public async Task<IActionResult> MyRequests()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var employee = await _employeeRepositories.GetByEmailAsync(email);

            if (employee == null)
            {
                return View(new List<AccessRequest>());
            }

            var requests = await _accessRequestRepositories.GetEmployeeRequestsAsync(employee.EmployeeId);
            return View(requests);
        }
    }
}