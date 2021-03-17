using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Shop.MVC.Common
{
    public class EmailSender : IEmailSender
    {
        private IConfiguration configuration;
        
        public EmailSender(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential(configuration["EmailSettings:Sender"], configuration["EmailSettings:Password"]);

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress(configuration["EmailSettings:Sender"], configuration["EmailSettings:SenderName"]),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(email));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = Convert.ToInt32(configuration["EmailSettings:MailPort"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = configuration["EmailSettings:MailServer"],
                    EnableSsl = true,
                    Credentials = credentials
                };

                // Send it...         
                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }

            await Task.CompletedTask;
        }

    }
}
