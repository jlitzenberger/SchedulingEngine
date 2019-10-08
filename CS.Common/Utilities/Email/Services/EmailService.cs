using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CS.Common.Utilities.Email.Client;

namespace CS.Common.Utilities.Email.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailClient _client;

        public EmailService(IEmailClient client)
        {
            _client = client;
        }

        public void Send(Models.Email message)
        {
            var mailMessage = new MailMessage
            {
                Subject = message.Subject,
                IsBodyHtml = message.IsBodyHtml,
                Priority = message.Priority,
                Body = message.Body,
                From = new MailAddress(message.From)
            };

            foreach (var to in message.To.Split(';'))
                mailMessage.To.Add(to.Trim());

            if (!string.IsNullOrWhiteSpace(message.CC))
            {
                foreach (var cc in message.CC.Split(';'))
                    mailMessage.CC.Add(cc.Trim());
            }

            _client.Send(mailMessage);
        }

        //private string GetSubjectForEnvironment(string subject)
        //{
        //    if (!_environmentService.Production())
        //    {
        //        return string.Concat($"({_environmentService.Environment()}) ", subject);
        //    }
        //    return subject;
        //}
    }
}
