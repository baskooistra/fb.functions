using FB.Functions.Connectors.IdentityServer;
using FB.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FB.Functions;

public class UserDataActivity(ILogger<UserDataActivity> logger, IIdentityServerConnector identityServerConnector)
{
    [Function(nameof(GetEmailConfirmationData))]
    public async Task<string> GetEmailConfirmationData([ActivityTrigger] UserCreatedEventData input, FunctionContext executionContext)
    {
        logger.LogInformation("Fetching email confirmation data from identity server");

        try
        {
            return await identityServerConnector.GetAccountConfirmationUrl(input.Id);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unable to fetch confirmation link from identity server");
            throw;
        }
    }
}