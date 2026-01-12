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
        public IActionResult Create()
        {
            return View();
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
