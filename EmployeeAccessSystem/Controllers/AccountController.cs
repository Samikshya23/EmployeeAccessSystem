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
        public AccountController(
            IAccountService accountService,
            IDepartmentRepository departmentRepo)
        {
            _accountService = accountService;
            _departmentRepo = departmentRepo;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var departments = await _departmentRepo.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var departments = await _departmentRepo.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", model.DepartmentId);
            string error = await _accountService.RegisterAsync(model);

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
                return View(model);
            }
            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string error = await _accountService.LoginAsync(model);
            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
                return View(model);
            }
            Account account = await _accountService.GetAccountByEmailAsync(model.Email);
            if (account == null)
            {
                ViewBag.Error = "Account not registered";
                return View(model);
            }
            HttpContext.Session.SetInt32("AccountId", account.AccountId);
            HttpContext.Session.SetString("FullName", account.FullName ?? "");
            HttpContext.Session.SetString("Email", account.Email ?? "");
            return RedirectToAction("Index", "Employee");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
