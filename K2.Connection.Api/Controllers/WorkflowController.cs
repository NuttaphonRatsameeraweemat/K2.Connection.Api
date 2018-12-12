using K2.Connection.Api.Models;
using K2.Connection.Bll.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace K2.Connection.Api.Controllers
{
    public class WorkflowController : ApiController
    {

        #region [Fields]

        /// <summary>
        /// The workflow manager provides workflow functionality.
        /// </summary>
        private readonly IWorkflow _workflow;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="WorkflowController" /> class.
        /// </summary>
        /// <param name="workflow"></param>
        public WorkflowController(IWorkflow workflow)
        {
            _workflow = workflow;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("StartWorkflow")]
        public IHttpActionResult StartWorkflow(WorkflowViewModel model)
        {
            _workflow.Initial(model.K2Connect, model.Management);
            int processInstantId = _workflow.StartWorkflow(model.ProcessName, model.Folio, model.DataFields);
            return Ok(processInstantId);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Test() => Ok();

        #endregion



    }
}