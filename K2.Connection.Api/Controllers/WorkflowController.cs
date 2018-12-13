﻿using K2.Connection.Api.Models;
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
        public IHttpActionResult StartWorkflow(StartWorkflowViewModel model)
        {
            _workflow.Initial(model.K2Connect);
            return Ok(_workflow.StartWorkflow(model.ProcessName, model.Folio, model.DataFields));
        }

        [HttpPost]
        [Route("ActionWorkflow")]
        public IHttpActionResult ActionWorkflow(ActionWorkflowViewModel model)
        {
            _workflow.Initial(model.K2Connect);
            return Ok(_workflow.ActionWorkflow(model.SerialNumber, model.Action, model.Datafields, model.AllocatedUser));
        }

        [HttpPost]
        [Route("GetWorkList")]
        public IHttpActionResult GetWorkList()
        {
            return Ok();
        }

        #endregion
        
    }
}