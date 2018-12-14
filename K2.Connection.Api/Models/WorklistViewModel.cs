using K2.Connection.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace K2.Connection.Api.Models
{
    public class WorklistViewModel
    {
        public K2ProfileModel K2Connect { get; set; }
        public string FromUser { get; set; }
    }
}