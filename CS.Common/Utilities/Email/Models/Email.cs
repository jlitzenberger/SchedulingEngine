using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CS.Common.Utilities.Email.Models
{
    public class Email : IEmail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }
        public MailPriority Priority { get; set; }
    }
}
