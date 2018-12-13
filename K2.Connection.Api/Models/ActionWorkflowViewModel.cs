using K2.Connection.Bll.Models;
using System.Collections.Generic;

namespace K2.Connection.Api.Models
{
    public class ActionWorkflowViewModel
    {
        public K2ProfileModel K2Connect { get; set; }
        public string SerialNumber { get; set; }
        public string Action { get; set; }
        public string AllocatedUser { get; set; }
        public Dictionary<string, object> Datafields { get; set; }
    }
}