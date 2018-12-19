using CrystalReport.Bll.Interfaces;
using CrystalReport.Bll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrystalReport.Api.Controllers
{
    public class CashAdvanceController : ApiController
    {

        #region [Fields]

        /// <summary>
        /// The cash advance function.
        /// </summary>
        private readonly ICashAdvance _cashAdvance;

        #endregion

        #region [Constructors]

        /// <summary>
        ///  Initializes a new instance of the <see cref="CashAdvanceController" /> class.
        /// </summary>
        /// <param name="cashAdvance"></param>
        public CashAdvanceController(ICashAdvance cashAdvance)
        {
            _cashAdvance = cashAdvance;
        }

        #endregion

        #region [Methods]

        [HttpPost]
        [Route("SubmitReport")]
        public void SubmitReport(CashAdvanceModel model)
        {
            _cashAdvance.SubmitReport(model);
        }

        #endregion

    }
}
