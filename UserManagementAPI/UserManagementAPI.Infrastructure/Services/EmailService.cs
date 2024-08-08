using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using UserManagementAPI.Application.Interfaces.Services;
using UserManagementAPI.Domain.Common_Models;

namespace UserManagementAPI.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }


        public void SendEmail(EmailModel emailModel)
        {
            var from = _config["EmailSettings:From"];
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("AdminEmail", from));
            emailMessage.To.Add(new MailboxAddress("UserEmail", "pobag42825@biowey.com"));
            emailMessage.Cc.Add(new MailboxAddress("CCUserEmail", emailModel.To));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_config["EmailSettings:StmpServer"], Convert.ToInt32(_config["Email:Port"]), true);
                    client.Authenticate(_config["EmailSettings:Username"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
        }
    }
}
