using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FB.Functions.Templates
{
    internal static class WelcomeEmail
    {
        private const string MessageTemplate = "<h1>Welcome {0} {1}</h1><br /><br /><p>Thank you for joining fitbook and trusting us to aid in your progress</p>";

        private const string Sender = "bas.kooistra1986@gmail.com";
        private const string Subject = "Welcome to fitbook";

        public static string CreateMessage(string firstName, string lastName, string email)
        {
            var message = new SendGridMessage();
            message.SetFrom(Sender, "Fitbook");
            message.SetSubject(Subject);
            message.AddTo(email, $"{firstName} {lastName}");

            message.AddContent(MimeType.Html, string.Format(MessageTemplate, firstName, lastName));

            return JsonConvert.SerializeObject(message);
        }
    }
}
