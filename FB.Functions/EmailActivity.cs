// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Azure.Messaging.EventGrid;
using CommunityToolkit.Diagnostics;
using FB.Functions.Models;
using FB.Functions.Templates;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace FB.Functions;

public class EmailActivity
{
    [Function(nameof(SendWelcomeEmail))]
    [SendGridOutput(ApiKey = "FB_SENDGRID_APIKEY")]
    public string SendWelcomeEmail([ActivityTrigger] SendWelcomeEmailRequest request, FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger(nameof(SendWelcomeEmail));

        return WelcomeEmail.CreateMessage(request.UserData.FirstName, request.UserData.LastName, request.UserData.Email, request.ConfirmationLink);
    }
}