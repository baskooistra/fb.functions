using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Diagnostics;
using FB.Functions.Connectors.IdentityServer;
using Azure.Identity;

var host = new HostBuilder()
    .ConfigureAppConfiguration(builder =>
    {
        if (Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT") == "Development")
            builder.AddJsonFile("local.settings.json");
        else
        {
            builder.AddAzureAppConfiguration(options =>
            {
                var configurationConnection = Environment.GetEnvironmentVariable("CONFIGURATION_CONNECTION_STRING");
                var configurationKey = Environment.GetEnvironmentVariable("CONFIGURATION_KEY") + ":";
                var environmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");

                Guard.IsNotNullOrWhiteSpace(configurationConnection);

                var config = options.Connect(new Uri(configurationConnection), new ManagedIdentityCredential())
                .ConfigureKeyVault(kv => kv.SetCredential(new ManagedIdentityCredential()))
                .Select($"{configurationKey}*", environmentName)
                .TrimKeyPrefix(configurationKey);
            });
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