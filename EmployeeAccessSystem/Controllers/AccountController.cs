using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Helpers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IConfiguration _config;

        public AccountController(IAccountRepository accountRepo, IConfiguration config)
        {
            _accountRepo = accountRepo;
            _config = config;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password, string confirmPassword)
        {
          
            if (string.IsNullOrWhiteSpace(fullName))
            {
                ViewBag.Error = "Full Name should not be empty";
                return View();
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Email should not be empty";
                return View();
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Password should not be empty";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Password and Confirm Password do not match";
                return View();
            }

           
            fullName = fullName.Trim();
            email = email.Trim().ToLower();

           
            var existing = await _accountRepo.GetByEmailAsync(email);
            if (existing != null)
            {
                ViewBag.Error = "Email already registered";
                return View();
            }

            
            string key = _config["Security:PasswordKey"];
            Helper.CreatePasswordHash(password, key, out byte[] hash, out byte[] salt);
            var account = new Account
            {
                FullName = fullName,
                Email = email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            await _accountRepo.CreateAsync(account);

            TempData["Success"] = "Registration successful. Please login.";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
   
            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Email should not be empty";
                return View();
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Password should not be empty";
                return View();
            }

            email = email.Trim().ToLower();

       
            var account = await _accountRepo.GetByEmailAsync(email);
            if (account == null)
            {
                ViewBag.Error = "Account not found";
                return View();
            }

            string key = _config["Security:PasswordKey"];
            bool ok = Helper.VerifyPassword(password, key, account.PasswordHash, account.PasswordSalt);

            if (!ok)
            {
                ViewBag.Error = "Invalid password";
                return View();
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
