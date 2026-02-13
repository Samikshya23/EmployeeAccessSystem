using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Services;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var data = await _service.GetAllAsync();
            return View(data.ToList());
        }
        public async Task<IActionResult> Index1()
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var data = await _service.GetAllAsync();
            return View(data.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            await _service.ToggleAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(category);

            await _service.AddAsync(category);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var category = await _service.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(category);

            await _service.UpdateAsync(category);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var category = await _service.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int categoryId)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            await _service.DeleteAsync(categoryId);
            return RedirectToAction(nameof(Index));
        }
    }
}
