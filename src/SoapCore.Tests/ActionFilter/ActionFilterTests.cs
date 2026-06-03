using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoapCore.Tests.Model;
using SoapCore.Tests.Utilities;

namespace SoapCore.Tests.ActionFilter
{
	[TestClass]
	public class ActionFilterTests
	{
		private static IHost _host;
		private static string _hostAddress;

		[ClassInitialize]
#pragma warning disable IDE0060 // Remove unused parameter
		public static void StartServer(TestContext context)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			_host = TestHostFactory.StartKestrel(webBuilder => webBuilder
				.UseKestrel()
				.UseUrls("http://127.0.0.1:0")
				.UseStartup<Startup>());
			_hostAddress = _host.GetServerAddress();
		}

		[ClassCleanup]
		public static async Task StopServer()
		{
			if (_host != null)
			{
				await _host.StopAsync();
				_host.Dispose();
				_host = null;
				_hostAddress = null;
			}
		}

		public ITestService CreateClient()
		{
			var binding = new BasicHttpBinding();
			var endpoint = new EndpointAddress(new Uri($"{_hostAddress}/Service.svc"));
			var channelFactory = new ChannelFactory<ITestService>(binding, endpoint);
			var serviceClient = channelFactory.CreateChannel();
			return serviceClient;
		}

		[TestMethod]
		public void ModelWasAlteredInActionFilter()
		{
			var inputModel = new ComplexModelInput
			{
				StringProperty = "string property test value",
				IntProperty = 123,
				ListProperty = new List<string> { "test", "list", "of", "strings" },
				DateTimeOffsetProperty = new DateTimeOffset(2018, 12, 31, 13, 59, 59, TimeSpan.FromHours(1))
			};

			var client = CreateClient();
			var result = client.ComplexParamWithActionFilter(inputModel);
			Assert.AreNotEqual(inputModel.StringProperty, result.StringProperty);
			Assert.AreNotEqual(inputModel.IntProperty, result.IntProperty);
		}
	}
}
