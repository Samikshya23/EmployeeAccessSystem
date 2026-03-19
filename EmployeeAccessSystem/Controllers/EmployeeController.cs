using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentRepositories _departmentRepository;

        public EmployeeController(
            IEmployeeService employeeService,
            IDepartmentRepositories departmentRepository)
        {
            _employeeService = employeeService;
            _departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index(string search)
        {
            var employees = await _employeeService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();

                employees = employees.Where(e =>
                    (!string.IsNullOrEmpty(e.FullName) && e.FullName.ToLower().Contains(search)) ||
                    (!string.IsNullOrEmpty(e.Email) && e.Email.ToLower().Contains(search))
                );
            }

            ViewBag.Search = search;
            return View(employees);
        }
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            await LoadDropdowns(employee.DepartmentId, employee.Role, employee.EmployeeId, employee.SupervisorEmployeeId);

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
            await LoadDropdowns(employee.DepartmentId, employee.Role, employee.EmployeeId, employee.SupervisorEmployeeId);

            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            string error = await _employeeService.UpdateAsync(employee);

            if (!string.IsNullOrEmpty(error))
            {
                ViewBag.Error = error;
                return View(employee);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return RedirectToAction("Register", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            await _employeeService.ToggleAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string error = await _employeeService.DeleteAsync(id);

            if (!string.IsNullOrEmpty(error))
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }

        private async Task LoadDropdowns(int selectedDepartmentId, string selectedRole, int currentEmployeeId, int? selectedSupervisorId)
        {
            var departments = await _departmentRepository.GetAllAsync();

            ViewBag.Departments = new SelectList(
                departments,
                "DepartmentId",
                "DepartmentName",
                selectedDepartmentId
            );

            ViewBag.Roles = new SelectList(
                new List<string> { "Admin", "Employee", "Supervisor" },
                selectedRole
            );

            var supervisors = await _employeeService.GetSupervisorsAsync();

            var filteredSupervisors = supervisors
                .Where(x => x.EmployeeId != currentEmployeeId)
                .ToList();

            ViewBag.Supervisors = new SelectList(
                filteredSupervisors,
                "EmployeeId",
                "FullName",
                selectedSupervisorId
            );
        }
    }
}