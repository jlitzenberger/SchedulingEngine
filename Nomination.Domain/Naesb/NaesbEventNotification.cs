using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.Domain.Naesb
{
    public class NaesbEventNotification
    {
        public int Id { get; set; }
        public string Source { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Cc { get; set; }
        public string Priority { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public DateTime? ProcessedTime { get; set; }
        public DateTime? GasDay { get; set; }
        public string Pipeline { get; set; }
        public string Utility { get; set; }
        public string Cycle { get; set; }

        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }

        public DateTime LastUpdateTime { get; set; }
        public string LastUpdateUserId { get; set; }

    }
}
