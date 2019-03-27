using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2.Connection.Bll.Models
{
    public class SmartObjectModel
    {
        public int ProcessSetId { get; set; }
        public int ProcessId { get; set; }
        public int ProcessInstancesId { get; set; }
        public string ProcessSetName { get; set; }
        public string Status { get; set; }
    }
}
