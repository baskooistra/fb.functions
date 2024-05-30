using CommunityToolkit.Diagnostics;
using FB.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;

namespace FB.Functions
{
    public class UserCreatedEventOrchestrator
    {
        [Function(nameof(UserCreatedEventOrchestrator))]
        public async Task RunOrchestrator(
            [OrchestrationTrigger] TaskOrchestrationContext context)
        {
            ILogger logger = context.CreateReplaySafeLogger(nameof(UserCreatedEventOrchestrator));
            var userData = context.GetInput<UserCreatedEventData>();

            logger.LogInformation("Starting task to fetch email confirmation data");
            var emailConfirmationLink = await context.CallActivityAsync<string>(nameof(UserDataActivity.GetEmailConfirmationData), userData);

            Guard.IsNotNullOrWhiteSpace(emailConfirmationLink, nameof(emailConfirmationLink));

            var request = new SendWelcomeEmailRequest
            {
                UserData = userData!,
                ConfirmationLink = emailConfirmationLink
            };

            await context.CallActivityAsync(nameof(EmailActivity.SendWelcomeEmail), request);
        }
    }
}
