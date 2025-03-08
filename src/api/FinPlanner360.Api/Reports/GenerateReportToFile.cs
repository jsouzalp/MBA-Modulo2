using DocumentFormat.OpenXml.Spreadsheet;
using FinPlanner360.Api.Reports.Closed_Xml;
using FinPlanner360.Api.Reports.Fast;

namespace FinPlanner360.Api.Reports
{
    public static class GenerateReportToFile
    {
        public static (byte[] FileBytes, string ContentType, string FileName) Generate<T>(string fileType, string reportName, IEnumerable<T> transactionsReport, Dictionary<string, object> parameters)
        {
            byte[] fileBytes = null;
            string contentType = null;
            string fileName = null;

            switch (fileType.ToLower())
            {
                case "pdf":
                    fileBytes = PdfReportService.GenerateReportPDF(reportName, transactionsReport, parameters);
                    contentType = "application/pdf";
                    fileName = $"{reportName}.pdf";
                    break;

                case "xlsx":
                    fileBytes = XlsReportService.GenerateXlsxBytes(reportName, transactionsReport);
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    fileName = $"{reportName}.xlsx";
                    break;
            }

            return (fileBytes, contentType, fileName);
        }

    }
}