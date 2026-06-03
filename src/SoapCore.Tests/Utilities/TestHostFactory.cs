using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SoapCore.Tests.Utilities
{
	internal static class TestHostFactory
	{
		public const string StartupConfigurationKeySetting = "SoapCoreTestStartupConfigurationKey";

		private static readonly ConcurrentDictionary<string, object> StartupConfigurations = new ConcurrentDictionary<string, object>();

		public static IHost StartKestrel(Action<IWebHostBuilder> configureWebHost)
		{
			var host = Host.CreateDefaultBuilder()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					configureWebHost(webBuilder);
				})
				.Build();

			host.StartAsync().GetAwaiter().GetResult();
			return host;
		}

		public static TestServerHost CreateTestServer<TStartup>()
			where TStartup : class
		{
			return CreateTestServer(webBuilder => webBuilder.UseStartup<TStartup>());
		}

		public static TestServerHost CreateTestServer(Action<IWebHostBuilder> configureWebHost)
		{
			var host = Host.CreateDefaultBuilder()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseTestServer();
					configureWebHost(webBuilder);
				})
				.Build();

			host.StartAsync().GetAwaiter().GetResult();
			return new TestServerHost(host);
		}

		public static string GetServerAddress(this IHost host)
		{
			var addressesFeature = host.Services
				.GetRequiredService<IServer>()
				.Features
				.Get<IServerAddressesFeature>();

			return addressesFeature?.Addresses.Single() ?? throw new InvalidOperationException("Server address not available.");
		}

		public static string RegisterStartupConfiguration(object configuration)
		{
			var key = Guid.NewGuid().ToString("N");
			StartupConfigurations[key] = configuration;
			return key;
		}

		public static T GetStartupConfiguration<T>(string key)
			where T : class
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new InvalidOperationException("Startup configuration key was not provided.");
			}

			if (StartupConfigurations.TryGetValue(key, out var configuration))
			{
				return configuration as T ?? throw new InvalidOperationException($"Startup configuration '{key}' is not of type {typeof(T).FullName}.");
			}

			throw new InvalidOperationException($"Startup configuration '{key}' was not found.");
		}
	}
}
