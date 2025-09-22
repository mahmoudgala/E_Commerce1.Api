using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
//using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace ECommerce.Api.Utility
{
    public class EmailSender : IEmailSender
    {


        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var Client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mg6507011@gmail.com", "iufu vgrb kiuj roio")
            };
            return Client.SendMailAsync(
                new MailMessage(from: "your.email@live.com", 
                                        to:email,
                                        subject,
                                        htmlMessage
                                        )
                {
                    IsBodyHtml = true
                });
        }

    }
}
