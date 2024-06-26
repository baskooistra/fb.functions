// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Azure.Messaging.EventGrid;
using FB.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace FB.Functions
{
    public class UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
    {
        [Function(nameof(HandleUserCreatedEvent))]
        public async Task HandleUserCreatedEvent(
            [EventGridTrigger] EventGridEvent gridEvent,
            [DurableClient] DurableTaskClient durableTaskClient,
            FunctionContext executionContext)
        {
            logger.LogInformation("Handling event of type: {type}. Event subject: {subject}", gridEvent.EventType, gridEvent.Subject);
            
            var userData = UserCreatedEventData.CreateFromCloudEvent(gridEvent);
            
            string durableTaskInstanceId = await durableTaskClient.ScheduleNewOrchestrationInstanceAsync(nameof(UserCreatedEventOrchestrator), userData);

            logger.LogInformation("Scheduled durable task orchestrator of type {type} with id {instanceId}", 
                nameof(UserCreatedEventOrchestrator), durableTaskInstanceId);
        }
    }
}
