using K2.Connection.Bll.Models;
using System.Collections.Generic;

namespace K2.Connection.Bll.Interfaces
{
    /// <summary>
    /// The class of management k2 smartobject. 
    /// </summary>
    public interface ISmartObject
    {
        /// <summary>
        /// Get Data from Smartobject.
        /// </summary>
        /// <param name="model">The information smartobject and method name.</param>
        /// <returns></returns>
        Dictionary<string, string> GetSmartObject(SmartObjectModel model);
    }
}
