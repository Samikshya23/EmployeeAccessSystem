using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Controllers
{
    [Authorize]
    public class ProductSetupController : Controller
    {
        private readonly IProductSetupService _service;

        public ProductSetupController(IProductSetupService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int id)
        {
            await _service.ToggleAsync(id);
            TempData["Success"] = "Product status updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            var model = new ProductSetup();
            model.IsActive = true;
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductSetup productSetup)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Create", productSetup);
            }

            await _service.AddAsync(productSetup);

            TempData["Success"] = "Product added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productSetup = await _service.GetByIdAsync(id);

            if (productSetup == null)
            {
                return NotFound();
            }

            return PartialView("Edit", productSetup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductSetup productSetup)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Edit", productSetup);
            }

            await _service.UpdateAsync(productSetup);

            TempData["Success"] = "Product updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productSetup = await _service.GetByIdAsync(id);

            if (productSetup == null)
            {
                return NotFound();
            }

            return PartialView("Delete", productSetup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int productId)
        {
            await _service.DeleteAsync(productId);

            TempData["Success"] = "Product deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}