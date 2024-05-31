using System.Text.Json.Serialization;

namespace FB.Functions.Models
{
    public class UserProfileResponse
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("email")]
        public required string Email { get; set; }
    }
}
