using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Services;
using EmployeeAccessSystem.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    [Authorize]
    public class SMCProductController : Controller
    {
        private readonly ISMCProductService _service;
        private readonly IProductSetupRepositories _productRepo;

        public SMCProductController(ISMCProductService service, IProductSetupRepositories productRepo)
        {
            _service = service;
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            await LoadProducts();

            var model = new SMCProduct();
            model.IsActive = true;

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SMCProduct smcProduct)
        {
            if (!ModelState.IsValid)
            {
                await LoadProducts();
                return PartialView("Create", smcProduct);
            }

            var error = await _service.AddAsync(smcProduct);

            if (error != null)
            {
                ViewBag.Error = error;
                await LoadProducts();
                return PartialView("Create", smcProduct);
            }

            TempData["Success"] = "SMC Product added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var smcProduct = await _service.GetByIdAsync(id);

            if (smcProduct == null)
            {
                return NotFound();
            }

            await LoadProducts();
            return PartialView("Edit", smcProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SMCProduct smcProduct)
        {
            if (!ModelState.IsValid)
            {
                await LoadProducts();
                return PartialView("Edit", smcProduct);
            }

            var existing = await _service.GetByIdAsync(smcProduct.SMCProductId);

            if (existing == null)
            {
                return NotFound();
            }

            var error = await _service.UpdateAsync(smcProduct);

            if (error != null)
            {
                ViewBag.Error = error;
                await LoadProducts();
                return PartialView("Edit", smcProduct);
            }

            TempData["Success"] = "SMC Product updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var smcProduct = await _service.GetByIdAsync(id);

            if (smcProduct == null)
            {
                return NotFound();
            }

            return PartialView("Delete", smcProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int smcProductId)
        {
            var error = await _service.DeleteAsync(smcProductId);

            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "SMC Product deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadProducts()
        {
            var products = await _productRepo.GetAllAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");
        }
    }
}