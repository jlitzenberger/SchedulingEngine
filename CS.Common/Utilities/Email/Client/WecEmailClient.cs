using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CS.Common.Utilities.Email.Settings;

namespace CS.Common.Utilities.Email.Client
{
    public class WecEmailClient : IEmailClient
    {
        private readonly IEmailSettings _settings;

        public WecEmailClient(IEmailSettings settings)
        {
            _settings = settings;
        }

        public void Send(MailMessage message)
        {
            using (SmtpClient client = new SmtpClient(_settings.Host, _settings.Port))
            {
                client.Send(message);
            }
        }
    }
}
