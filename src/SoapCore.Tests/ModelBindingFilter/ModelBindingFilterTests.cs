using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoapCore.Tests.Model;

namespace SoapCore.Tests.ModelBindingFilter
{
	[TestClass]
	public class ModelBindingFilterTests
	{
		private static IWebHost _host;

		[ClassInitialize]
		public static void StartServer(TestContext testContext)
		{
			_host = new WebHostBuilder()
				.UseKestrel()
				.UseUrls("http://127.0.0.1:0")
				.UseStartup<Startup>()
				.Build();

			var task = _host.RunAsync();

			while (true)
			{
				if (_host != null)
				{
					if (task.IsFaulted && task.Exception != null)
					{
						throw task.Exception;
					}

					if (!task.IsCompleted || !task.IsCanceled)
					{
						if (!_host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First().EndsWith(":0"))
						{
							break;
						}
					}
				}

				Thread.Sleep(2000);
			}
		}

		[ClassCleanup]
		public static async Task StopServer()
		{
			await _host.StopAsync();
		}

		public ITestService CreateClient(Dictionary<string, object> headers = null)
		{
			var addresses = _host.ServerFeatures.Get<IServerAddressesFeature>();
			var address = addresses.Addresses.Single();

			var binding = new BasicHttpBinding();
			var endpoint = new EndpointAddress(new Uri(string.Format("{0}/Service.svc", address)));
			var channelFactory = new ChannelFactory<ITestService>(binding, endpoint);
			var serviceClient = channelFactory.CreateChannel();
			return serviceClient;
		}

		[TestMethod]
		public void ModelWasAlteredInModelBindingFilter()
		{
			var inputModel = new ComplexModelInputForModelBindingFilter
			{
				StringProperty = "string property test value",
				IntProperty = 123,
				ListProperty = new List<string> { "test", "list", "of", "strings" },
				DateTimeOffsetProperty = new DateTimeOffset(2018, 12, 31, 13, 59, 59, TimeSpan.FromHours(1))
			};

			var client = CreateClient();
			var result = client.ComplexParamWithModelBindingFilter(inputModel);
			Assert.AreNotEqual(inputModel.StringProperty, result.StringProperty);
			Assert.AreNotEqual(inputModel.IntProperty, result.IntProperty);
		}

		[TestMethod]
		public void ModelWasNotAlteredInModelBindingFilter()
		{
			var inputModel = new ComplexModelInput
			{
				StringProperty = "string property test value",
				IntProperty = 123,
				ListProperty = new List<string> { "test", "list", "of", "strings" },
				DateTimeOffsetProperty = new DateTimeOffset(2018, 12, 31, 13, 59, 59, TimeSpan.FromHours(1))
			};

			var client = CreateClient();
			var result = client.ComplexParam(inputModel);
			Assert.AreEqual(inputModel.StringProperty, result.StringProperty);
			Assert.AreEqual(inputModel.IntProperty, result.IntProperty);
		}
	}
}
