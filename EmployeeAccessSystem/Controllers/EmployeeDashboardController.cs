using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Services;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Controllers
{
    public class EmployeeDashboardController : Controller
    {
        private readonly IAccessRequestService _accessRequestService;
        private readonly ICategoryRepositories _categoryRepository;
        private readonly ISubCategoryRepositories _subCategoryRepository;
        private readonly IEmployeeRepositories _employeeRepository;
        public EmployeeDashboardController(
            IAccessRequestService accessRequestService,
            ICategoryRepositories categoryRepository,
            ISubCategoryRepositories subCategoryRepository,
            IEmployeeRepositories employeeRepository)
        {
            _accessRequestService = accessRequestService;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyProfile()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CreateRequest()
        {
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.SubCategories = await _subCategoryRepository.GetAllAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRequest(AccessRequest request)
        {
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.SubCategories = await _subCategoryRepository.GetAllAsync();

            string email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var employee = await _employeeRepository.GetByEmailAsync(email);

            if (employee == null)
            {
                ViewBag.Error = "Employee not found";
                return View(request);
            }
            request.EmployeeId = employee.EmployeeId;

            string error = await _accessRequestService.CreateRequestAsync(request);

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
                return View(request);
            }
            TempData["Success"] = "Request submitted successfully";
            return RedirectToAction("MyRequests");
        }
        public async Task<IActionResult> MyRequests()
        {
            string email = User.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var employee = await _employeeRepository.GetByEmailAsync(email);

            if (employee == null)
            {
                return View(new List<AccessRequest>());
            }
            var requests = await _accessRequestService.GetEmployeeRequestsAsync(employee.EmployeeId);
            return View(requests);
        }
    }
}