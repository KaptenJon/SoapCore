using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Models;
using SoapCore;

namespace Net10Server
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSoapCore();
			services.TryAddSingleton<ISampleService, SampleService>();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.UseSoapEndpoint<ISampleService>("/Service.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);
				endpoints.UseSoapEndpoint<ISampleService>("/Service.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
			});
		}
	}
}
