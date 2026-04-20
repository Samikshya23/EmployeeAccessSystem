using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    public class FortigateCategoryController : Controller
    {
        private readonly IFortigateCategoryService _service;
        private readonly IProductSetupRepositories _productRepo;

        public FortigateCategoryController(
            IFortigateCategoryService service,
            IProductSetupRepositories productRepo)
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

            FortigateCategory model = new FortigateCategory();
            model.IsActive = true;
            model.DisplayOrder = 0;

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FortigateCategory model)
        {
            if (!ModelState.IsValid)
            {
                await LoadProducts();
                return PartialView(model);
            }

            if (model.DisplayOrder < 0)
            {
                model.DisplayOrder = 0;
            }

            string message = await _service.AddAsync(model);

            if (message.Contains("successfully"))
            {
                return Content("success|" + message);
            }

            ViewBag.Error = message;
            await LoadProducts();
            return PartialView(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            await LoadProducts();

            FortigateCategory model = await _service.GetByIdAsync(id);

            if (model == null)
            {
                return Content("error|Fortigate category not found.");
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FortigateCategory model)
        {
            if (!ModelState.IsValid)
            {
                await LoadProducts();
                return PartialView(model);
            }

            if (model.DisplayOrder < 0)
            {
                model.DisplayOrder = 0;
            }

            string message = await _service.UpdateAsync(model);

            if (message.Contains("successfully"))
            {
                return Content("success|" + message);
            }

            ViewBag.Error = message;
            await LoadProducts();
            return PartialView(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            FortigateCategory model = await _service.GetByIdAsync(id);

            if (model == null)
            {
                return Content("error|Fortigate category not found.");
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int FortigateCategoryId)
        {
            string message = await _service.DeleteAsync(FortigateCategoryId);

            if (message.Contains("successfully"))
            {
                return Content("success|" + message);
            }

            return Content("error|" + message);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            string message = await _service.ToggleAsync(id);

            if (message.Contains("successfully"))
            {
                return Content("success|" + message);
            }

            return Content("error|" + message);
        }

        private async Task LoadProducts()
        {
            var products = await _productRepo.GetActiveAsync();

            List<SelectListItem> productList = new List<SelectListItem>();

            foreach (var item in products)
            {
                SelectListItem listItem = new SelectListItem();
                listItem.Value = item.ProductId.ToString();
                listItem.Text = item.ProductName;

                productList.Add(listItem);
            }

            ViewBag.ProductList = productList;
        }
    }
}