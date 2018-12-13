using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2.Connection.Bll.Content
{
    public class ConstantValueService
    {
        /// <summary>
        /// The key validate case user type connect to k2.
        /// </summary>
        public const string USERTYPE_EMPLOYEE = "EMPLOYEE";
        
        /// <summary>
        /// The key of activity k2 approve.
        /// </summary>
        public const string K2_APPROVE = "Approve";
        /// <summary>
        /// The key of activity k2 reject.
        /// </summary>
        public const string K2_REJECT = "Reject";
        /// <summary>
        /// The key of activity k2 cancel.
        /// </summary>
        public const string K2_CANCEL = "Cancel";

        /// <summary>
        /// The key name of webconfig k2 url.
        /// </summary>
        public const string K2_URL = "K2Url";
        /// <summary>
        /// The key name of webconfig k2 port.
        /// </summary>
        public const string K2_PORT = "K2WorkflowPort";
        /// <summary>
        /// The key name of webconifg k2 security label.
        /// </summary>
        public const string K2_SECURITYLABEL = "K2SecurityLabel";
        /// <summary>
        /// The key name of webconifg k2 admin username.
        /// </summary>
        public const string K2_ADMINUSERNAME = "K2Admin";
        /// <summary>
        /// The key name of webconifg k2 admin password.
        /// </summary>
        public const string K2_ADMINPASSWORD = "K2AdminPassword";
        /// <summary>
        /// The key name of webconfig k2 work flow employee.
        /// </summary>
        public const string K2_WORKFLOWEMPLOYEE = "K2WorkflowEmployee";
        /// <summary>
        /// The key name of webconfig k2 work flow iis.
        /// </summary>
        public const string K2_WORKFLOWIIS = "K2WorkflowIIS";
        /// <summary>
        /// The key name of webconfig k2 task url.
        /// </summary>
        public const string K2_TASKURL = "K2TaskUrl";
        /// <summary>
        /// The K2 prefix parameter.
        /// </summary>
        public const string K2_PREFIX = "K2:";

        /// <summary>
        /// The workflow complete message.
        /// </summary>
        public const string MSG_WORKFLOW_ACTION_COMPLETE = "Workflow Action Complete.";

        /// <summary>
        /// The Error message worklist item is null.
        /// </summary>
        public const string MSG_ERR_CANNOT_FOUND_WORKLISTITEM = "Not found work list item.";

    }
}
