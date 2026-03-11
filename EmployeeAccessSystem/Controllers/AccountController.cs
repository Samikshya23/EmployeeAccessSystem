using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;

namespace EmployeeAccessSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IDepartmentRepository _departmentRepo;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAccountService accountService,
            IDepartmentRepository departmentRepo,
            ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _departmentRepo = departmentRepo;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            _logger.LogInformation("Register page opened.");

            var departments = await _departmentRepo.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            _logger.LogInformation("Register submitted. Email: " + model.Email);

            var departments = await _departmentRepo.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", model.DepartmentId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Register validation failed. Email: " + model.Email);
                return View(model);
            }

            string error = await _accountService.RegisterAsync(model);
            if (!string.IsNullOrEmpty(error))
            {
                _logger.LogWarning("Register failed. Email: " + model.Email + " Error: " + error);
                ViewBag.Error = error;
                return View(model);
            }

            _logger.LogInformation("Registration successful. Email: " + model.Email);

            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Login page opened.");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            _logger.LogInformation("Login submitted. Email: " + model.Email);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Login validation failed. Email: " + model.Email);
                return View(model);
            }

            string error = await _accountService.LoginAsync(model);
            if (!string.IsNullOrEmpty(error))
            {
                _logger.LogWarning("Login failed. Email: " + model.Email + " Error: " + error);
                ViewBag.Error = error;
                return View(model);
            }

            Account account = await _accountService.GetAccountByEmailAsync(model.Email);

            HttpContext.Session.SetInt32("AccountId", account.AccountId);
            HttpContext.Session.SetString("FullName", account.FullName ?? "");
            HttpContext.Session.SetString("Email", account.Email ?? "");
            HttpContext.Session.SetString("RoleName", account.RoleName ?? "");

            _logger.LogInformation("Login successful. AccountId: " + account.AccountId + " Email: " + model.Email + " Role: " + account.RoleName);

            if (account.RoleName == "Admin")
            {
                return RedirectToAction("Index", "Employee");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Logout()
        {
            string email = HttpContext.Session.GetString("Email") ?? "";

            _logger.LogInformation("Logout clicked. Email: " + email);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}