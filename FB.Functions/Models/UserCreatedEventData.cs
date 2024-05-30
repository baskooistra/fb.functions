using Azure.Messaging;
using Azure.Messaging.EventGrid;

namespace FB.Functions.Models
{
    public class UserCreatedEventData
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }

        public static UserCreatedEventData CreateFromCloudEvent(EventGridEvent gridEvent) => gridEvent.Data.ToObjectFromJson<UserCreatedEventData>();
    }
}
