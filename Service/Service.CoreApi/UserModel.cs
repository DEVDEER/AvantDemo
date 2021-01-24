namespace Service.CoreApi
{
	using Newtonsoft.Json;
	using System;
	using System.Linq;

	public class UserModel
	{
		#region properties

		[JsonProperty("firstName")]
		public string FirstName { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("lastName")]
		public string LastName { get; set; }

		#endregion
	}
}
