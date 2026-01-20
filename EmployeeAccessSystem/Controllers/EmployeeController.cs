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
        private readonly ICategoryRepository _categoryRepository;


        public EmployeeController(IEmployeeRepository employeeRepository,IDepartmentRepository departmentRepository, ICategoryRepository categoryRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _categoryRepository = categoryRepository;
        }


        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();

            var departments = await _departmentRepository.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", employee.DepartmentId);

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", employee.CategoryId);

            return View(employee);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentRepository.GetAllAsync();
                ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", employee.DepartmentId);

                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", employee.CategoryId);

                return View(employee);
            }

            await _employeeRepository.UpdateAsync(employee);
            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentRepository.GetAllAsync();
            ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            return View(new Employee());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentRepository.GetAllAsync();
                ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", employee.DepartmentId);

                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", employee.CategoryId);

                return View(employee);
            }

            try
            {
                await _employeeRepository.AddAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Database error: " + ex.Message);

                var departments = await _departmentRepository.GetAllAsync();
                ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName", employee.DepartmentId);

                var categories = await _categoryRepository.GetAllAsync();
                ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", employee.CategoryId);

                return View(employee);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
