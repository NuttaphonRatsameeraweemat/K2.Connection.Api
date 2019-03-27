using K2.Connection.Bll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace K2.Connection.Api.Controllers
{
    public class SmartObjectController : ApiController
    {
        #region [Fields]

        /// <summary>
        /// The workflow manager provides workflow functionality.
        /// </summary>
        private readonly ISmartObject _smartObject;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="SmartObjectController" /> class.
        /// </summary>
        /// <param name="smartObject"></param>
        public SmartObjectController(ISmartObject smartObject)
        {
            _smartObject = smartObject;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("GetSmartObject")]
        public IHttpActionResult GetSmartObject()
        {
            return Ok(_smartObject.GetSmartObject());
        }

        #endregion
    }
}
