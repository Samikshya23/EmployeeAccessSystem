using ClosedXML.Excel;
using EmployeeAccessSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace EmployeeAccessSystem.Excel
{
    public class ReportExcelDocument
    {
        private readonly List<ReportModel> _reportData;
        private readonly string _reportTitle;
        private readonly DateTime? _fromDate;
        private readonly DateTime? _toDate;
        private readonly bool _isPingReport;

        public ReportExcelDocument(
            List<ReportModel> reportData,
            string reportTitle,
            DateTime? fromDate,
            DateTime? toDate,
            bool isPingReport)
        {
            _reportData = reportData;
            _reportTitle = reportTitle;
            _fromDate = fromDate;
            _toDate = toDate;
            _isPingReport = isPingReport;
        }

        public byte[] Generate()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Report");

                if (_isPingReport)
                {
                    GeneratePingReport(worksheet);
                }
                else
                {
                    GenerateSmscReport(worksheet);
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private void GenerateSmscReport(IXLWorksheet worksheet)
        {
            List<DateTime> dateList = GetDateList();
            List<SmscRowItem> uniqueRows = GetUniqueSmscRows();

            int currentRow = 1;
            int totalColumns = 2 + dateList.Count;

            BuildTitleSection(worksheet, ref currentRow, totalColumns);
            BuildSmscHeader(worksheet, ref currentRow, dateList);

            for (int i = 0; i < uniqueRows.Count; i++)
            {
                SmscRowItem currentItem = uniqueRows[i];

                bool showMonitoringType = true;

                for (int x = 0; x < i; x++)
                {
                    if (uniqueRows[x].MonitoringTypeName == currentItem.MonitoringTypeName)
                    {
                        showMonitoringType = false;
                        break;
                    }
                }

                int sameTypeCount = 0;

                for (int y = 0; y < uniqueRows.Count; y++)
                {
                    if (uniqueRows[y].MonitoringTypeName == currentItem.MonitoringTypeName)
                    {
                        sameTypeCount++;
                    }
                }

                if (showMonitoringType)
                {
                    worksheet.Cell(currentRow, 1).Value = currentItem.MonitoringTypeName;

                    if (sameTypeCount > 1)
                    {
                        worksheet.Range(currentRow, 1, currentRow + sameTypeCount - 1, 1).Merge();
                    }

                    worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                    worksheet.Cell(currentRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                worksheet.Cell(currentRow, 2).Value = currentItem.ItemName;
                worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell(currentRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                for (int d = 0; d < dateList.Count; d++)
                {
                    string displayText = GetSmscCellValue(
                        currentItem.MonitoringTypeName,
                        currentItem.ItemName,
                        dateList[d]
                    );

                    worksheet.Cell(currentRow, d + 3).Value = displayText;
                    worksheet.Cell(currentRow, d + 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, d + 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    if (displayText == "✔")
                    {
                        worksheet.Cell(currentRow, d + 3).Style.Font.FontColor = XLColor.Green;
                        worksheet.Cell(currentRow, d + 3).Style.Font.Bold = true;
                    }
                }

                currentRow++;
            }

            ApplyOuterBorders(worksheet, 4, currentRow - 1, totalColumns);

            worksheet.Column(1).Width = 24;
            worksheet.Column(2).Width = 24;

            for (int c = 3; c <= totalColumns; c++)
            {
                worksheet.Column(c).Width = 10;
            }

            worksheet.Rows().AdjustToContents();
        }

        private void GeneratePingReport(IXLWorksheet worksheet)
        {
            List<DateTime> dateList = GetDateList();
            List<PingRowItem> uniqueRows = GetUniquePingRows();

            int currentRow = 1;
            int totalColumns = 3 + dateList.Count;

            BuildTitleSection(worksheet, ref currentRow, totalColumns);
            BuildPingHeader(worksheet, ref currentRow, dateList);

            for (int i = 0; i < uniqueRows.Count; i++)
            {
                PingRowItem currentItem = uniqueRows[i];

                worksheet.Cell(currentRow, 1).Value = i + 1;
                worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell(currentRow, 2).Value = currentItem.IPAddress;
                worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell(currentRow, 2).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                worksheet.Cell(currentRow, 3).Value = currentItem.ServerHostName;
                worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                worksheet.Cell(currentRow, 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                for (int d = 0; d < dateList.Count; d++)
                {
                    string displayText = GetPingCellValue(
                        currentItem.IPAddress,
                        currentItem.ServerHostName,
                        dateList[d]
                    );

                    worksheet.Cell(currentRow, d + 4).Value = displayText;
                    worksheet.Cell(currentRow, d + 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell(currentRow, d + 4).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                    if (displayText == "✔")
                    {
                        worksheet.Cell(currentRow, d + 4).Style.Font.FontColor = XLColor.Green;
                        worksheet.Cell(currentRow, d + 4).Style.Font.Bold = true;
                    }
                }

                currentRow++;
            }

            ApplyOuterBorders(worksheet, 4, currentRow - 1, totalColumns);

            worksheet.Column(1).Width = 8;
            worksheet.Column(2).Width = 18;
            worksheet.Column(3).Width = 28;

            for (int c = 4; c <= totalColumns; c++)
            {
                worksheet.Column(c).Width = 10;
            }

            worksheet.Rows().AdjustToContents();
        }

        private void BuildTitleSection(IXLWorksheet worksheet, ref int currentRow, int totalColumns)
        {
            worksheet.Cell(currentRow, 1).Value = _reportTitle;
            worksheet.Range(currentRow, 1, currentRow, totalColumns).Merge();
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 1).Style.Font.FontSize = 14;
            worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(currentRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            currentRow++;

            worksheet.Cell(currentRow, 1).Value =
                "From Date : " +
                (_fromDate.HasValue ? _fromDate.Value.ToString("dd/MM/yyyy") : "-") +
                "    To Date : " +
                (_toDate.HasValue ? _toDate.Value.ToString("dd/MM/yyyy") : "-");

            worksheet.Range(currentRow, 1, currentRow, totalColumns).Merge();
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Cell(currentRow, 1).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            currentRow++;
            currentRow++;
        }

        private void BuildSmscHeader(IXLWorksheet worksheet, ref int currentRow, List<DateTime> dateList)
        {
            int headerRow1 = currentRow;

            worksheet.Cell(headerRow1, 1).Value = "Monitoring Type";
            worksheet.Cell(headerRow1, 2).Value = "Item";

            for (int i = 0; i < dateList.Count; i++)
            {
                worksheet.Cell(headerRow1, i + 3).Value = dateList[i].ToString("dd");
            }

            currentRow++;

            int headerRow2 = currentRow;

            worksheet.Cell(headerRow2, 1).Value = "";
            worksheet.Cell(headerRow2, 2).Value = "";

            for (int i = 0; i < dateList.Count; i++)
            {
                worksheet.Cell(headerRow2, i + 3).Value = dateList[i].ToString("ddd");
            }

            ApplyHeaderStyle(worksheet, headerRow1, headerRow2, 2 + dateList.Count);

            currentRow++;
        }

        private void BuildPingHeader(IXLWorksheet worksheet, ref int currentRow, List<DateTime> dateList)
        {
            int headerRow1 = currentRow;

            worksheet.Cell(headerRow1, 1).Value = "SN";
            worksheet.Cell(headerRow1, 2).Value = "IP";
            worksheet.Cell(headerRow1, 3).Value = "Server Host Name";

            for (int i = 0; i < dateList.Count; i++)
            {
                worksheet.Cell(headerRow1, i + 4).Value = dateList[i].ToString("dd");
            }

            currentRow++;

            int headerRow2 = currentRow;

            worksheet.Cell(headerRow2, 1).Value = "";
            worksheet.Cell(headerRow2, 2).Value = "";
            worksheet.Cell(headerRow2, 3).Value = "";

            for (int i = 0; i < dateList.Count; i++)
            {
                worksheet.Cell(headerRow2, i + 4).Value = dateList[i].ToString("ddd");
            }

            ApplyHeaderStyle(worksheet, headerRow1, headerRow2, 3 + dateList.Count);

            currentRow++;
        }

        private void ApplyHeaderStyle(IXLWorksheet worksheet, int headerRow1, int headerRow2, int totalColumns)
        {
            worksheet.Range(headerRow1, 1, headerRow2, totalColumns).Style.Font.Bold = true;
            worksheet.Range(headerRow1, 1, headerRow2, totalColumns).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            worksheet.Range(headerRow1, 1, headerRow2, totalColumns).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            worksheet.Range(headerRow1, 1, headerRow2, totalColumns).Style.Fill.BackgroundColor = XLColor.LightGray;
            worksheet.Range(headerRow1, 1, headerRow2, totalColumns).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range(headerRow1, 1, headerRow2, totalColumns).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
        }

        private void ApplyOuterBorders(IXLWorksheet worksheet, int fromRow, int toRow, int totalColumns)
        {
            if (toRow >= fromRow)
            {
                worksheet.Range(fromRow, 1, toRow, totalColumns).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(fromRow, 1, toRow, totalColumns).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            }
        }

        private List<DateTime> GetDateList()
        {
            List<DateTime> dates = new List<DateTime>();

            if (_fromDate.HasValue && _toDate.HasValue)
            {
                DateTime currentDate = _fromDate.Value.Date;
                DateTime endDate = _toDate.Value.Date;

                while (currentDate <= endDate)
                {
                    dates.Add(currentDate);
                    currentDate = currentDate.AddDays(1);
                }
            }

            return dates;
        }

        private List<SmscRowItem> GetUniqueSmscRows()
        {
            List<SmscRowItem> uniqueRows = new List<SmscRowItem>();

            for (int i = 0; i < _reportData.Count; i++)
            {
                bool exists = false;

                for (int j = 0; j < uniqueRows.Count; j++)
                {
                    if (uniqueRows[j].MonitoringTypeName == _reportData[i].MonitoringTypeName &&
                        uniqueRows[j].ItemName == _reportData[i].ItemName)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    SmscRowItem item = new SmscRowItem();
                    item.MonitoringTypeName = _reportData[i].MonitoringTypeName;
                    item.ItemName = _reportData[i].ItemName;
                    uniqueRows.Add(item);
                }
            }

            return uniqueRows;
        }

        private List<PingRowItem> GetUniquePingRows()
        {
            List<PingRowItem> uniqueRows = new List<PingRowItem>();

            for (int i = 0; i < _reportData.Count; i++)
            {
                bool exists = false;

                for (int j = 0; j < uniqueRows.Count; j++)
                {
                    if (uniqueRows[j].IPAddress == _reportData[i].IPAddress &&
                        uniqueRows[j].ServerHostName == _reportData[i].ServerHostName)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    PingRowItem item = new PingRowItem();
                    item.IPAddress = _reportData[i].IPAddress;
                    item.ServerHostName = _reportData[i].ServerHostName;
                    uniqueRows.Add(item);
                }
            }

            return uniqueRows;
        }

        private string GetSmscCellValue(string monitoringTypeName, string itemName, DateTime date)
        {
            for (int r = 0; r < _reportData.Count; r++)
            {
                if (_reportData[r].MonitoringTypeName == monitoringTypeName &&
                    _reportData[r].ItemName == itemName &&
                    _reportData[r].EntryDate.Date == date.Date)
                {
                    if (!string.IsNullOrWhiteSpace(_reportData[r].EntryMode))
                    {
                        if (_reportData[r].EntryMode.Trim() == "Checkbox")
                        {
                            if (_reportData[r].IsChecked)
                            {
                                return "✔";
                            }

                            return "-";
                        }

                        if (_reportData[r].EntryMode.Trim() == "Value")
                        {
                            if (!string.IsNullOrWhiteSpace(_reportData[r].ConfigValue))
                            {
                                return _reportData[r].ConfigValue.Trim();
                            }

                            return "-";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(_reportData[r].ConfigValue))
                    {
                        return _reportData[r].ConfigValue.Trim();
                    }

                    return "-";
                }
            }

            return "-";
        }

        private string GetPingCellValue(string ipAddress, string serverHostName, DateTime date)
        {
            for (int r = 0; r < _reportData.Count; r++)
            {
                if (_reportData[r].IPAddress == ipAddress &&
                    _reportData[r].ServerHostName == serverHostName &&
                    _reportData[r].EntryDate.Date == date.Date)
                {
                    if (!string.IsNullOrWhiteSpace(_reportData[r].EntryMode))
                    {
                        if (_reportData[r].EntryMode.Trim() == "Checkbox")
                        {
                            if (_reportData[r].IsChecked)
                            {
                                return "✔";
                            }

                            return "-";
                        }

                        if (_reportData[r].EntryMode.Trim() == "Value")
                        {
                            if (!string.IsNullOrWhiteSpace(_reportData[r].ConfigValue))
                            {
                                return _reportData[r].ConfigValue.Trim();
                            }

                            return "-";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(_reportData[r].ConfigValue))
                    {
                        return _reportData[r].ConfigValue.Trim();
                    }

                    return "-";
                }
            }

            return "-";
        }

        private class SmscRowItem
        {
            public string MonitoringTypeName { get; set; } = string.Empty;
            public string ItemName { get; set; } = string.Empty;
        }

        private class PingRowItem
        {
            public string IPAddress { get; set; } = string.Empty;
            public string ServerHostName { get; set; } = string.Empty;
        }
    }
}