using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAccessSystem.Controllers
{
    [Authorize]
    public class SMCProductController : Controller
    {
        private readonly ISMCProductService _service;

        public SMCProductController(ISMCProductService service)
        {
            _service = service;
        }

        // ✅ FIXED
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        public IActionResult Create()
        {
            return View(new SMCProduct
            {
                IsActive = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SMCProduct smcProduct)
        {
            if (!ModelState.IsValid)
            {
                return View(smcProduct);
            }

            var error = await _service.AddAsync(smcProduct);

            if (error != null)
            {
                ViewBag.Error = error;
                return View(smcProduct);
            }

            TempData["Success"] = "SMC Product added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var smcProduct = await _service.GetByIdAsync(id);

            if (smcProduct == null)
                return NotFound();

            return View(smcProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SMCProduct smcProduct)
        {
            if (!ModelState.IsValid)
                return View(smcProduct);

            var existing = await _service.GetByIdAsync(smcProduct.SMCProductId);

            if (existing == null)
                return NotFound();

            var error = await _service.UpdateAsync(smcProduct);

            if (error != null)
            {
                ViewBag.Error = error;
                return View(smcProduct);
            }

            TempData["Success"] = "SMC Product updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var smcProduct = await _service.GetByIdAsync(id);

            if (smcProduct == null)
                return NotFound();

            return View(smcProduct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var existing = await _service.GetByIdAsync(id);

            if (existing == null)
                return NotFound();

            await _service.DeleteAsync(id);

            TempData["Success"] = "SMC Product deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}