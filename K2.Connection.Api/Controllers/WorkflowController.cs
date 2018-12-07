using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace K2.Connection.Api.Controllers
{
    public class WorkflowController : ApiController
    {

        [HttpGet]
        [Route("")]
        public IHttpActionResult StartWorkflow()
        {
            return Ok();
        }

    }
}