using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FB.Functions.Templates
{
    public static class WelcomeEmail
    {
        private const string MessageTemplate = "<h1>Welcome {0} {1}</h1><br /><br />" +
            "<h2>Thank you for joining fitbook and trusting us to aid in your fitness progress.<h2>" +
            "<p>We are absolutely delighted to have your here. Please follow the link to confirm your account.</p><br /><br />" +
            "<a href='{2}'>Confirm your account</a>";

        private const string Sender = "bas.kooistra1986@gmail.com";
        private const string Subject = "Welcome to fitbook";

        public static string CreateMessage(string firstName, string lastName, string email, string confirmationLink)
        {
            var message = new SendGridMessage();
            message.SetFrom(Sender, "Fitbook");
            message.SetSubject(Subject);
            message.AddTo(email, $"{firstName} {lastName}");

            message.AddContent(MimeType.Html, string.Format(MessageTemplate, firstName, lastName, confirmationLink));

            return JsonConvert.SerializeObject(message);
        }
    }
}
