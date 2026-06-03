using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoapCore.Tests.Utilities;

namespace SoapCore.Tests.ServiceOperationTuner
{
	[TestClass]
	public class ServiceOperationTunerTests
	{
		private static IHost _host;

		[ClassInitialize]
		public static void StartServer(TestContext testContext)
		{
			_host = TestHostFactory.StartKestrel(webBuilder => webBuilder
				.UseKestrel()
				.UseUrls("http://127.0.0.1:0")
				.UseStartup<Startup>());
		}

		[ClassCleanup]
		public static async Task StopServer()
		{
			if (_host != null)
			{
				await _host.StopAsync();
				_host.Dispose();
			}
		}

		[TestInitialize]
		public void Reset()
		{
			TestServiceOperationTuner.Reset();
		}

		public ITestService CreateClient(string pingValue)
		{
			var address = _host.GetServerAddress();

			var binding = new BasicHttpBinding();
			var endpoint = new EndpointAddress(new Uri(string.Format("{0}/Service.svc", address)));
			var channelFactory = new ChannelFactory<ITestService>(binding, endpoint);
			channelFactory.Endpoint.EndpointBehaviors.Add(new CustomHeadersEndpointBehavior(pingValue));
			var serviceClient = channelFactory.CreateChannel();
			return serviceClient;
		}

		[TestMethod]
		public void PassParameterViaHttpHeader()
		{
			Assert.IsFalse(TestServiceOperationTuner.IsCalled);
			Assert.IsFalse(TestServiceOperationTuner.IsSetPingValue);

			string expected = "ping value";
			var client = CreateClient(expected);
			var result = client.PingWithServiceOperationTuning();

			Assert.IsTrue(TestServiceOperationTuner.IsCalled);
			Assert.IsTrue(TestServiceOperationTuner.IsSetPingValue);
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void CheckThatPingIsNotAffectedByOperationTuner()
		{
			Assert.IsFalse(TestServiceOperationTuner.IsCalled);
			Assert.IsFalse(TestServiceOperationTuner.IsSetPingValue);

			string expected = "ping";
			var client = CreateClient("bla-bla-bla");
			var result = client.Ping("ping");

			Assert.IsTrue(TestServiceOperationTuner.IsCalled);
			Assert.IsFalse(TestServiceOperationTuner.IsSetPingValue);
			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void CheckThatHttpContextBodyAreAvailableByOperationTuner()
		{
			Assert.IsFalse(TestServiceOperationTuner.IsCalled);
			Assert.IsFalse(TestServiceOperationTuner.IsBodyAvailable);

			var client = CreateClient("bla-bla-bla");

			client.PingWithServiceOperationTuning();

			Assert.IsTrue(TestServiceOperationTuner.IsCalled);
			Assert.IsTrue(TestServiceOperationTuner.IsBodyAvailable);
		}
	}
}
