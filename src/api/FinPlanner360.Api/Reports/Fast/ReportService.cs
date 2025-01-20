using FastReport.Export.PdfSimple;
using System.Collections;
using System.Reflection;

namespace FinPlanner360.Api.Reports.Fast
{
    public static class ReportService
    {
        public static byte[] GenerateReportPDF(string reportFile, IEnumerable data)
        {
            var report = GenerateReport(reportFile, data);

            using MemoryStream ms = new();
            PDFSimpleExport pdfExport = new();
            pdfExport.Export(report, ms);
            ms.Flush();

            return ms.ToArray();
        }

        private static FastReport.Report GenerateReport(string reportFile, IEnumerable data)
        {
            FastReport.Report report = new FastReport.Report();
            report.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Reports","Fast", "Files", $"{reportFile}.frx"));
            report.RegisterData(data, reportFile);
            report.Prepare();
            return report;
        }
    }
}
