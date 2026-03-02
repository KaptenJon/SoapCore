using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoapCore.Tests.MessageInspectors.MessageInspector2
{
	[TestClass]
	public class MessageInspector2NoExceptionTests
	{
		private static IWebHost _host;

		[ClassInitialize]
		public static void StartServer(TestContext testContext)
		{
			_host = new WebHostBuilder()
				.UseKestrel()
				.UseUrls("http://127.0.0.1:0")
				.UseStartup<Startup>()
				.UseSetting("InspectorStyle", InspectorStyle.MessageInspector2NoException.ToString())
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

		[TestInitialize]
		public void Reset()
		{
			MessageInspector2Mock.Reset();
		}

		public ITestService CreateClient()
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
		public void AfterReceivedRequestCalled()
		{
			Assert.IsFalse(MessageInspector2Mock.AfterReceivedRequestCalled);
			var client = CreateClient();
			var result = client.Ping("Hello World");
			Assert.IsTrue(MessageInspector2Mock.AfterReceivedRequestCalled);
		}

		[TestMethod]
		public void BeforeSendReplyCalled()
		{
			Assert.IsFalse(MessageInspector2Mock.BeforeSendReplyCalled);
			var client = CreateClient();
			var result = client.Ping("Hello World");
			Assert.IsTrue(MessageInspector2Mock.BeforeSendReplyCalled);
		}

		[TestMethod]
		public void BeforeSendReplyCalledEvenIfServiceThrowsException()
		{
			Assert.IsFalse(MessageInspector2Mock.BeforeSendReplyCalled);
			var client = CreateClient();
			Assert.ThrowsException<FaultException>(client.ThrowException);
			Assert.IsTrue(MessageInspector2Mock.BeforeSendReplyCalled);
		}
	}
}
