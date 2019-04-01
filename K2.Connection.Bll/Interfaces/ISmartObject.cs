using K2.Connection.Bll.Models;
using SourceCode.SmartObjects.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2.Connection.Bll.Interfaces
{
    public interface ISmartObject
    {
        List<SmartObjectModel> GetSmartObject(SmartObjectModel model);
    }
}
