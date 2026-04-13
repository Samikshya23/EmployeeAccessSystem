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

        public ReportExcelDocument(List<ReportModel> reportData, string reportTitle, DateTime? fromDate, DateTime? toDate)
        {
            _reportData = reportData;
            _reportTitle = reportTitle;
            _fromDate = fromDate;
            _toDate = toDate;
        }

        public byte[] Generate()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Report");
                List<DateTime> dateList = GetDateList();
                List<ReportRowItem> uniqueRows = GetUniqueRows();

                int currentRow = 1;

                worksheet.Cell(currentRow, 1).Value = _reportTitle;
                int totalColumns = 2 + dateList.Count;
                worksheet.Range(currentRow, 1, currentRow, totalColumns).Merge();
                worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 1).Style.Font.FontSize = 14;
                worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.LightGreen;

                currentRow++;

                int midStart = (totalColumns / 2) - 2;
                int midEnd = (totalColumns / 2) + 1;

                if (midStart < 1)
                {
                    midStart = 1;
                }

                if (midEnd > totalColumns)
                {
                    midEnd = totalColumns;
                }

                worksheet.Range(currentRow, midStart, currentRow, midEnd).Merge();

                string dateText = "From Date : " +
                                  (_fromDate.HasValue ? _fromDate.Value.ToString("dd/MM/yyyy") : "-") +
                                  "        To Date : " +
                                  (_toDate.HasValue ? _toDate.Value.ToString("dd/MM/yyyy") : "-");

                worksheet.Cell(currentRow, midStart).Value = dateText;
                worksheet.Cell(currentRow, midStart).Style.Font.Bold = true;
                worksheet.Cell(currentRow, midStart).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell(currentRow, midStart).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                currentRow++;
                currentRow++;

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

                worksheet.Range(headerRow1, 1, headerRow2, 2 + dateList.Count).Style.Font.Bold = true;
                worksheet.Range(headerRow1, 1, headerRow2, 2 + dateList.Count).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Range(headerRow1, 1, headerRow2, 2 + dateList.Count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range(headerRow1, 1, headerRow2, 2 + dateList.Count).Style.Fill.BackgroundColor = XLColor.LightGray;
                worksheet.Range(headerRow1, 1, headerRow2, 2 + dateList.Count).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(headerRow1, 1, headerRow2, 2 + dateList.Count).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                currentRow++;

                for (int i = 0; i < uniqueRows.Count; i++)
                {
                    ReportRowItem currentItem = uniqueRows[i];

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
                        worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    }

                    worksheet.Cell(currentRow, 2).Value = currentItem.ItemName;
                    worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    for (int d = 0; d < dateList.Count; d++)
                    {
                        string displayText = "-";
                        bool foundRow = false;
                        bool isChecked = false;
                        string entryMode = "";
                        string configValue = "";

                        for (int r = 0; r < _reportData.Count; r++)
                        {
                            if (_reportData[r].MonitoringTypeName == currentItem.MonitoringTypeName &&
                                _reportData[r].ItemName == currentItem.ItemName &&
                                _reportData[r].EntryDate.Date == dateList[d].Date)
                            {
                                foundRow = true;
                                isChecked = _reportData[r].IsChecked;

                                if (_reportData[r].EntryMode != null)
                                {
                                    entryMode = _reportData[r].EntryMode.Trim();
                                }

                                if (_reportData[r].ConfigValue != null)
                                {
                                    configValue = _reportData[r].ConfigValue.Trim();
                                }

                                break;
                            }
                        }

                        if (!foundRow)
                        {
                            displayText = "-";
                        }
                        else if (entryMode == "Checkbox")
                        {
                            if (isChecked)
                            {
                                displayText = "✔";
                            }
                            else
                            {
                                displayText = "✘";
                            }
                        }
                        else if (entryMode == "Value")
                        {
                            if (configValue != "")
                            {
                                displayText = configValue;
                            }
                            else
                            {
                                displayText = "-";
                            }
                        }
                        else
                        {
                            if (configValue != "")
                            {
                                displayText = configValue;
                            }
                            else
                            {
                                displayText = "-";
                            }
                        }

                        worksheet.Cell(currentRow, d + 3).Value = displayText;
                        worksheet.Cell(currentRow, d + 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Cell(currentRow, d + 3).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                        if (displayText == "✔")
                        {
                            worksheet.Cell(currentRow, d + 3).Style.Font.FontColor = XLColor.Green;
                            worksheet.Cell(currentRow, d + 3).Style.Font.Bold = true;
                        }
                        else if (displayText == "✘")
                        {
                            worksheet.Cell(currentRow, d + 3).Style.Font.FontColor = XLColor.Red;
                            worksheet.Cell(currentRow, d + 3).Style.Font.Bold = true;
                        }
                    }

                    currentRow++;

                    bool isLastOfGroup = true;

                    if (i < uniqueRows.Count - 1)
                    {
                        if (uniqueRows[i].MonitoringTypeName == uniqueRows[i + 1].MonitoringTypeName)
                        {
                            isLastOfGroup = false;
                        }
                    }

                    if (isLastOfGroup)
                    {
                        currentRow++;
                    }
                }

                worksheet.Range(headerRow1, 1, currentRow - 1, 2 + dateList.Count).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(headerRow1, 1, currentRow - 1, 2 + dateList.Count).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                worksheet.Column(1).Width = 22;
                worksheet.Column(2).Width = 22;

                for (int c = 3; c <= 2 + dateList.Count; c++)
                {
                    worksheet.Column(c).Width = 10;
                }

                worksheet.Rows().AdjustToContents();

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
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

        private List<ReportRowItem> GetUniqueRows()
        {
            List<ReportRowItem> uniqueRows = new List<ReportRowItem>();

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
                    ReportRowItem item = new ReportRowItem();
                    item.MonitoringTypeName = _reportData[i].MonitoringTypeName;
                    item.ItemName = _reportData[i].ItemName;
                    uniqueRows.Add(item);
                }
            }

            return uniqueRows;
        }

        private class ReportRowItem
        {
            public string MonitoringTypeName { get; set; }
            public string ItemName { get; set; }
        }
    }
}