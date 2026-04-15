using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    public class PingProductItemValueController : Controller
    {
        private readonly IPingProductItemValueService _service;
        private readonly IPingProductItemRepository _itemRepo;
        private readonly IPingProductFieldRepository _fieldRepo;

        public PingProductItemValueController(
            IPingProductItemValueService service,
            IPingProductItemRepository itemRepo,
            IPingProductFieldRepository fieldRepo)
        {
            _service = service;
            _itemRepo = itemRepo;
            _fieldRepo = fieldRepo;
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
            await LoadDropdowns();

            PingProductItemValue model = new PingProductItemValue();
            model.IsActive = true;

            if (IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int pingProductItemId, int[] pingProductFieldId, string[] fieldValue, bool isActive)
        {
            if (pingProductItemId <= 0)
            {
                ViewBag.Error = "Please select Ping Product Item.";
                await LoadDropdowns();

                PingProductItemValue model = new PingProductItemValue();
                model.IsActive = true;

                if (IsAjaxRequest())
                {
                    return PartialView(model);
                }

                return View(model);
            }

            if (pingProductFieldId == null || fieldValue == null || pingProductFieldId.Length == 0)
            {
                ViewBag.Error = "No fields found for selected item.";
                await LoadDropdowns();

                PingProductItemValue model = new PingProductItemValue();
                model.IsActive = true;

                if (IsAjaxRequest())
                {
                    return PartialView(model);
                }

                return View(model);
            }

            bool hasAnyValue = false;

            for (int i = 0; i < pingProductFieldId.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(fieldValue[i]))
                {
                    hasAnyValue = true;
                    break;
                }
            }

            if (!hasAnyValue)
            {
                ViewBag.Error = "Please enter at least one field value.";
                await LoadDropdowns();

                PingProductItemValue model = new PingProductItemValue();
                model.IsActive = true;

                if (IsAjaxRequest())
                {
                    return PartialView(model);
                }

                return View(model);
            }

            for (int i = 0; i < pingProductFieldId.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(fieldValue[i]))
                {
                    continue;
                }

                PingProductItemValue model = new PingProductItemValue
                {
                    PingProductItemId = pingProductItemId,
                    PingProductFieldId = pingProductFieldId[i],
                    FieldValue = fieldValue[i].Trim(),
                    IsActive = isActive
                };

                var message = await _service.AddAsync(model);

                if (message != "Ping Product Item Value added successfully.")
                {
                    ViewBag.Error = message;
                    await LoadDropdowns();

                    PingProductItemValue returnModel = new PingProductItemValue();
                    returnModel.IsActive = isActive;

                    if (IsAjaxRequest())
                    {
                        return PartialView(returnModel);
                    }

                    return View(returnModel);
                }
            }

            if (IsAjaxRequest())
            {
                return Content("success|Ping Product Item Values added successfully.");
            }

            return RedirectToAction("Index", new { successMessage = "Ping Product Item Values added successfully." });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            if (IsAjaxRequest())
            {
                return PartialView(data);
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PingProductItemValue model)
        {
            var message = await _service.UpdateAsync(model);

            if (message == "Ping Product Item Value updated successfully.")
            {
                if (IsAjaxRequest())
                {
                    return Content("success|Ping Product Item Value updated successfully.");
                }

                return RedirectToAction("Index", new { successMessage = "Ping Product Item Value updated successfully." });
            }

            ViewBag.Error = message;

            if (IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            if (IsAjaxRequest())
            {
                return PartialView(data);
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int pingProductItemValueId)
        {
            var message = await _service.DeleteAsync(pingProductItemValueId);

            if (message == "Ping Product Item Value deleted successfully.")
            {
                if (IsAjaxRequest())
                {
                    return Content("success|Ping Product Item Value deleted successfully.");
                }

                return RedirectToAction("Index", new { successMessage = "Ping Product Item Value deleted successfully." });
            }

            if (IsAjaxRequest())
            {
                return Content("error|" + message);
            }

            return RedirectToAction("Index", new { errorMessage = message });
        }

        [HttpGet]
        public async Task<IActionResult> LoadFieldsByItem(int pingProductItemId)
        {
            var item = await _itemRepo.GetByIdAsync(pingProductItemId);

            if (item == null)
            {
                return Content("");
            }

            var fields = await _fieldRepo.GetByPingProductIdAsync(item.PingProductId);
            return PartialView("_PingProductDynamicFields", fields);
        }

        private async Task LoadDropdowns()
        {
            var items = await _itemRepo.GetAllAsync();

            List<SelectListItem> itemList = new List<SelectListItem>();

            foreach (var item in items)
            {
                itemList.Add(new SelectListItem
                {
                    Value = item.PingProductItemId.ToString(),
                    Text = item.ItemName
                });
            }

            ViewBag.PingProductItemList = itemList;
        }

        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}