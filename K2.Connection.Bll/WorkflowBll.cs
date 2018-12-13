using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.Workflow.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using K2.Connection.Bll.Interfaces;
using K2.Connection.Bll.Models;
using K2.Connection.Bll.Content;
using System.Linq;

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
        public void Initial(K2ProfileModel model)
        {
            _model = SetValue(model.UserName, model.Password, model.UserType, model.ImpersonateUser);
            if (model.Management)
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
        public int StartWorkflow(string processName, string folio, Dictionary<string, object> dataFields)
        {
            ProcessInstance processInstance = _connection.CreateProcessInstance(processName);
            processInstance.Folio = folio;

            //Default Task URL
            dataFields.Add("TaskUrl", ConfigurationManager.AppSettings[ConstantValueService.K2_TASKURL]);

            if (dataFields != null)
            {
                foreach (KeyValuePair<string, object> current in dataFields)
                {
                    processInstance.DataFields[current.Key].Value = current.Value;
                }
            }
            _connection.StartProcessInstance(processInstance);

            return processInstance.ID;
        }

        /// <summary>
        /// Action workflow item.
        /// </summary>
        /// <param name="serialNumber">The identity workflow.</param>
        /// <param name="action">The action workflow value.</param>
        /// <param name="datafields">The data fields workflow.</param>
        /// <param name="allocatedUser">The allocated user.</param>
        /// <returns></returns>
        public string ActionWorkflow(string serialNumber, string action, Dictionary<string, object> datafields, string allocatedUser)
        {
            try
            {
                bool isSharingItem = false;
                if ((!string.IsNullOrEmpty(allocatedUser) && !string.IsNullOrEmpty(_model.K2Profile.UserName))
                    && !string.Equals(allocatedUser, _model.K2Profile.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    isSharingItem = true;
                }
                WorklistItem worklistItem;
                if (isSharingItem)
                {
                    worklistItem = _connection.OpenSharedWorklistItem(allocatedUser, _model.K2Profile.UserName, serialNumber);
                }
                else worklistItem = _connection.OpenWorklistItem(serialNumber, "ASP", true);
                if (worklistItem == null)
                {
                    throw new ArgumentNullException(ConstantValueService.MSG_ERR_CANNOT_FOUND_WORKLISTITEM);
                }
                foreach (var datafield in datafields)
                {
                    worklistItem.ProcessInstance.DataFields[datafield.Key].Value = datafield.Value;
                    worklistItem.ProcessInstance.Update();
                }
                worklistItem.Actions[action].Execute();

                //Force to Reject for round robin case.
                if (string.Equals(action, ConstantValueService.K2_REJECT, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        worklistItem.GotoActivity(ConstantValueService.K2_REJECT);
                    }
                    catch (Exception)
                    {
                        /* Ignore this exception */
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
            return ConstantValueService.MSG_WORKFLOW_ACTION_COMPLETE;
        }

        public List<TaskViewModel> GetWorkList(string fromUser)
        {
            List<TaskViewModel> result = new List<TaskViewModel>();
            var processFolder = ConfigurationManager.AppSettings["K2ProcessFolder"];
            if (processFolder == null)
            {
                processFolder = "DS";
            }

            for (int i = 0; i < 2; i++)
            {
                try
                {
                    Worklist taskList;

                    WorklistCriteria worklistCriteria = new WorklistCriteria();
                    worklistCriteria.AddFilterField(0, WCCompare.NotEqual, 2);
                    worklistCriteria.AddFilterField(WCField.ProcessFolder, WCCompare.Equal, processFolder);

                    //For Share Worklist Items
                    worklistCriteria.AddFilterField(WCLogical.AndBracket, WCField.WorklistItemOwner,
                                      WCWorklistItemOwner.Me.ToString(), WCCompare.Equal, 0);
                    worklistCriteria.AddFilterField(WCLogical.Or, WCField.WorklistItemOwner,
                                      WCWorklistItemOwner.Other.ToString(), WCCompare.Equal, 0);

                    taskList = _connection.OpenWorklist(worklistCriteria);
                    result = ConvertTaskList(taskList);

                    if (result.Count <= 0)
                    {
                        throw new Exception("No Result Found");
                    }

                    if (!string.IsNullOrEmpty(fromUser))
                    {
                        var formUserK2Format = ConstantValueService.K2_PREFIX + fromUser;
                        result = result.Where(m => string.Equals(m.AllocatedUser, formUserK2Format, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                }
                catch (Exception ex)
                {
                    if (i == 2)
                    {
                        throw ex;
                    }
                    System.Threading.Thread.Sleep(50);
                    this.ReConnection();
                }
            }

            return result;
        }

        /// <summary>
        /// Set value for open connection k2.
        /// </summary>
        /// <returns></returns>
        private K2ConnectModel SetValue(string userName, string password, string userType, string impersonateUser)
        {
            K2ConnectModel result = new K2ConnectModel
            {
                K2Profile = new K2ProfileModel
                {
                    UserName = userName,
                    Password = password,
                    UserType = userType
                },
                Port = Convert.ToInt32(ConfigurationManager.AppSettings[ConstantValueService.K2_PORT]),
                Url = ConfigurationManager.AppSettings[ConstantValueService.K2_URL],
                SecurityLabelName = ConfigurationManager.AppSettings[ConstantValueService.K2_SECURITYLABEL]
            };
            if (!string.IsNullOrEmpty(impersonateUser))
            {
                result.K2Profile.Impersonate = true;
                result.K2Profile.ImpersonateUser = impersonateUser;
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
                string connectionString = this.GetConnectionString(model);
                connection.Open(model.Url, connectionString);
                if (model.K2Profile.Impersonate)
                {
                    connection.ImpersonateUser(model.SecurityLabelName + ":" + model.K2Profile.ImpersonateUser);
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
        /// Reconnection k2.
        /// </summary>
        private void ReConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
            }
            _connection = Connection(_model);
        }

        /// <summary>
        /// Convert work list item to task list viewmodel.
        /// </summary>
        /// <param name="worklist">The worklist item.</param>
        /// <returns></returns>
        private List<TaskViewModel> ConvertTaskList(Worklist worklist)
        {
            List<TaskViewModel> taskList = new List<TaskViewModel>();
            foreach (WorklistItem item in worklist)
            {
                taskList.Add(this.ConvertTask(item));
            }
            return taskList;
        }

        /// <summary>
        /// Convert work item to task view model.
        /// </summary>
        /// <param name="item">The work item.</param>
        /// <returns></returns>
        private TaskViewModel ConvertTask(WorklistItem item)
        {
            TaskViewModel task = new TaskViewModel
            {
                Action = item.Actions.ToString(),
                AllocatedUser = item.AllocatedUser,
                DataID = UtilityService.DatafieldToInt(item.ProcessInstance.DataFields["DataID"]),
                Step = UtilityService.DatafieldToInt(item.ProcessInstance.DataFields["CurrentStep"]),
                StartDate = item.ProcessInstance.StartDate,
                Folder = item.ProcessInstance.Folder,
                Name = item.ProcessInstance.Name,
                FullName = item.ProcessInstance.FullName,
                Folio = item.ProcessInstance.Folio,
                ReceivedDate = UtilityService.DatafieldToDateTime(item.ProcessInstance.DataFields["ReceivedDate"], item.ProcessInstance.StartDate),
                RequesterName = UtilityService.DatafieldToString(item.ProcessInstance.DataFields["Requester"]),
                ProcessCode = UtilityService.DatafieldToString(item.ProcessInstance.DataFields["ProcessCode"])
            };
            task.Data = $"{item.Data}&ProcessCode={UtilityService.DatafieldToString(item.ProcessInstance.DataFields["ProcessCode"])}" +
                        $"&DataID={UtilityService.DatafieldToString(item.ProcessInstance.DataFields["DataID"])}";

            string currentStep = UtilityService.DatafieldToString(item.ProcessInstance.DataFields["CurrentStep"]);
            if (!string.IsNullOrEmpty(currentStep))
            {
                task.Data = string.Format(task.Data + "&Step={0}", currentStep);
            }

            return task;
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

            switch (model.K2Profile.UserType)
            {
                case ConstantValueService.USERTYPE_EMPLOYEE:
                    result = string.Format(ConfigurationManager.ConnectionStrings[ConstantValueService.K2_WORKFLOWEMPLOYEE].ToString(),
                                           model.Url, model.Port, model.SecurityLabelName, model.K2Profile.UserName, model.K2Profile.Password);
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
