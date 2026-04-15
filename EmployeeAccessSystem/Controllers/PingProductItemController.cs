using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    public class PingProductItemController : Controller
    {
        private readonly IPingProductItemService _service;
        private readonly IPingProductRepository _pingProductRepo;

        public PingProductItemController(IPingProductItemService service, IPingProductRepository pingProductRepo)
        {
            _service = service;
            _pingProductRepo = pingProductRepo;
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
            await LoadPingProducts();

            PingProductItem model = new PingProductItem();
            model.IsActive = true;

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PingProductItem model)
        {
            var message = await _service.AddAsync(model);

            if (message == "Ping Product Item added successfully.")
            {
                return Content("success|Ping Product Item added successfully.");
            }

            ViewBag.Error = message;
            await LoadPingProducts();
            return PartialView(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            await LoadPingProducts();
            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PingProductItem model)
        {
            var message = await _service.UpdateAsync(model);

            if (message == "Ping Product Item updated successfully.")
            {
                return Content("success|Ping Product Item updated successfully.");
            }

            ViewBag.Error = message;
            await LoadPingProducts();
            return PartialView(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int pingProductItemId)
        {
            var message = await _service.DeleteAsync(pingProductItemId);

            if (message == "Ping Product Item deleted successfully.")
            {
                return Content("success|Ping Product Item deleted successfully.");
            }

            return Content("error|" + message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int id)
        {
            var message = await _service.ToggleAsync(id);

            if (message == "Status changed successfully.")
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction("Index");
        }

        private async Task LoadPingProducts()
        {
            var pingProducts = await _pingProductRepo.GetActiveAsync();
            ViewBag.PingProductList = new SelectList(pingProducts, "PingProductId", "PingProductName");
        }
    }
}