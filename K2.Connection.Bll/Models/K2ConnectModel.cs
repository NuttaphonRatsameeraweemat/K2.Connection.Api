using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2.Connection.Bll.Models
{
    public class K2ConnectModel
    {
        public K2ConnectModel()
        {
            Impersonate = false;
            ImpersonateUser = string.Empty;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public int Port { get; set; }
        public string Url { get; set; }
        public string SecurityLabelName { get; set; }
        public bool Impersonate { get; set; }
        public string ImpersonateUser { get; set; }
    }
}
