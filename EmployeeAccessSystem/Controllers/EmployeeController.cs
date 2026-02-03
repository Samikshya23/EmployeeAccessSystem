using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IAccountRepository _accountRepository;

        public EmployeeController(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IAccountRepository accountRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _accountRepository = accountRepository;
        }

        private bool IsLoggedIn() => HttpContext.Session.GetInt32("AccountId") != null;
        private IActionResult GoLogin() => RedirectToAction("Login", "Account");

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn()) return GoLogin();

            var employees = await _employeeRepository.GetAllAsync();
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsLoggedIn()) return GoLogin();

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn()) return GoLogin();

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();

            var departments = await _departmentRepository.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", employee.DepartmentId);

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (!IsLoggedIn()) return GoLogin();

            if (!ModelState.IsValid)
            {
                var departments = await _departmentRepository.GetAllAsync();
                ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", employee.DepartmentId);
                return View(employee);
            }

            await _employeeRepository.UpdateAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            return RedirectToAction("Register", "Account");
        }

     
        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            if (!IsLoggedIn()) return GoLogin();

            await _employeeRepository.ToggleAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn()) return GoLogin();

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsLoggedIn()) return GoLogin();

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();

            await _accountRepository.DeleteAsync(employee.AccountId);

            return RedirectToAction(nameof(Index));
        }
    }
}
