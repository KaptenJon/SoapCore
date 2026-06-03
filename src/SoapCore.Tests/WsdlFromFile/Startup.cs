using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using SoapCore.Tests.Model;
using SoapCore.Tests.Utilities;

namespace SoapCore.Tests.WsdlFromFile
{
	public class Startup
	{
		private readonly string _serviceName;
		private readonly Type _serviceType;
		private readonly string _testFileFolder;
		private readonly string _wsdlFile;

		public Startup(IConfiguration configuration)
		{
			var startupConfigurationKey = configuration[TestHostFactory.StartupConfigurationKeySetting];
			var startupConfiguration = TestHostFactory.GetStartupConfiguration<IStartupConfiguration>(startupConfigurationKey);

			_serviceName = startupConfiguration.ServiceName;
			_serviceType = startupConfiguration.ServiceType;
			_testFileFolder = startupConfiguration.TestFileFolder;
			_wsdlFile = startupConfiguration.WsdlFile;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSoapCore();
			services.TryAddSingleton(_serviceType);
			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			WsdlFileOptions options = new WsdlFileOptions
			{
				UrlOverride = string.Empty,
				VirtualPath = string.Empty,
				WebServiceWSDLMapping = new Dictionary<string, WebServiceWSDLMapping>
				{
					{
						_serviceName + ".asmx", new WebServiceWSDLMapping
						{
							SchemaFolder = "/WsdlFromFile/" + _testFileFolder,
							WsdlFile = _wsdlFile,
							WSDLFolder = "/WsdlFromFile/" + _testFileFolder,
							UrlOverride = "Management/" + _serviceName + ".asmx"
						}
					}
				},
				AppPath = env.ContentRootPath
			};

			app.UseRouting();

			app.UseEndpoints(x =>
			{
				x.UseSoapEndpoint(_serviceType, "/" + _serviceName + ".svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);
				x.UseSoapEndpoint(_serviceType, "/" + _serviceName + ".asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer, false, null, options);
			});
		}
	}
}
