using ClosedXML.Excel;
using System.ComponentModel.DataAnnotations;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FinPlanner360.Api.Reports.Closed_Xml
{
    public static class XlsReportService
    {
        public static byte[] GenerateXlsxBytes<T>(string sheetName, IEnumerable<T> list)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(sheetName);

            var properties = typeof(T).GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                var displayAttribute = properties[i].GetCustomAttributes(typeof(DisplayAttribute), true)
                                                    .FirstOrDefault() as DisplayAttribute;

                sheet.Cell(1, i + 1).Value = displayAttribute?.Name ?? properties[i].Name;
            }

            int row = 2;
            foreach (var item in list)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    var cell = sheet.Cell(row, i + 1);
                    cell.Value = value?.ToString();

                    if (IsNumeric(properties[i].PropertyType))
                    {
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                    }
                }
                row++;
            }

            using var memory = new MemoryStream();
            workbook.SaveAs(memory);
            return memory.ToArray();
        }

        private static bool IsNumeric(Type type)
        {
            return type == typeof(decimal) || 
                type == typeof(double) ||
                type == typeof(byte) ||
                type == typeof(short) ||
                type == typeof(int) ||
                type == typeof(long);
        }

    }
}
