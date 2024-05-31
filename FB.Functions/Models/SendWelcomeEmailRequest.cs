namespace FB.Functions.Models
{
    public class SendWelcomeEmailRequest
    {
        public required UserCreatedEventData UserData { get; set; }
        public required string ConfirmationLink { get; set; }
    }
}
