using Microsoft.AspNetCore.Mvc;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repo;

        public CategoryController(ICategoryRepository repo)
        {
            _repo = repo;
        }

    
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var data = await _repo.GetAllAsync();
            return View(data.ToList());
        }

       
        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            await _repo.ToggleAsync(id);
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

            if (!ModelState.IsValid) return View(category);

            await _repo.AddAsync(category);
            return RedirectToAction(nameof(Index));
        }

   
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var category = await _repo.GetByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

       
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(category);

            await _repo.UpdateAsync(category);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            var category = await _repo.GetByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

       
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int categoryId)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            await _repo.DeleteAsync(categoryId);
            return RedirectToAction(nameof(Index));
        }
    }
}
