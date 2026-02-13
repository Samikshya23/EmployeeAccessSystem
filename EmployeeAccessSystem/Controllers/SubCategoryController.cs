using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryRepository _subRepo;
        private readonly ICategoryRepository _catRepo;

        public SubCategoryController(ISubCategoryRepository subRepo, ICategoryRepository catRepo)
        {
            _subRepo = subRepo;
            _catRepo = catRepo;
        }
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            return View(await _subRepo.GetAllAsync());
        }
        public async Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Categories = new SelectList(
                await _catRepo.GetActiveAsync(),
                "CategoryId",
                "CategoryName"
            );
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SubCategory subCategory)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            await _subRepo.AddAsync(subCategory);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Categories = new SelectList(
                await _catRepo.GetActiveAsync(),
                "CategoryId",
                "CategoryName"
            );
            return View(await _subRepo.GetByIdAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SubCategory subCategory)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            await _subRepo.UpdateAsync(subCategory);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            return View(await _subRepo.GetByIdAsync(id));
        } 
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetInt32("AccountId") == null)
                return RedirectToAction("Login", "Account");

            await _subRepo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
