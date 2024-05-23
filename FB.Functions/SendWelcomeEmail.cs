// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Azure.Messaging.EventGrid;
using CommunityToolkit.Diagnostics;
using FB.Functions.Templates;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace FB.Functions;

public class SendWelcomeEmail(ILogger<SendWelcomeEmail> logger)
{
    [Function("SendWelcomeEmail")]
    public SendGridMessage Run([EventGridTrigger] EventGridEvent cloudEvent, FunctionContext context)
    {
        logger.LogInformation("Event type: {type}, Event subject: {subject}", cloudEvent.EventType, cloudEvent.Subject);
        var data = cloudEvent.Data.ToObjectFromJson<UserCreatedEventData>();

        Guard.IsNotNullOrWhiteSpace(data.FirstName, nameof(data.FirstName));
        Guard.IsNotNullOrWhiteSpace(data.LastName, nameof(data.LastName));
        Guard.IsNotNullOrWhiteSpace(data.Email, nameof(data.Email));

        return WelcomeEmail.CreateMessage(data.FirstName, data.LastName, data.Email);
    }

    private class UserCreatedEventData
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
    }
}