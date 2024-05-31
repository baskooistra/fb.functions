using FB.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FB.Functions.Activities
{
    public class UserProfileActivity(ILogger<UserProfileActivity> logger)
    {
        [Function(nameof(CreateUserProfile))]
        [CosmosDBOutput("%COSMOS_DB_DATABASE_NAME%", "%COSMOS_DB_CONTAINER_NAME%", Connection = "CosmosDB")]
        public UserProfileResponse CreateUserProfile([ActivityTrigger] UserCreatedEventData userData, FunctionContext executionContext)
        {
            logger.LogInformation("Creating user profile in cosmos db");
            return new UserProfileResponse
            {
                Id = userData.Id,
                Name = $"{userData.FirstName} {userData.LastName}",
                Email = userData.Email
            };
        }
    }
}
