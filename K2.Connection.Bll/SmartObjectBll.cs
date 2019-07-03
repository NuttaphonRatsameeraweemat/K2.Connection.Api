using K2.Connection.Bll.Content;
using K2.Connection.Bll.Interfaces;
using K2.Connection.Bll.Models;
using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.SmartObjects.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2.Connection.Bll
{
    /// <summary>
    /// The class of management k2 smartobject. 
    /// </summary>
    public class SmartObjectBll : ISmartObject
    {

        #region [Methods]

        /// <summary>
        /// Get Data from Smartobject.
        /// </summary>
        /// <param name="model">The information smartobject and method name.</param>
        /// <returns></returns>
        public Dictionary<string, string> GetSmartObject(SmartObjectModel model)
        {
            var result = new Dictionary<string, string>();
            var soServer = GetServer();
            using (soServer.Connection)
            {
                var workflowSmartObject = WorkflowSmartObject(soServer, model);

                var smartObject = soServer.ExecuteList(workflowSmartObject);
                result = this.ConvertToModel(smartObject.SmartObjectsList);
            }
            return result;
        }

        /// <summary>
        /// Convert Smartobject collection to dictionary.
        /// </summary>
        /// <param name="smItem">The SmartObjectCollection.</param>
        /// <returns></returns>
        private Dictionary<string, string> ConvertToModel(SmartObjectCollection smItem)
        {
            var result = new Dictionary<string, string>();
            foreach (SmartObject item in smItem)
            {
                foreach (SmartProperty property in item.Properties)
                {
                    result.Add(property.Name, property.Value);
                }
            }
            return result;
        }

        /// <summary>
        /// Initial Smartobject name and method name execute.
        /// </summary>
        /// <param name="soServer">The Smartobject server connection.</param>
        /// <param name="model">The information smartobject and method name.</param>
        /// <returns></returns>
        private SmartObject WorkflowSmartObject(SmartObjectClientServer soServer, SmartObjectModel model)
        {
            var result = soServer.GetSmartObject(model.SmartObjectName);
            result.MethodToExecute = model.ExecuteMethodName;
            return result;
        }

        /// <summary>
        /// Get Connection String Connect to K2.
        /// </summary>
        /// <returns>The connection string.</returns>
        private string GetConnectionString()
        {
            var hostServerConnectionString = new SCConnectionStringBuilder
            {
                Host = ConfigurationManager.AppSettings[ConstantValueService.K2_URL],
                Port = Convert.ToUInt32(ConfigurationManager.AppSettings[ConstantValueService.K2_MANAGEMENT_PORT]),
                IsPrimaryLogin = true,
                Integrated = true,
                UserID = ConfigurationManager.AppSettings[ConstantValueService.K2_ADMINUSERNAME],
                Password = ConfigurationManager.AppSettings[ConstantValueService.K2_ADMINPASSWORD],
                SecurityLabelName = ConfigurationManager.AppSettings[ConstantValueService.K2_SECURITYLABEL]
            };
            return hostServerConnectionString.ToString();
        }

        /// <summary>
        /// Create Connection to K2 Server.
        /// </summary>
        /// <returns></returns>
        private SmartObjectClientServer GetServer()
        {
            var soServer = new SmartObjectClientServer();
            soServer.CreateConnection();
            soServer.Connection.Open(this.GetConnectionString());
            return soServer;
        }

        #endregion

    }
}
