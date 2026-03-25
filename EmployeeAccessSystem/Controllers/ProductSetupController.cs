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
        public async Task<IActionResult> Create(ProductSetup productSetup)
        {
            if (!ModelState.IsValid)
            {
                return View(productSetup);
            }

            await _service.AddAsync(productSetup);
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
        public async Task<IActionResult> Edit(ProductSetup productSetup)
        {
            if (!ModelState.IsValid)
            {
                return View(productSetup);
            }

            await _service.UpdateAsync(productSetup);
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
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int productId)
        {
            await _service.DeleteAsync(productId);
            return RedirectToAction(nameof(Index));
        }
    }
}