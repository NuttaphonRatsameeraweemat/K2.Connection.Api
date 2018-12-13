using K2.Connection.Bll.Models;
using System.Collections.Generic;

namespace K2.Connection.Api.Models
{
    public class StartWorkflowViewModel
    {
        public K2ProfileModel K2Connect { get; set; }
        public string ProcessName { get; set; }
        public string Folio { get; set; }
        public Dictionary<string, object> DataFields { get; set; }
    }
}