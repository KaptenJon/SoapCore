using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoapCore.Tests.Utilities;
using SoapCore.Tests.WsdlFromFile.Services;

namespace SoapCore.Tests.WsdlFromFile
{
	[TestClass]
	public class WsdlIncludeTests
	{
		private IHost _host;

		[TestMethod]
		public void CheckWsdlInclude()
		{
			StartService2(typeof(EchoIncludeService));
			var wsdl = GetWsdlFromAsmx("Service2.asmx");
			var address = _host.GetServerAddress();
			StopServer();

			var root = new XmlDocument();
			root.LoadXml(wsdl);

			var nsmgr = new XmlNamespaceManager(root.NameTable);
			nsmgr.AddNamespace("wsdl", "http://schemas.xmlsoap.org/wsdl/");
			nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
			nsmgr.AddNamespace("soapbind", "http://schemas.xmlsoap.org/wsdl/soap/");

			var element = root.SelectSingleNode("/wsdl:definitions/wsdl:import[1]", nsmgr);

			string url = address + "/Management/Service2.asmx?import&name=ServiceDefinitions.xml";

			Assert.IsNotNull(element);
			Assert.AreEqual(url, element.Attributes["location"]?.Value);
		}

		[TestMethod]
		public void CheckXSDInclude()
		{
			StartService(typeof(EchoIncludeService));
			var wsdl = GetWsdlFromAsmx("Service.asmx");
			var address = _host.GetServerAddress();
			StopServer();

			var root = new XmlDocument();
			root.LoadXml(wsdl);

			var nsmgr = new XmlNamespaceManager(root.NameTable);
			nsmgr.AddNamespace("wsdl", "http://schemas.xmlsoap.org/wsdl/");
			nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
			nsmgr.AddNamespace("soapbind", "http://schemas.xmlsoap.org/wsdl/soap/");

			var element = root.SelectSingleNode("/wsdl:definitions/wsdl:types/xs:schema/xs:include[1]", nsmgr);

			string url = address + "/Management/Service.asmx?xsd&name=echoInclude.xsd";

			Assert.IsNotNull(element);
			Assert.AreEqual(url, element.Attributes["schemaLocation"]?.Value);
		}

		[TestMethod]
		public void CheckXSDExists()
		{
			StartService(typeof(MeasurementSiteTablePublicationService));
			var xsd = GetXSDFromAsmx();
			Trace.TraceInformation(xsd);
			Assert.IsNotNull(xsd);
			StopServer();

			XmlSchema.Read(new XmlTextReader(new StringReader(xsd)), ValidationCallback);

			static void ValidationCallback(object sender, ValidationEventArgs args)
			{
				Assert.Fail(args.Message);
			}
		}

		[TestMethod]
		public void CheckXSDIncludeXSD()
		{
			StartService(typeof(EchoIncludeService));
			var xsd = GetXSDFromAsmx();
			var address = _host.GetServerAddress();
			StopServer();

			var root = new XmlDocument();
			root.LoadXml(xsd);

			var nsmgr = new XmlNamespaceManager(root.NameTable);
			nsmgr.AddNamespace("wsdl", "http://schemas.xmlsoap.org/wsdl/");
			nsmgr.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");
			nsmgr.AddNamespace("soapbind", "http://schemas.xmlsoap.org/wsdl/soap/");

			var element = root.SelectSingleNode("/xs:schema/xs:include[1]", nsmgr);

			string url = address + "/Service.asmx?xsd&name=echoIncluded.xsd";

			Assert.IsNotNull(element);
			Assert.AreEqual(url, element.Attributes["schemaLocation"]?.Value);
		}

		[TestCleanup]
		public void StopServer()
		{
			if (_host != null)
			{
				_host.StopAsync().GetAwaiter().GetResult();
				_host.Dispose();
				_host = null;
			}
		}

		private string GetWsdlFromAsmx(string serviceName)
		{
			var address = _host.GetServerAddress();

			using (var httpClient = new HttpClient())
			{
				return httpClient.GetStringAsync(string.Format("{0}/{1}?wsdl", address, serviceName)).Result;
			}
		}

		private string GetXSDFromAsmx()
		{
			var serviceName = "Service.asmx";

			var address = _host.GetServerAddress();

			using (var httpClient = new HttpClient())
			{
				return httpClient.GetStringAsync(string.Format("{0}/{1}?xsd&name=echoInclude.xsd", address, serviceName)).Result;
			}
		}

		private void StartService(Type serviceType)
		{
			StartService("Service", serviceType, "WSDL", "echoInclude.wsdl");
		}

		private void StartService2(Type serviceType)
		{
			StartService("Service2", serviceType, "WSDLInclude", "echoWsdlInclude.wsdl");
		}

		private void StartService(string serviceName, Type serviceType, string testFileFolder, string wsdlFile)
		{
			var configurationKey = TestHostFactory.RegisterStartupConfiguration(new StartupConfiguration(serviceName, serviceType, testFileFolder, wsdlFile));

			_host = TestHostFactory.StartKestrel(webBuilder => webBuilder
				.UseKestrel()
				.UseUrls("http://127.0.0.1:0")
				.UseSetting(TestHostFactory.StartupConfigurationKeySetting, configurationKey)
				.UseStartup<Startup>());
		}
	}
}
