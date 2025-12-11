using System;
using System.Collections.Generic;
using System.ServiceModel;
using Models;

namespace Net10Client
{
	public class Program
	{
		public static void Main()
		{
			Console.WriteLine("NET 10 Client - Connecting to SOAP service...");

			var binding = new BasicHttpBinding();
			var endpoint = new EndpointAddress(new Uri($"http://{Environment.MachineName}:5060/Service.svc"));
			var channelFactory = new ChannelFactory<ISampleService>(binding, endpoint);
			var serviceClient = channelFactory.CreateChannel();

			try
			{
				var result = serviceClient.Ping("Hello from NET 10 Client");
				Console.WriteLine("Ping method result: {0}", result);

				var complexModel = new ComplexModelInput
				{
					StringProperty = Guid.NewGuid().ToString(),
					IntProperty = int.MaxValue / 2,
					ListProperty = new List<string> { "NET", "10", "test", "list" },
					DateTimeOffsetProperty = new DateTimeOffset(2024, 12, 31, 13, 59, 59, TimeSpan.FromHours(1))
				};

				var complexResult = serviceClient.PingComplexModel(complexModel);
				Console.WriteLine("PingComplexModel result. FloatProperty: {0}, StringProperty: {1}, ListProperty: {2}, DateTimeOffsetProperty: {3}, EnumProperty: {4}",
					complexResult.FloatProperty, complexResult.StringProperty, string.Join(", ", complexResult.ListProperty), 
					complexResult.DateTimeOffsetProperty, complexResult.TestEnum);

				serviceClient.VoidMethod(out var stringValue);
				Console.WriteLine("Void method result: {0}", stringValue);

				var asyncMethodResult = serviceClient.AsyncMethod().Result;
				Console.WriteLine("Async method result: {0}", asyncMethodResult);

				var xmlElement = System.Xml.Linq.XElement.Parse("<test>NET 10 string</test>");
				serviceClient.XmlMethod(xmlElement);
				Console.WriteLine("XmlMethod executed successfully");

				var complexReturnModels = serviceClient.ComplexReturnModel();
				Console.WriteLine("ComplexReturnModel results:");
				foreach (var model in complexReturnModels)
				{
					Console.WriteLine("  Id: {0}, Name: {1}", model.Id, model.Name);
				}

				Console.WriteLine("\nAll tests completed successfully!");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error: {0}", ex.Message);
			}
			finally
			{
				if (serviceClient is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}

			Console.WriteLine("\nPress any key to exit...");
			Console.ReadKey();
		}
	}
}
