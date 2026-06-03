using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using SoapCore.Tests.Utilities;

namespace SoapCore.Tests.RequestArgumentsOrder
{
	public sealed class ServiceFixture<TOriginalParametersOrderService, TReversedParametersOrderService> : IDisposable
		where TOriginalParametersOrderService : class
		where TReversedParametersOrderService : class
	{
		private readonly IHost _host;
		private readonly Dictionary<SoapSerializer, TOriginalParametersOrderService> _originalRequestArgumentsOrderClients;
		private readonly Dictionary<SoapSerializer, TReversedParametersOrderService> _reversedRequestArgumentsOrderClients;

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
					// init service mock
					ServiceMock = new Mock<TOriginalParametersOrderService>();
					services.AddSingleton(ServiceMock.Object);
					services.AddSoapCore();
					services.AddMvc();
				})
				.Configure(appBuilder =>
				{
					appBuilder.UseRouting();

					appBuilder.UseEndpoints(x =>
					{
						x.UseSoapEndpoint<TOriginalParametersOrderService>("/Service.svc", new SoapEncoderOptions(), SoapSerializer.DataContractSerializer);
						x.UseSoapEndpoint<TOriginalParametersOrderService>("/Service.asmx", new SoapEncoderOptions(), SoapSerializer.XmlSerializer);
					});
				})
				.UseKestrel()
				.UseUrls($"http://127.0.0.1:0")
				.UseContentRoot(Directory.GetCurrentDirectory()));

			var address = _host.GetServerAddress();

			//make clients
			_originalRequestArgumentsOrderClients = InitClients<TOriginalParametersOrderService>(binding, address);
			_reversedRequestArgumentsOrderClients = InitClients<TReversedParametersOrderService>(binding, address);
		}

		public Mock<TOriginalParametersOrderService> ServiceMock { get; private set; }

		public static IEnumerable<object[]> SoapSerializersList()
		{
			foreach (var soapSerializer in Enum.GetValues(typeof(SoapSerializer)))
			{
				yield return new[] { soapSerializer };
			}
		}

		public TOriginalParametersOrderService GetOriginalRequestArgumentsOrderClient(SoapSerializer soapSerializer)
		{
			return _originalRequestArgumentsOrderClients[soapSerializer];
		}

		public TReversedParametersOrderService GetReversedRequestArgumentsOrderClient(SoapSerializer soapSerializer)
		{
			return _reversedRequestArgumentsOrderClients[soapSerializer];
		}

		public void Dispose()
		{
			_host.StopAsync().GetAwaiter().GetResult();
			_host.Dispose();
		}

		private Dictionary<SoapSerializer, TService> InitClients<TService>(BasicHttpBinding binding, string address)
		{
			var endpointXml = new EndpointAddress(new Uri($"{address}/Service.asmx"));
			var channelFactoryXml = new ChannelFactory<TService>(binding, endpointXml);
			var serviceClientXml = channelFactoryXml.CreateChannel();

			var endpointDC = new EndpointAddress(new Uri($"{address}/Service.svc"));
			var channelFactoryDC = new ChannelFactory<TService>(binding, endpointDC);
			var serviceClientDc = channelFactoryDC.CreateChannel();

			return new Dictionary<SoapSerializer, TService>
			{
				[SoapSerializer.XmlSerializer] = serviceClientXml,
				[SoapSerializer.DataContractSerializer] = serviceClientDc
			};
		}
	}
}
