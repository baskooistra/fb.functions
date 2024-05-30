using CommunityToolkit.Diagnostics;
using FB.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace FB.Functions
{
    public static class UserCreatedEventOrchestrator
    {
        [Function(nameof(UserCreatedEventOrchestrator))]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(UserCreatedEventOrchestrator));
            var userData = context.GetInput<UserCreatedEventData>();

            logger.LogInformation("Starting task to fetch email confirmation data");
            var emailConfirmationData = await context.CallActivityAsync<string>(nameof(GetEmailConfirmationData), userData);

            Guard.IsNotNullOrWhiteSpace(emailConfirmationData, nameof(GetEmailConfirmationData));
        }
    }
}
