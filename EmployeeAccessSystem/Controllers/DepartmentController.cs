using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Services;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _service.GetAllAsync();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            await _service.AddAsync(department);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var department = await _service.GetByIdAsync(id);
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            await _service.UpdateAsync(department);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var department = await _service.GetByIdAsync(id);
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}