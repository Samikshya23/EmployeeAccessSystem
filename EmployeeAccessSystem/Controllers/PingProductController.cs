using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    public class PingProductController : Controller
    {
        private readonly IPingProductService _service;
        private readonly IProductSetupRepositories _productRepo;

        public PingProductController(IPingProductService service, IProductSetupRepositories productRepo)
        {
            _service = service;
            _productRepo = productRepo;
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
            await LoadProducts();

            PingProduct model = new PingProduct();
            model.IsActive = true;

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PingProduct model)
        {
            string message = await _service.AddAsync(model);

            if (message == "Ping Product added successfully.")
            {
                return Content("success|" + message);
            }

            ViewBag.Error = message;
            await LoadProducts();
            return PartialView(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            await LoadProducts();
            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PingProduct model)
        {
            string message = await _service.UpdateAsync(model);

            if (message == "Ping Product updated successfully.")
            {
                return Content("success|" + message);
            }

            ViewBag.Error = message;
            await LoadProducts();
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
        public async Task<IActionResult> DeleteConfirmed(int pingProductId)
        {
            string message = await _service.DeleteAsync(pingProductId);

            if (message == "Ping Product deleted successfully.")
            {
                return Content("success|" + message);
            }

            return Content("error|" + message);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Toggle(int id)
        {
            string message = await _service.ToggleAsync(id);

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

        private async Task LoadProducts()
        {
            var products = await _productRepo.GetActiveAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");
        }
    }
}