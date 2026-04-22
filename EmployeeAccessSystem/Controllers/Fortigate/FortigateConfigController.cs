using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace EmployeeAccessSystem.Controllers
{
    public class FortigateConfigController : Controller
    {
        private readonly IFortigateConfigService _service;
        private readonly IProductSetupRepositories _productRepo;
        private readonly IFortigateCategoryRepository _categoryRepo;
        private readonly IFortigateItemRepositories _itemRepo;

        public FortigateConfigController(
            IFortigateConfigService service,
            IProductSetupRepositories productRepo,
            IFortigateCategoryRepository categoryRepo,
            IFortigateItemRepositories itemRepo)
        {
            _service = service;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _itemRepo = itemRepo;
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

            IEnumerable<FortigateConfig> data = await _service.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            await LoadProducts();

            List<FortigateCategory> categories = new List<FortigateCategory>();
            List<FortigateItem> items = new List<FortigateItem>();

            int selectedProductId = 0;
            int selectedCategoryId = 0;
            int selectedItemId = 0;

            IEnumerable<ProductSetup> products = await _productRepo.GetActiveAsync();

            foreach (ProductSetup product in products)
            {
                selectedProductId = product.ProductId;
                break;
            }

            if (selectedProductId > 0)
            {
                IEnumerable<FortigateCategory> categoryData = await _categoryRepo.GetActiveAsync();

                foreach (FortigateCategory category in categoryData)
                {
                    if (category.ProductId == selectedProductId)
                    {
                        categories.Add(category);
                    }
                }

                foreach (FortigateCategory category in categories)
                {
                    selectedCategoryId = category.FortigateCategoryId;
                    break;
                }
            }

            if (selectedCategoryId > 0)
            {
                IEnumerable<FortigateItem> itemData = await _itemRepo.GetByCategoryAsync(selectedCategoryId);

                foreach (FortigateItem item in itemData)
                {
                    items.Add(item);
                }

                foreach (FortigateItem item in items)
                {
                    selectedItemId = item.FortigateItemId;
                    break;
                }
            }

            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);
            ViewBag.CategoryList = new SelectList(categories, "FortigateCategoryId", "CategoryName", selectedCategoryId);
            ViewBag.ItemList = new SelectList(items, "FortigateItemId", "ItemName", selectedItemId);

            List<SelectListItem> statusList = new List<SelectListItem>();
            statusList.Add(new SelectListItem { Value = "UP", Text = "UP" });
            statusList.Add(new SelectListItem { Value = "DOWN", Text = "DOWN" });

            ViewBag.StatusList = new SelectList(statusList, "Value", "Text", "UP");

            FortigateConfig model = new FortigateConfig();
            model.ProductId = selectedProductId;
            model.FortigateCategoryId = selectedCategoryId;
            model.FortigateItemId = selectedItemId;
            model.EntryDate = DateTime.Today;
            model.ConfigValue = "UP";
            model.IsActive = true;

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FortigateConfig model)
        {
            if (!ModelState.IsValid)
            {
                await LoadProducts(model.ProductId);
                await LoadCategories(model.ProductId, model.FortigateCategoryId);
                await LoadItems(model.FortigateCategoryId, model.FortigateItemId);
                await LoadStatusList(model.ConfigValue);
                return PartialView(model);
            }

            string? currentUser = User.FindFirst(ClaimTypes.Email)?.Value
                                  ?? User.FindFirst("email")?.Value
                                  ?? User.Identity?.Name;

            string result = await _service.AddAsync(model, currentUser);

            string[] parts = result.Split('|', 2);
            string status = parts[0];
            string message = parts.Length > 1 ? parts[1] : "Operation failed.";

            if (status == "success")
            {
                return Content("success|" + message);
            }

            ViewBag.Error = message;
            await LoadProducts(model.ProductId);
            await LoadCategories(model.ProductId, model.FortigateCategoryId);
            await LoadItems(model.FortigateCategoryId, model.FortigateItemId);
            await LoadStatusList(model.ConfigValue);
            return PartialView(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            FortigateConfig? data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return Content("error|Fortigate config not found.");
            }

            await LoadProducts(data.ProductId);
            await LoadCategories(data.ProductId, data.FortigateCategoryId);
            await LoadItems(data.FortigateCategoryId, data.FortigateItemId);
            await LoadStatusList(data.ConfigValue);

            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FortigateConfig model)
        {
            if (!ModelState.IsValid)
            {
                await LoadProducts(model.ProductId);
                await LoadCategories(model.ProductId, model.FortigateCategoryId);
                await LoadItems(model.FortigateCategoryId, model.FortigateItemId);
                await LoadStatusList(model.ConfigValue);
                return PartialView(model);
            }

            string? currentUser = User.FindFirst(ClaimTypes.Email)?.Value
                                  ?? User.FindFirst("email")?.Value
                                  ?? User.Identity?.Name;

            string result = await _service.UpdateAsync(model, currentUser);

            string[] parts = result.Split('|', 2);
            string status = parts[0];
            string message = parts.Length > 1 ? parts[1] : "Operation failed.";

            if (status == "success")
            {
                return Content("success|" + message);
            }

            ViewBag.Error = message;
            await LoadProducts(model.ProductId);
            await LoadCategories(model.ProductId, model.FortigateCategoryId);
            await LoadItems(model.FortigateCategoryId, model.FortigateItemId);
            await LoadStatusList(model.ConfigValue);
            return PartialView(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            FortigateConfig? data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return Content("error|Fortigate config not found.");
            }

            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int FortigateConfigId)
        {
            string? currentUser = User.FindFirst(ClaimTypes.Email)?.Value
                                  ?? User.FindFirst("email")?.Value
                                  ?? User.Identity?.Name;

            string result = await _service.DeleteAsync(FortigateConfigId, currentUser);

            string[] parts = result.Split('|', 2);
            string status = parts[0];
            string message = parts.Length > 1 ? parts[1] : "Operation failed.";

            if (status == "success")
            {
                return Content("success|" + message);
            }

            return Content("error|" + message);
        }

        [HttpPost]
        public async Task<IActionResult> Toggle(int id)
        {
            string result = await _service.ToggleAsync(id);

            string[] parts = result.Split('|', 2);
            string status = parts[0];
            string message = parts.Length > 1 ? parts[1] : "Operation failed.";

            if (status == "success")
            {
                return Content("success|" + message);
            }

            return Content("error|" + message);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriesByProductId(int productId)
        {
            IEnumerable<FortigateCategory> data = await _categoryRepo.GetActiveAsync();

            List<FortigateCategory> result = new List<FortigateCategory>();

            foreach (FortigateCategory item in data)
            {
                if (item.ProductId == productId)
                {
                    result.Add(item);
                }
            }

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetItemsByCategoryId(int fortigateCategoryId)
        {
            IEnumerable<FortigateItem> data = await _itemRepo.GetByCategoryAsync(fortigateCategoryId);
            return Json(data);
        }

        private async Task LoadProducts(int? selectedProductId = null)
        {
            IEnumerable<ProductSetup> products = await _productRepo.GetActiveAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);
        }

        private async Task LoadCategories(int productId, int? selectedCategoryId = null)
        {
            IEnumerable<FortigateCategory> data = await _categoryRepo.GetActiveAsync();

            List<FortigateCategory> result = new List<FortigateCategory>();

            foreach (FortigateCategory item in data)
            {
                if (item.ProductId == productId)
                {
                    result.Add(item);
                }
            }

            ViewBag.CategoryList = new SelectList(result, "FortigateCategoryId", "CategoryName", selectedCategoryId);
        }

        private async Task LoadItems(int fortigateCategoryId, int? selectedItemId = null)
        {
            IEnumerable<FortigateItem> data = await _itemRepo.GetByCategoryAsync(fortigateCategoryId);
            ViewBag.ItemList = new SelectList(data, "FortigateItemId", "ItemName", selectedItemId);
        }

        private Task LoadStatusList(string? selectedValue = null)
        {
            List<SelectListItem> statusList = new List<SelectListItem>();
            statusList.Add(new SelectListItem { Value = "UP", Text = "UP" });
            statusList.Add(new SelectListItem { Value = "DOWN", Text = "DOWN" });

            ViewBag.StatusList = new SelectList(statusList, "Value", "Text", selectedValue);
            return Task.CompletedTask;
        }
    }
}