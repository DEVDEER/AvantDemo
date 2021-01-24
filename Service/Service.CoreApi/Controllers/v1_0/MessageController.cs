namespace Service.CoreApi.Controllers.v1_0
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Provides messaging endpoints.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class MessageController : ControllerBase
    {
        #region methods

        /// <summary>
        /// Retrieves the amount of messages retrieved.
        /// </summary>
        /// <returns>The amount of messages.</returns>
        [HttpGet("Count")]
        public ActionResult<int> GetMessageCount()
        {
	        var x = Request.RouteValues;
            return 0;
        }

        #endregion
    }
}
