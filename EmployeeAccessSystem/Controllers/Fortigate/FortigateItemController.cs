using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    public class FortigateItemController : Controller
    {
        private readonly IFortigateItemService _service;
        private readonly IFortigateCategoryRepository _categoryRepo;

        public FortigateItemController(
            IFortigateItemService service,
            IFortigateCategoryRepository categoryRepo)
        {
            _service = service;
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index(string successMessage, string errorMessage)
        {
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                TempData["Success"] = successMessage;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                TempData["Error"] = errorMessage;
            }

            var data = await _service.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            await LoadCategories();

            FortigateItem model = new FortigateItem();
            model.IsActive = true;

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FortigateItem model)
        {
            string message = await _service.AddAsync(model);

            if (message == "Fortigate item added successfully.")
            {
                return Content("success|" + message);
            }

            await LoadCategories();
            return PartialView(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            FortigateItem model = await _service.GetByIdAsync(id);

            if (model == null)
            {
                return Content("error|Fortigate item not found.");
            }

            await LoadCategories();
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FortigateItem model)
        {
            string message = await _service.UpdateAsync(model);

            if (message == "Fortigate item updated successfully.")
            {
                return Content("success|" + message);
            }

            await LoadCategories();
            return PartialView(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            FortigateItem model = await _service.GetByIdAsync(id);

            if (model == null)
            {
                return Content("error|Fortigate item not found.");
            }

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string message = await _service.DeleteAsync(id);

            if (message == "Fortigate item deleted successfully.")
            {
                return Content("success|" + message);
            }

            return Content("error|" + message);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            string message = await _service.ToggleAsync(id);

            if (message == "Fortigate item status changed successfully.")
            {
                return Content("success|" + message);
            }

            return Content("error|" + message);
        }

        private async Task LoadCategories()
        {
            var categories = await _categoryRepo.GetActiveAsync();

            ViewBag.FortigateCategoryList = new SelectList(
                categories,
                "FortigateCategoryId",
                "CategoryName"
            );
        }
    }
}