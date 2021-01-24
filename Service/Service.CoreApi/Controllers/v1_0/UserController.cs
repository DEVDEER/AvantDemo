namespace Service.CoreApi.Controllers.v1_0
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Azure.Cosmos;
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// </summary>
	[ApiController]
	[Produces("application/json")]
	[Route("api/v1/[controller]")]
	public class UserController : ControllerBase
	{
		#region member vars

		private readonly CosmosClient _cosmosClient;

		#endregion

		#region constructors and destructors

		public UserController(IConfiguration config)
		{
			Config = config;
			var connectionString = Config.GetConnectionString("CosmosDb");
			_cosmosClient = new CosmosClient(connectionString);
		}

		#endregion

		#region methods

		[HttpPost("CreateMany")]
		public async Task<ActionResult<bool>> GenerateUsersAsync([FromQuery] int amount)
		{
			await AddItemsToContainerAsync(amount);
			return true;
		}

		/// <summary>
		/// Retrieves all users from the database.
		/// </summary>
		/// <returns>The list of users.</returns>
		[HttpGet]
		public async Task<ActionResult<IEnumerable<UserModel>>> GetUsersAsync()
		{
			var connectionString = Config.GetConnectionString("CosmosDb");
			var databaseId = Config["App:DatabaseId"];
			var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
			var container = await database.Database.CreateContainerIfNotExistsAsync("users", "/lastName");
			var results = new List<UserModel>();
			var queryDefinition = new QueryDefinition("SELECT * FROM c");
			using (var iterator = container.Container.GetItemQueryIterator<UserModel>(queryDefinition))
			{
				while (iterator.HasMoreResults)
				{
					var currentResultSet = await iterator.ReadNextAsync();
					foreach (var item in currentResultSet)
					{
						results.Add(item);
					}
				}
			}
			return results.OrderBy(u => u.LastName)
				.ThenBy(u => u.FirstName)
				.ToList();
		}

		private async Task AddItemsToContainerAsync(int itemsToCreate)
		{
			var tasks = new List<Task>();			
			var databaseId = Config["App:DatabaseId"];
			var database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
			var container = await database.Database.CreateContainerIfNotExistsAsync("users", "/lastName");
			for (var i = 0; i < itemsToCreate; i++)
			{
				var user = new UserModel
				{
					Id = Guid.NewGuid().ToString(),
					FirstName = Guid.NewGuid()
						.ToString()
						.Substring(10),
					LastName = Guid.NewGuid()
						.ToString()
						.Substring(10)
				};
				tasks.Add(AddItemToContainerAsync(container.Container, user));
			}
			Task.WaitAll(tasks.ToArray());
		}

		private async Task AddItemToContainerAsync(Container container, UserModel user)
		{
			var exResponse = await container.CreateItemAsync(user, new PartitionKey(user.LastName));
		}

		#endregion

		#region properties

		private IConfiguration Config { get; }

		#endregion
	}
}
