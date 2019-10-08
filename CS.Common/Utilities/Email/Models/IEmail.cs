using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CS.Common.Utilities.Email.Models
{
    public interface IEmail
    {
        string Body { get; set; }
        string CC { get; set; }
        string From { get; set; }
        bool IsBodyHtml { get; set; }
        MailPriority Priority { get; set; }
        string Subject { get; set; }
        string To { get; set; }
    }
}
