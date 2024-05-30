using FB.Functions.Connectors.IdentityServer;
using FB.Functions.Models;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace FB.Functions;

[DurableTask(nameof(GetEmailConfirmationData))]
public class GetEmailConfirmationData(
    ILoggerFactory loggerFactory, 
    IIdentityServerConnector identityServerConnector) : TaskActivity<UserCreatedEventData, string>
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<GetEmailConfirmationData>();

    public async override Task<string> RunAsync(TaskActivityContext context, UserCreatedEventData input)
    {
        try
        {
            return await identityServerConnector.GetAccountConfirmationUrl(input.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to fetch confirmation link from identity server");
            throw;
        }
    }
}