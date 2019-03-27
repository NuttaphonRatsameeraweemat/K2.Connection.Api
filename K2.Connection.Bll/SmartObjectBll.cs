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
    public class SmartObjectBll : ISmartObject
    {

        public List<SmartObjectModel> GetSmartObject()
        {
            var result = new List<SmartObjectModel>();
            var soServer = GetServer();
            using (soServer.Connection)
            {
                var workflowSmartObject = WorkflowSmartObject(soServer);

                var smartObject = soServer.ExecuteList(workflowSmartObject);
                result = this.ConvertToModel(smartObject.SmartObjectsList);
            }
            return result;
        }

        private List<SmartObjectModel> ConvertToModel(SmartObjectCollection smItem)
        {
            var result = new List<SmartObjectModel>();
            foreach (SmartObject item in smItem)
            {
                result.Add(new SmartObjectModel
                {
                    ProcessId = Convert.ToInt32(item.Properties["PROCESS_ID"].Value.ToString()),
                });
            }
            return result;
        }

        private SmartObject WorkflowSmartObject(SmartObjectClientServer soServer, int retry = 1)
        {
            var result = new SmartObject();
            result = soServer.GetSmartObject("GetTest");
            if (result == null && retry <= 2)
            {
                System.Threading.Thread.Sleep(1500);
                result = WorkflowSmartObject(soServer, ++retry);
            }
            var methodName = "All_Process_Instances";
            result.MethodToExecute = methodName;
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
                SecurityLabelName = "K2"
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

    }
}
