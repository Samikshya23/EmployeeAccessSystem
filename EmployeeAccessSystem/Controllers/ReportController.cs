using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Services;
using EmployeeAccessSystem.Pdf.Documents;
using EmployeeAccessSystem.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
namespace EmployeeAccessSystem.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public async Task<IActionResult> Index(int? selectedProductId, DateTime? fromDate, DateTime? toDate)
        {
            ReportPageViewModel model = await _reportService.GetReportPageAsync(selectedProductId, fromDate, toDate);
            return View(model);
        }
        public async Task<IActionResult> DownloadPdf(int? selectedProductId, DateTime? fromDate, DateTime? toDate)
        {
            ReportPageViewModel model = await _reportService.GetReportPageAsync(selectedProductId, fromDate, toDate);
            if (!model.SelectedProductId.HasValue)
            {
                TempData["Error"] = "Please select a product first.";
                return RedirectToAction("Index");
            }
            if (!model.HasData || model.ReportData == null || model.ReportData.Count == 0)
            {
                TempData["Error"] = "No report data found for PDF export.";
                return RedirectToAction("Index", new
                {
                    selectedProductId = selectedProductId,
                    fromDate = fromDate,
                    toDate = toDate
                });
            }
            ReportPdfDocument document = new ReportPdfDocument(model);
            byte[] pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", "MonitoringReport.pdf");
        }
        public async Task<IActionResult> DownloadExcel(int? selectedProductId, DateTime? fromDate, DateTime? toDate)
        {
            ReportPageViewModel model = await _reportService.GetReportPageAsync(selectedProductId, fromDate, toDate);

            if (!model.SelectedProductId.HasValue)
            {
                TempData["Error"] = "Please select a product first.";
                return RedirectToAction("Index");
            }
            if (!model.HasData || model.ReportData == null || model.ReportData.Count == 0)
            {
                TempData["Error"] = "No report data found for Excel export.";
                return RedirectToAction("Index", new
                {
                    selectedProductId = selectedProductId,
                    fromDate = fromDate,
                    toDate = toDate
                });
            }
            ReportExcelDocument document = new ReportExcelDocument(
                model.ReportData,
                model.ReportTitle,
                model.FromDate,
                model.ToDate
            );
            byte[] excelBytes = document.Generate();

            return File(
                excelBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "MonitoringReport.xlsx"
            );
        }
    }
}