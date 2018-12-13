using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2.Connection.Bll.Models
{
    public class TaskViewModel
    {
        public string AllocatedUser { get; set; }
        public string Data { get; set; }
        public int ID { get; set; }
        public int Step { get; set; }
        public string Folio { get; set; }
        public string SerialNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int OverDealDay { get; set; }
        public int WaitingDay { get; set; }
        public string Folder { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string RequesterName { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public int DataID { get; set; }
        public string Action { get; set; }
    }
}
