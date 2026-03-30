using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IProductSetupService _productSetupService;

        public ReportService(IReportRepository reportRepository, IProductSetupService productSetupService)
        {
            _reportRepository = reportRepository;
            _productSetupService = productSetupService;
        }

        public async Task<ReportPageViewModel> GetReportPageAsync(int? selectedProductId, DateTime? fromDate, DateTime? toDate)
        {
            ReportPageViewModel vm = new ReportPageViewModel();

            vm.ProductList = (await _productSetupService.GetAllAsync()).ToList();

            if (!fromDate.HasValue)
            {
                fromDate = DateTime.Today;
            }

            if (!toDate.HasValue)
            {
                toDate = DateTime.Today;
            }

            vm.SelectedProductId = selectedProductId;
            vm.FromDate = fromDate;
            vm.ToDate = toDate;

            if (!selectedProductId.HasValue)
            {
                vm.HasData = false;
                vm.ReportTitle = "Monitoring Report";
                return vm;
            }

            vm.ReportData = await _reportRepository.GetReportDataAsync(
                selectedProductId.Value,
                fromDate.Value,
                toDate.Value
            );

            vm.ReportTitle = GetProductName(vm.ProductList, selectedProductId.Value) + " Report Summary";
            vm.Dates = GetUniqueDates(vm.ReportData);
            vm.HasData = vm.ReportData.Count > 0;

            return vm;
        }

        private string GetProductName(List<ProductSetup> products, int productId)
        {
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].ProductId == productId)
                {
                    return products[i].ProductName;
                }
            }

            return "Monitoring";
        }

        private List<DateTime> GetUniqueDates(List<ReportModel> data)
        {
            List<DateTime> dates = new List<DateTime>();

            for (int i = 0; i < data.Count; i++)
            {
                DateTime currentDate = data[i].EntryDate.Date;
                bool exists = false;

                for (int j = 0; j < dates.Count; j++)
                {
                    if (dates[j] == currentDate)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    dates.Add(currentDate);
                }
            }

            dates.Sort();
            return dates;
        }
    }
}