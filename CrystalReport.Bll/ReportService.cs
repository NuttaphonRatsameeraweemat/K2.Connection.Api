using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalReport.Bll
{
    public class ReportService
    {

        #region [Methods]

        public static DataTable ConvertListToDatatable<T>(List<T> dataList)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in dataList)
            {
                var values = new object[props.Count];
                for (int i = 0; i < props.Count; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static void ExportExcel(ReportDocument reportDocument, string reportFileName = "")
        {
            ExportOptions options = new ExportOptions();
            options.ExportFormatType = ExportFormatType.Excel;
            reportDocument.ExportToHttpResponse(options, System.Web.HttpContext.Current.Response, false, reportFileName + '_' + DateTime.Now.ToString("yyyy-MM-dd_HHmmss",
                      CultureInfo.CreateSpecificCulture("en-US")));
        }

        public static void ExportWord(ReportDocument reportDocument, string reportFileName = "")
        {
            ExportOptions options = new ExportOptions();
            options.ExportFormatType = ExportFormatType.WordForWindows;
            reportDocument.ExportToHttpResponse(options, System.Web.HttpContext.Current.Response, false, reportFileName + '_' + DateTime.Now.ToString("yyyy-MM-dd_HHmmss",
                      CultureInfo.CreateSpecificCulture("en-US")));
        }

        public static void ExportPdf(ReportDocument reportDocument, string reportFileName = "", bool noStampTime = false)
        {
            ExportOptions options = new ExportOptions();
            options.ExportFormatType = ExportFormatType.PortableDocFormat;
            if (noStampTime)
                reportDocument.ExportToHttpResponse(options, System.Web.HttpContext.Current.Response, false, reportFileName);
            else
                reportDocument.ExportToHttpResponse(options, System.Web.HttpContext.Current.Response, false, reportFileName + '_' + DateTime.Now.ToString("yyyy-MM-dd_HHmmss",
                          CultureInfo.CreateSpecificCulture("en-US")));
        }

        public static void ExportPdfToDisk(ReportDocument reportDocument, string process, string reportFileName = "")
        {
            string path = GetReportDirectory(reportFileName, process);
            reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, path);
        }

        public static string GetReportDirectory(string reportFileName, string process)
        {
            string path = Path.Combine(ConfigurationManager.AppSettings["DocumentFilePath"], "Report", process);
            path = System.Web.HttpContext.Current.Server.MapPath(path);
            string fileName = reportFileName + ".pdf";
            return Path.Combine(path, fileName);
        }

        #endregion
        
    }
}
