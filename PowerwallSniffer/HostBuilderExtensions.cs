namespace PowerwallSniffer
{
    using System;
    using System.IO;
    using System.Net.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public static class HostBuilderExtensions
    {
        public static IHostBuilder BuildApplication(this IHostBuilder hostBuilder, string[] args)
        {
            return hostBuilder
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((context, config) =>
                {
                    config
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables();
                })
                .ConfigureLogging((context, logger) =>
                {
                    logger
                        .AddConfiguration(context.Configuration.GetSection("Logging"))
                        .AddConsole();
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .Configure<AppConfig>(context.Configuration.GetSection("AppConfig"))
                        .AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppConfig>>().Value) // DI inject AppConfig object
                        .AddHostedService<PowerwallService>()
                        .AddHttpClient("powerwall", client =>
                        {
                            client.BaseAddress = new Uri(context.Configuration.GetSection("AppConfig").Get<AppConfig>().GatewayUrl);
                            client.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-PowerwallSniffer");
                        })
                        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true // Ignore self signed cert
                        })
                        .AddTypedClient<PowerwallClient>();
                });
        }
    }
}