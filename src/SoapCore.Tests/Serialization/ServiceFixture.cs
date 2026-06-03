using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.ServiceModel;
using System.Xml;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SoapCore.Tests.Utilities;

namespace SoapCore.Tests.Serialization
{
	public sealed class ServiceFixture<TService> : IDisposable
		where TService : class
	{
		private readonly IHost _host;
		private readonly Dictionary<SoapSerializer, TService> _sampleServiceClients = new Dictionary<SoapSerializer, TService>();

		public ServiceFixture()
		{
			var binding = new BasicHttpBinding
			{
				MaxReceivedMessageSize = int.MaxValue,
				ReaderQuotas = XmlDictionaryReaderQuotas.Max
			};

			// start service host
			_host = TestHostFactory.StartKestrel(webBuilder => webBuilder
				.ConfigureServices(services =>
				{
					// init SampleService service mock
					ServiceMock = new Mock<TService>();
					services.AddSingleton(ServiceMock.Object);
					services.AddSoapCore();
					services.AddMvc();
				})
				.Configure(appBuilder =>
				{
					appBuilder.UseRouting();

					appBuilder.UseEndpoints(x =>
					{
						x.UseSoapEndpoint<TService>("/Service.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);
						x.UseSoapEndpoint<TService>("/Service.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
					});
				})
				.UseKestrel()
				.UseUrls($"http://127.0.0.1:0")
				.UseContentRoot(Directory.GetCurrentDirectory()));

			var address = _host.GetServerAddress();

			//make service client
			var endpointXml = new EndpointAddress(new Uri($"{address}/Service.asmx"));
			var channelFactoryXml = new ChannelFactory<TService>(binding, endpointXml);
			var serviceClientXml = channelFactoryXml.CreateChannel();

			var endpointDC = new EndpointAddress(new Uri($"{address}/Service.svc"));
			var channelFactoryDC = new ChannelFactory<TService>(binding, endpointDC);
			var serviceClientDc = channelFactoryDC.CreateChannel();

			_sampleServiceClients[SoapSerializer.XmlSerializer] = serviceClientXml;
			_sampleServiceClients[SoapSerializer.DataContractSerializer] = serviceClientDc;
		}

		public Mock<TService> ServiceMock { get; private set; }

		public static IEnumerable<object[]> SoapSerializersList()
		{
			foreach (var soapSerializer in Enum.GetValues(typeof(SoapSerializer)))
			{
				yield return new[] { soapSerializer };
			}
		}

		public TService GetSampleServiceClient(SoapSerializer soapSerializer)
		{
			return _sampleServiceClients[soapSerializer];
		}

		public void Dispose()
		{
			_host.StopAsync().GetAwaiter().GetResult();
			_host.Dispose();
		}
	}
}
