namespace Ui.EventConsole
{
	using Azure.Messaging.EventHubs;
	using Azure.Messaging.EventHubs.Consumer;
	using Azure.Messaging.EventHubs.Producer;
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Linq;
	using System.Text;
	using System.Text.Json;
	using System.Threading;
	using System.Threading.Tasks;

	internal class Program
	{
		#region constants

		private static Guid _clientId = Guid.NewGuid();

		private static IConfigurationRoot _config;

		private static readonly CancellationTokenSource TokenSource = new CancellationTokenSource();

		#endregion

		#region methods

		private static void Main(string[] args)
		{
			var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{env}.json", true, true)
				.AddEnvironmentVariables();
			_config = builder.Build();
			StartEventHubListener(TokenSource.Token);
			StartEventHubSender(TokenSource.Token);
			Console.WriteLine($"App with id {_clientId} started. Press a button to stop the app");
			Console.ReadKey();
			TokenSource.Cancel();
			Task.Delay(200)
				.Wait();
			Console.WriteLine("\nApp finished.");
		}

		private static void StartEventHubListener(CancellationToken token)
		{
			Task.Run(
					async () =>
					{
						Console.WriteLine("Listener started.");
						await using (var client = new EventHubConsumerClient(EventHubConsumerClient.DefaultConsumerGroupName, _config["EventHub:ConnectionString"], _config["EventHub:HubName"]))
						{
							await foreach (var receivedEvent in client.ReadEventsAsync(token))
							{
								if (receivedEvent.Data != null)
								{
									var text = Encoding.UTF8.GetString(receivedEvent.Data.Body.ToArray());
									var model = JsonSerializer.Deserialize<MessageModel>(text);
									if (model == null)
									{
										Console.WriteLine($"Invalid data received:\n{text}");
									}
									else
									{
										Console.WriteLine($"Event received from client {model.ClientId}: {model.Data}");
									}
									if (receivedEvent.Data.Properties != null)
									{
										foreach (var property in receivedEvent.Data.Properties)
										{
											Console.WriteLine("Event data key = {0}, Event data value = {1}", property.Key, property.Value);
										}
									}
								}
							}
						}
					},
					token)
				.ContinueWith(
					t =>
					{
						Console.WriteLine("Listener stopped.");
					});
		}

		private static void StartEventHubSender(CancellationToken token)
		{
			Task.Run(
					async () =>
					{
						Console.WriteLine("Sender started.");
						while (!token.IsCancellationRequested)
						{
							await using (var client = new EventHubProducerClient(_config["EventHub:ConnectionString"], _config["EventHub:HubName"]))
							{
								var options = new CreateBatchOptions { PartitionKey = _clientId.ToString() };
								using (var eventBatch = await client.CreateBatchAsync(options, token))
								{
									var message = new MessageModel { ClientId = _clientId.ToString(), Data = $"{DateTimeOffset.Now} Hello World" };
									var messageText = JsonSerializer.Serialize(message);
									var eventData = new EventData(Encoding.UTF8.GetBytes(messageText));
									eventData.Properties.Add("company", "DEVDEER");
									eventBatch.TryAdd(eventData);
									// Use the producer client to send the batch of events to the event hub
									await client.SendAsync(eventBatch, token);
									Console.WriteLine("Message sent");
								}
							}
							await Task.Delay(1000, token);
						}
					},
					token)
				.ContinueWith(
					t =>
					{
						Console.WriteLine("Sender stopped.");
					});
		}

		#endregion
	}
}
