using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Diagnostics;
using FB.Functions.Connectors.IdentityServer;

var host = new HostBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development")
            builder.AddJsonFile("local.settings.json");
        else
        {
            var configurationConnection = Environment.GetEnvironmentVariable("CONFIGURATION_CONNECTION_STRING");
            Guard.IsNotNullOrWhiteSpace(configurationConnection);
            builder.AddAzureAppConfiguration(configurationConnection);
        }
    })
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddTransient<IIdentityServerConnector, IdentityServerConnector>();
    })
    .Build();

host.Run();