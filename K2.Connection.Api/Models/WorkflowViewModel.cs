using K2.Connection.Bll.Models;
using System.Collections.Generic;

namespace K2.Connection.Api.Models
{
    public class WorkflowViewModel
    {
        public K2ConnectModel K2Connect { get; set; }
        public bool Management { get; set; }
        public string ProcessName { get; set; }
        public string Folio { get; set; }
        public Dictionary<string, object> DataFields { get; set; }
    }
}