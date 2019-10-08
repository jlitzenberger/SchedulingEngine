using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CS.Common.Utilities.Email.Client
{
    public interface IEmailClient
    {
        void Send(MailMessage message);
    }
}
