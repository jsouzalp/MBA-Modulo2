using ClosedXML.Excel;

namespace FinPlanner360.Api.Reports.Closed_Xml
{
    public static class ReportService
    {
        public static byte[] GenerateXlsxBytes<T>(string sheetName, IEnumerable<T> list)
        {
            using var workbook = new XLWorkbook();
            var sheet = workbook.Worksheets.Add(sheetName);

            var properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                sheet.Cell(1, i + 1).Value = properties[i].Name;
            }

            int row = 2;
            foreach (var item in list)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var value = properties[i].GetValue(item);
                    sheet.Cell(row, i + 1).Value = value?.ToString();
                }
                row++;
            }

            using var memory = new MemoryStream();
            workbook.SaveAs(memory);
            return memory.ToArray();
        }

    }
}
