using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoapCore.Tests.MessageContract.Models;
using SoapCore.Tests.Utilities;

namespace SoapCore.Tests.MessageContract
{
	[TestClass]
	public class RawRequestSoap11And12
	{
		[TestMethod]
		public async Task Soap11And12MessageContractGetWSDL_ShouldContainSoap11AndSoap12Namespaces()
		{
			using var host = CreateTestHost(typeof(TestService));
			using var client = host.CreateClient();
			using var res = host.CreateRequest("/Service11And12.asmx?wsdl").GetAsync().Result;

			res.EnsureSuccessStatusCode();

			var response = await res.Content.ReadAsStringAsync();
			var root = XDocument.Parse(response);
			Assert.AreEqual("http://schemas.xmlsoap.org/wsdl/soap/", root.Root.Attributes().FirstOrDefault(t => t.Name.LocalName == "soap").Value);
			Assert.AreEqual("http://schemas.xmlsoap.org/wsdl/soap12/",  root.Root.Attributes().FirstOrDefault(t => t.Name.LocalName == "soap12").Value);
		}

		private TestServerHost CreateTestHost(Type serviceType)
		{
			var configurationKey = TestHostFactory.RegisterStartupConfiguration(new StartupConfiguration(serviceType));

			return TestHostFactory.CreateTestServer(webBuilder =>
			{
				webBuilder
					.UseSetting(TestHostFactory.StartupConfigurationKeySetting, configurationKey)
					.UseStartup<Startup>();
			});
		}
	}
}
