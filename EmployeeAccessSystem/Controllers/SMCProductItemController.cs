using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    [Authorize]
    public class SMCProductItemController : Controller
    {
        private readonly ISMCProductItemService _service;
        private readonly ISMCProductRepository _smcProductRepo;

        public SMCProductItemController(
            ISMCProductItemService service,
            ISMCProductRepository smcProductRepo)
        {
            _service = service;
            _smcProductRepo = smcProductRepo;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            await LoadSMCProducts();

            var model = new SMCProductItem();
            model.IsActive = true;

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SMCProductItem model)
        {
            if (!ModelState.IsValid)
            {
                await LoadSMCProducts();
                return PartialView("Create", model);
            }

            var message = await _service.AddAsync(model);

            if (!string.IsNullOrWhiteSpace(message))
            {
                ViewBag.Error = message;
                await LoadSMCProducts();
                return PartialView("Create", model);
            }

            TempData["Success"] = "SMC Product Item added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            await LoadSMCProducts();
            return PartialView("Edit", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SMCProductItem model)
        {
            if (!ModelState.IsValid)
            {
                await LoadSMCProducts();
                return PartialView("Edit", model);
            }

            var existingItem = await _service.GetByIdAsync(model.SMCProductItemId);

            if (existingItem == null)
            {
                return NotFound();
            }

            var message = await _service.UpdateAsync(model);

            if (!string.IsNullOrWhiteSpace(message))
            {
                ViewBag.Error = message;
                await LoadSMCProducts();
                return PartialView("Edit", model);
            }

            TempData["Success"] = "SMC Product Item updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return PartialView("Delete", data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int smcProductItemId)
        {
            var existingItem = await _service.GetByIdAsync(smcProductItemId);

            if (existingItem == null)
            {
                return NotFound();
            }

            var message = await _service.DeleteAsync(smcProductItemId);

            if (!string.IsNullOrWhiteSpace(message))
            {
                TempData["Error"] = message;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "SMC Product Item deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadSMCProducts()
        {
            var products = await _smcProductRepo.GetActiveAsync();
            ViewBag.SMCProductList = new SelectList(products, "SMCProductId", "SMCProductName");
        }
    }
}