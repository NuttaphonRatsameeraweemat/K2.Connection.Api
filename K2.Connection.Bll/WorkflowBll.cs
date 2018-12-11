using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Workflow.Client;
using System;
using System.Configuration;
using K2.Connection.Bll.Interfaces;
using K2.Connection.Bll.Models;
using K2.Connection.Bll.Content;

namespace K2.Connection.Bll
{
    public class WorkflowBll : IWorkflow
    {

        #region [Fields]

        /// <summary>
        /// The connection k2 value.
        /// </summary>
        private SourceCode.Workflow.Client.Connection _connection;
        /// <summary>
        /// The connectionmanagement for k2.
        /// </summary>
        private SourceCode.Workflow.Management.WorkflowManagementServer _connectionManagement;
        /// <summary>
        /// The value of json for connection k2.
        /// </summary>
        private K2ConnectModel _model;

        #endregion


        #region [Methods]

        /// <summary>
        /// Initial for k2 connection.
        /// </summary>
        /// <param name="model"></param>
        public void Initial(K2ConnectModel model, bool management)
        {
            _model = SetValue(model.UserName, model.Password, model.Port, model.ImpersonateUser);
            if (management)
            {
                _connectionManagement = this.ConnectionManagement(_model);
            }
            else _connection = this.Connection(_model);
        }

        /// <summary>
        /// Start workflow process.
        /// </summary>
        /// <param name="processName">The workflow process name.</param>
        /// <param name="folio">The title workflow.</param>
        /// <param name="dataFields">The data fields workflow.</param>
        /// <returns>process instance id.</returns>
        public int StartWorkflow(string processName, string folio, System.Collections.Generic.Dictionary<string, object> dataFields)
        {
            ProcessInstance processInstance = _connection.CreateProcessInstance(processName);
            processInstance.Folio = folio;

            //Default Task URL
            dataFields.Add("TaskUrl", ConfigurationManager.AppSettings[ConstantValueService.K2_TASKURL]);

            if (dataFields != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, object> current in dataFields)
                {
                    processInstance.DataFields[current.Key].Value = current.Value;
                }
            }
            _connection.StartProcessInstance(processInstance);

            return processInstance.ID;
        }

        /// <summary>
        /// Set value for open connection k2.
        /// </summary>
        /// <returns></returns>
        private K2ConnectModel SetValue(string userName, string password, int port, string impersonateUser)
        {
            K2ConnectModel result = new K2ConnectModel
            {
                UserName = userName,
                Password = password,
                Port = port,
                Url = ConfigurationManager.AppSettings[ConstantValueService.K2_URL],
                SecurityLabelName = ConfigurationManager.AppSettings[ConstantValueService.K2_SECURITYLABEL]
            };
            if (!string.IsNullOrEmpty(impersonateUser))
            {
                result.Impersonate = true;
                result.ImpersonateUser = impersonateUser;
            }
            return result;
        }

        /// <summary>
        /// Open connection k2.
        /// </summary>
        /// <param name="model">The value for connect k2.</param>
        /// <param name="retry">The round of try to connect k2.</param>
        /// <returns></returns>
        private SourceCode.Workflow.Client.Connection Connection(K2ConnectModel model, int retry = 0)
        {
            var connection = new SourceCode.Workflow.Client.Connection();
            int retryNum = retry;
            try
            {
                uint port2 = Convert.ToUInt32(model.Port);
                connection = new SourceCode.Workflow.Client.Connection();
                string connectionString = this.GetConnectionString(model);
                connection.Open(model.Url, connectionString);
                if (model.Impersonate)
                {
                    connection.ImpersonateUser(model.SecurityLabelName + ":" + model.ImpersonateUser);
                }

            }
            catch (System.Exception ex)
            {
                connection = null;
                retryNum++;

                if (retryNum == 2)
                {
                    //AppLogService.Log(ex.ToString(), "");
                    throw new System.Exception("Unable to create connection (retry:" + retryNum.ToString() + ") : " + ex.ToString());
                }
                else
                {
                    System.Threading.Thread.Sleep(50);
                    return Connection(model, retryNum);
                }
            }
            return connection;
        }

        /// <summary>
        /// Open connection management k2.
        /// </summary>
        /// <param name="model">The value for connect k2.</param>
        /// <returns></returns>
        private SourceCode.Workflow.Management.WorkflowManagementServer ConnectionManagement(K2ConnectModel model)
        {
            var managementServer = new SourceCode.Workflow.Management.WorkflowManagementServer();
            try
            {
                model.Port = 5555;
                String connectionString = this.GetConnectionString(model);
                managementServer.Open(connectionString);

            }
            catch (System.Exception ex)
            {
                //AppLogService.Log(ex.ToString(), "");
                throw new System.Exception("Unable to create management connection : " + ex.ToString());
            }
            return managementServer;
        }

        /// <summary>
        /// Get k2 url connection string.
        /// </summary>
        /// <param name="model">The value for connect k2.</param>
        /// <returns></returns>
        private string GetConnectionString(K2ConnectModel model)
        {
            string result = string.Empty;

            switch (model.UserType)
            {
                case ConstantValueService.UserTypeEmployee:
                    result = string.Format(ConfigurationManager.ConnectionStrings[ConstantValueService.K2_WORKFLOWEMPLOYEE].ToString(), 
                                           model.Url, model.Port, model.SecurityLabelName, model.UserName, model.Password);
                    break;
                default:
                    result = string.Format(ConfigurationManager.ConnectionStrings[ConstantValueService.K2_WORKFLOWIIS].ToString(), 
                                           model.Url, model.Port, model.SecurityLabelName);
                    break;
            }

            return result;
        }

        #endregion

    }
}
