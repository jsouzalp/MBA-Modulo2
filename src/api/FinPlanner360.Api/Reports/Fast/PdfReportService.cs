using FastReport.Export.PdfSimple;
using System.Collections;
using System.Reflection;

namespace FinPlanner360.Api.Reports.Fast
{
    public static class PdfReportService
    {
        public static byte[] GenerateReportPDF(string reportFile, IEnumerable data, Dictionary<string, object> parameters)
        {
            var report = GenerateReport(reportFile, data, parameters);

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

        private static FastReport.Report GenerateReport(string reportFile, IEnumerable data, Dictionary<string, object> parameters)
        {
            FastReport.Report report = new FastReport.Report();
            report.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Reports", "Fast", "Files", $"{reportFile}.frx"));
            report.RegisterData(data, reportFile);

            foreach (var parameter in parameters ?? [])
            {
                report.SetParameterValue(parameter.Key, parameter.Value);
            }

            report.Prepare();
            return report;
        }

    }
}
