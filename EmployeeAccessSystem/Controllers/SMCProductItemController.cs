using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Services;
using EmployeeAccessSystem.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SMCProductItem model)
        {
            var message = await _service.AddAsync(model);

            if (message != null)
            {
                ViewBag.Error = message;
                await LoadSMCProducts();
                return View(model);
            }

            TempData["Success"] = "SMC Product Item added successfully.";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound();

            await LoadSMCProducts();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SMCProductItem model)
        {
            var message = await _service.UpdateAsync(model);

            if (message != null)
            {
                ViewBag.Error = message;
                await LoadSMCProducts();
                return View(model);
            }

            TempData["Success"] = "SMC Product Item updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound();

            return View(data);
        }
        public async Task<IActionResult> Details(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return NotFound();

            return View(data);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            TempData["Success"] = "SMC Product Item deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int id)
        {
            await _service.ToggleAsync(id);
            TempData["Success"] = "Status changed successfully.";
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadSMCProducts()
        {
            var products = await _smcProductRepo.GetAllAsync();
            ViewBag.SMCProductList = new SelectList(products, "SMCProductId", "SMCProductName");
        }
    }
}