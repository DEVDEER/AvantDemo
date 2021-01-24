namespace Service.CoreApi.Controllers.v1_0
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using System;
	using System.Linq;

	/// <summary>
	/// Provides ping endpoints.
	/// </summary>
	[ApiController]
	[Produces("application/json")]
	[Route("api/v1/[controller]")]
	public class PingController : ControllerBase
	{
		#region methods

		/// <summary>
		/// Retrieves the request headers as they arrived at the API.
		/// </summary>
		/// <returns>The headers.</returns>
		[HttpGet("Headers")]
		
		public ActionResult<IHeaderDictionary> RetrieveHeaders()
		{
			return Ok(Request.Headers);
		}

		#endregion
	}
}
