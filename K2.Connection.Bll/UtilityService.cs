using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SourceCode.Workflow.Client;

namespace K2.Connection.Bll
{
    /// <summary>
    /// The Utility function.
    /// </summary>
    public class UtilityService
    {

        #region [Methods]

        /// <summary>
        /// Convert data field to string.
        /// </summary>
        /// <param name="data">The data field value.</param>
        /// <returns></returns>
        public static string DatafieldToString(DataField data)
        {

            string result = string.Empty;
            if (data != null && data.Value != null)
            {
                result = data.Value.ToString();
            }
            return result;
        }

        /// <summary>
        /// Convert data field to int.
        /// </summary>
        /// <param name="data">The data field value.</param>
        /// <returns></returns>
        public static int DatafieldToInt(DataField data)
        {
            int result = 0;
            if (data != null && data.Value != null)
            {
                result = int.TryParse(data.Value.ToString(), out int temp) ? temp : 0;
            }
            return result;
        }

        /// <summary>
        /// Convert data field to datetime.
        /// </summary>
        /// <param name="data">The data field value.</param>
        /// <param name="startDate">The startdate for set receive date when datafield is null.</param>
        /// <returns></returns>
        public static DateTime DatafieldToDateTime(DataField data, DateTime? startDate = null)
        {
            DateTime result = new DateTime();
            if (startDate != null)
            {
                result = startDate.Value;
            }
            if (data != null && data.Value != null)
            {
                result = Convert.ToDateTime(data.Value);
            }
            return result;
        }

        #endregion

    }
}
