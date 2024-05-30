using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FB.Functions.Models
{
    public class SendWelcomeEmailRequest
    {
        public required UserCreatedEventData UserData { get; set; }
        public required string ConfirmationLink { get; set; }
    }
}
