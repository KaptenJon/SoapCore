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

namespace SoapCore.Tests.Wsdl
{
	public class Startup
	{
		private readonly Type _serviceType;

		private readonly string _schemeOverride;

		public Startup(IConfiguration configuration)
		{
			var startupConfigurationKey = configuration[TestHostFactory.StartupConfigurationKeySetting];
			var startupConfiguration = TestHostFactory.GetStartupConfiguration<IStartupConfiguration>(startupConfigurationKey);

			_serviceType = startupConfiguration.ServiceType;
			_schemeOverride = startupConfiguration.SchemeOverride;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSoapCore();
			services.TryAddSingleton(_serviceType);
			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			app.UseRouting();

			app.UseEndpoints(x =>
			{
				x.UseSoapEndpoint(_serviceType, "/Service.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer, schemeOverride: _schemeOverride);
				x.UseSoapEndpoint(_serviceType, "/Service.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer, schemeOverride: _schemeOverride);
			});
		}
	}
}
