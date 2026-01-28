using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
       


        public EmployeeController(IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }


        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var employees = await _employeeRepository.GetAllAsync();
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();

            var departments = await _departmentRepository.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", employee.DepartmentId);

            return View(employee);
        }


        [HttpPost]
     
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

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
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var departments = await _departmentRepository.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");

           
            return View(new Employee());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(employee);

            int result = await _employeeRepository.AddAsync(employee);

            if (result == -1)
            {
                ModelState.AddModelError("Email", "Email is already used");
                return View(employee);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
      
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            await _employeeRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
