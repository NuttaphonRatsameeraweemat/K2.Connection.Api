using CrystalDecisions.CrystalReports.Engine;
using CrystalReport.Bll.Interfaces;
using CrystalReport.Bll.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CrystalReport.Bll
{
    public class CashAdvanceBll : ICashAdvance
    {

        #region [Fields]

        /// <summary>
        /// The report name.
        /// </summary>
        public const string REPORT_NAME = "CashAdvance";

        #endregion

        #region [Constructors]

        /// <summary>
        /// Initializes a new instance of the <see cref="CashAdvanceBll" /> class.
        /// </summary>
        public CashAdvanceBll()
        {

        }

        #endregion

        #region [Methods]

        public void SubmitReport(CashAdvanceModel model)
        {

            List<CashAdvanceModel> result = new List<CashAdvanceModel>();
            DataTable getDataReport = new DataTable();
            using (var rd = new ReportDocument())
            {
                rd.Load(Path.Combine(HttpContext.Current.Server.MapPath($"~/Reports/{REPORT_NAME}"), $"{REPORT_NAME}Report.rpt"));
                //Parameter
                result.Add(model);
                //Convert Data To Report
                getDataReport = ReportService.ConvertListToDatatable(result);

                getDataReport.TableName = REPORT_NAME;
                rd.SetDataSource(getDataReport);

                //Encoding FileName
                ReportService.ExportPdf(rd, REPORT_NAME);
            }
            GC.Collect();
        }

        #endregion

    }
}
