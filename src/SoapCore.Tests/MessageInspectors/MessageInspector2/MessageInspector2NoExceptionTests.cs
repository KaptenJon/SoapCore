using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoapCore.Tests.Utilities;

namespace SoapCore.Tests.MessageInspectors.MessageInspector2
{
	[TestClass]
	public class MessageInspector2NoExceptionTests
	{
		private static IHost _host;
		private static string _hostAddress;

		[ClassInitialize]
		public static void StartServer(TestContext testContext)
		{
			_host = TestHostFactory.StartKestrel(webBuilder => webBuilder
				.UseKestrel()
				.UseUrls("http://127.0.0.1:0")
				.UseStartup<Startup>()
				.UseSetting("InspectorStyle", InspectorStyle.MessageInspector2NoException.ToString()));
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

		[TestInitialize]
		public void Reset()
		{
			MessageInspector2Mock.Reset();
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
