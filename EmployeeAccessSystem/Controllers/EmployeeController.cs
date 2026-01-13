using Microsoft.AspNetCore.Mvc;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

    
        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return View(employees);
        }

  
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }
     
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
        {
            if (!ModelState.IsValid)
                return View(employee);

            var existingEmployee = await _employeeRepository.GetByIdAsync(employee.EmployeeId);
            if (existingEmployee == null)
                return NotFound();

       
            var emailOwner = await _employeeRepository.GetByEmailAsync(employee.Email);
            if (emailOwner != null && emailOwner.EmployeeId != employee.EmployeeId)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(employee);
            }

            await _employeeRepository.UpdateAsync(employee);
            TempData["Success"] = "Employee updated successfully!";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
         [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            return View(employee); 
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return NotFound();

            await _employeeRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
         
            if (!ModelState.IsValid)
            {
                return View(employee);
            }

            try
            {
          
                var existingEmployee = await _employeeRepository.GetByEmailAsync(employee.Email);
                if (existingEmployee != null)
                {
                   
                    ModelState.AddModelError("Email", "Email already exists");
                    return View(employee);
                }

         
                await _employeeRepository.AddAsync(employee);

         
                TempData["Success"] = "Employee added successfully!";
                return RedirectToAction("Index");
            }
            catch
            {
          
                ModelState.AddModelError("", "Something went wrong while saving the employee. Please try again.");
                return View(employee);
            }
        }
    }
}
