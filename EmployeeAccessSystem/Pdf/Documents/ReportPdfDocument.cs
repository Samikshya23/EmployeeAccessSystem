using EmployeeAccessSystem.Models;
using QuestPDF.Elements.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;

namespace EmployeeAccessSystem.Pdf.Documents
{
    public class ReportPdfDocument : IDocument
    {
        private readonly ReportPageViewModel _model;

        public ReportPdfDocument(ReportPageViewModel model)
        {
            _model = model;
        }

        public DocumentMetadata GetMetadata()
        {
            return DocumentMetadata.Default;
        }

        public void Compose(IDocumentContainer container)
        {
            container.Page(BuildPage);
        }

        private void BuildPage(PageDescriptor page)
        {
            page.Size(PageSizes.A3.Landscape());
            page.Margin(12);
            page.DefaultTextStyle(TextStyle.Default.FontSize(8));

            page.Content().Column(delegate (ColumnDescriptor col)
            {
                col.Item().Text(GetReportTitle()).Bold().FontSize(14);

                col.Item().Text("Product: " + GetSelectedProductName());

                if (_model.FromDate.HasValue && _model.ToDate.HasValue)
                {
                    col.Item().Text(
                        "Date Range: " +
                        _model.FromDate.Value.ToString("dd MMM yyyy") +
                        " to " +
                        _model.ToDate.Value.ToString("dd MMM yyyy")
                    );
                }

                col.Item().PaddingTop(8).Element(BuildTable);
            });
        }

        private void BuildTable(IContainer container)
        {
            container.Table(delegate (TableDescriptor table)
            {
                table.ColumnsDefinition(delegate (TableColumnsDefinitionDescriptor columns)
                {
                    columns.ConstantColumn(110);
                    columns.ConstantColumn(120);

                    if (_model.Dates != null)
                    {
                        int i = 0;

                        while (i < _model.Dates.Count)
                        {
                            columns.RelativeColumn();
                            i++;
                        }
                    }
                });

                table.Cell().Element(HeaderStyle).Text("Monitoring Type").Bold();
                table.Cell().Element(HeaderStyle).Text("Item").Bold();

                if (_model.Dates != null)
                {
                    int i = 0;

                    while (i < _model.Dates.Count)
                    {
                        DateTime d = _model.Dates[i];

                        table.Cell().Element(HeaderStyle).Column(delegate (ColumnDescriptor col)
                        {
                            col.Item().AlignCenter().Text(d.ToString("dd/MM")).Bold();
                            col.Item().AlignCenter().Text(d.ToString("ddd"));
                        });

                        i++;
                    }
                }

                List<ReportModel> uniqueRows = GetUniqueRows();

                int rowIndex = 0;

                while (rowIndex < uniqueRows.Count)
                {
                    ReportModel row = uniqueRows[rowIndex];

                    bool showType = ShouldShowType(uniqueRows, rowIndex);
                    int count = GetTypeCount(uniqueRows, row.MonitoringTypeName);

                    if (showType)
                    {
                        table.Cell()
                            .RowSpan((uint)count)
                            .Element(TypeStyle)
                            .Text(row.MonitoringTypeName)
                            .Bold();
                    }

                    table.Cell()
                        .Element(ItemStyle)
                        .Text(row.ItemName);

                    if (_model.Dates != null)
                    {
                        int d = 0;

                        while (d < _model.Dates.Count)
                        {
                            string value = GetValue(
                                row.MonitoringTypeName,
                                row.ItemName,
                                _model.Dates[d]);

                            table.Cell().Element(ValueStyle).Text(delegate (TextDescriptor text)
                            {
                                if (value == "TICK")
                                {
                                    text.Span("✓").FontColor("#008000").Bold().FontSize(11);
                                }
                                else if (value == "CROSS")
                                {
                                    text.Span("✗").FontColor("#DC3545").Bold().FontSize(11);
                                }
                                else
                                {
                                    text.Span(value);
                                }
                            });

                            d++;
                        }
                    }

                    rowIndex++;
                }
            });
        }

        private List<ReportModel> GetUniqueRows()
        {
            List<ReportModel> list = new List<ReportModel>();

            if (_model.ReportData == null)
            {
                return list;
            }

            int i = 0;

            while (i < _model.ReportData.Count)
            {
                bool exists = false;

                int j = 0;

                while (j < list.Count)
                {
                    if (list[j].MonitoringTypeName == _model.ReportData[i].MonitoringTypeName &&
                        list[j].ItemName == _model.ReportData[i].ItemName)
                    {
                        exists = true;
                        break;
                    }

                    j++;
                }

                if (!exists)
                {
                    list.Add(_model.ReportData[i]);
                }

                i++;
            }

            return list;
        }

        private bool ShouldShowType(List<ReportModel> rows, int index)
        {
            int i = 0;

            while (i < index)
            {
                if (rows[i].MonitoringTypeName == rows[index].MonitoringTypeName)
                {
                    return false;
                }

                i++;
            }

            return true;
        }

        private int GetTypeCount(List<ReportModel> rows, string type)
        {
            int count = 0;
            int i = 0;

            while (i < rows.Count)
            {
                if (rows[i].MonitoringTypeName == type)
                {
                    count++;
                }

                i++;
            }

            return count;
        }

        private string GetValue(string type, string item, DateTime date)
        {
            if (_model.ReportData == null)
            {
                return "-";
            }

            int i = 0;

            while (i < _model.ReportData.Count)
            {
                ReportModel data = _model.ReportData[i];

                if (data.MonitoringTypeName == type &&
                    data.ItemName == item &&
                    data.EntryDate.Date == date.Date)
                {
                    if (!string.IsNullOrWhiteSpace(data.EntryMode))
                    {
                        if (data.EntryMode == "Checkbox")
                        {
                            if (data.IsChecked)
                            {
                                return "TICK";
                            }
                            else
                            {
                                return "CROSS";
                            }
                        }

                        if (data.EntryMode == "Value")
                        {
                            if (!string.IsNullOrWhiteSpace(data.ConfigValue))
                            {
                                return data.ConfigValue.Trim();
                            }
                            else
                            {
                                return "-";
                            }
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(data.ConfigValue))
                    {
                        return data.ConfigValue.Trim();
                    }

                    return "-";
                }

                i++;
            }

            return "-";
        }

        private string GetReportTitle()
        {
            if (string.IsNullOrEmpty(_model.ReportTitle))
            {
                return "Monitoring Report";
            }

            return _model.ReportTitle;
        }

        private string GetSelectedProductName()
        {
            if (!_model.SelectedProductId.HasValue || _model.ProductList == null)
            {
                return "-";
            }

            int i = 0;

            while (i < _model.ProductList.Count)
            {
                if (_model.ProductList[i].ProductId == _model.SelectedProductId.Value)
                {
                    return _model.ProductList[i].ProductName;
                }

                i++;
            }

            return "-";
        }

        private IContainer HeaderStyle(IContainer c)
        {
            return c.Border(1).Padding(2).AlignCenter().Background(Colors.Grey.Lighten3);
        }

        private IContainer TypeStyle(IContainer c)
        {
            return c.Border(1).Padding(2).Background(Colors.Grey.Lighten4);
        }

        private IContainer ItemStyle(IContainer c)
        {
            return c.Border(1).Padding(2);
        }

        private IContainer ValueStyle(IContainer c)
        {
            return c.Border(1).Padding(2).AlignCenter();
        }
    }
}