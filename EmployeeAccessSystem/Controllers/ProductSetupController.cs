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
        public async Task<IActionResult> Toggle(int id)
        {
            await _service.ToggleAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductSetup productSetup)
        {
            if (!ModelState.IsValid)
            {
                if (IsAjaxRequest())
                {
                    return PartialView("Create", productSetup);
                }

                TempData["Error"] = "Please enter valid data.";
                return RedirectToAction(nameof(Index));
            }

            await _service.AddAsync(productSetup);

            if (IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = "Product added successfully."
                });
            }

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

            return View(productSetup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductSetup productSetup)
        {
            if (!ModelState.IsValid)
            {
                if (IsAjaxRequest())
                {
                    return PartialView("Edit", productSetup);
                }

                TempData["Error"] = "Please enter valid data.";
                return RedirectToAction(nameof(Index));
            }

            await _service.UpdateAsync(productSetup);

            if (IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = "Product updated successfully."
                });
            }

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

            return View(productSetup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int productId)
        {
            await _service.DeleteAsync(productId);

            if (IsAjaxRequest())
            {
                return Json(new
                {
                    success = true,
                    message = "Product deleted successfully."
                });
            }

            TempData["Success"] = "Product deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        private bool IsAjaxRequest()
        {
            var headerValue = Request.Headers["X-Requested-With"].ToString();

            if (headerValue == "XMLHttpRequest")
            {
                return true;
            }

            return false;
        }
    }
}