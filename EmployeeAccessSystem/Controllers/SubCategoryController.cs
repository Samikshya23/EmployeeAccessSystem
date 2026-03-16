using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    [Authorize]
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryRepository _subRepo;
        private readonly ICategoryRepository _catRepo;
        private readonly ISubCategoryService _subService;

        public SubCategoryController(
            ISubCategoryRepository subRepo,
            ICategoryRepository catRepo,
            ISubCategoryService subService)
        {
            _subRepo = subRepo;
            _catRepo = catRepo;
            _subService = subService;
        }

        public async Task<IActionResult> Index()
        {
            var subCategories = await _subRepo.GetAllAsync();
            return View(subCategories);
        }

        public async Task<IActionResult> Create()
        {
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
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    await _catRepo.GetActiveAsync(),
                    "CategoryId",
                    "CategoryName",
                    subCategory.CategoryId
                );

                return View(subCategory);
            }

            await _subService.CreateAsync(subCategory);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var subCategory = await _subRepo.GetByIdAsync(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(
                await _catRepo.GetActiveAsync(),
                "CategoryId",
                "CategoryName",
                subCategory.CategoryId
            );

            return View(subCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(
                    await _catRepo.GetActiveAsync(),
                    "CategoryId",
                    "CategoryName",
                    subCategory.CategoryId
                );

                return View(subCategory);
            }

            await _subService.UpdateAsync(subCategory);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var subCategory = await _subRepo.GetByIdAsync(id);

            if (subCategory == null)
            {
                return NotFound();
            }

            return View(subCategory);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _subService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}