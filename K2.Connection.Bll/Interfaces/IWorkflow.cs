using K2.Connection.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2.Connection.Bll.Interfaces
{
    public interface IWorkflow
    {
        void Initial(K2ProfileModel model);
        int StartWorkflow(string processName, string folio, System.Collections.Generic.Dictionary<string, object> dataFields);
        string ActionWorkflow(string serialNumber, string action, Dictionary<string, object> datafields, string allocatedUser);
    }
}
