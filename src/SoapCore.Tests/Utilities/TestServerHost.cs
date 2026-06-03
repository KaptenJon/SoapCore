using System;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace SoapCore.Tests.Utilities
{
	internal sealed class TestServerHost : IDisposable
	{
		private readonly IHost _host;

		public TestServerHost(IHost host)
		{
			_host = host;
		}

		public IServiceProvider Services => _host.Services;

		public HttpClient CreateClient() => _host.GetTestClient();

		public RequestBuilder CreateRequest(string path) => _host.GetTestServer().CreateRequest(path);

		public void Dispose()
		{
			_host.Dispose();
		}
	}
}
