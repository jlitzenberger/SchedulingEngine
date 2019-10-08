using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.Domain.Naesb
{
    public class NaesbEventProcess
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime? GasDay { get; set; }
        public string Cycle { get; set; }
        public string Pipeline { get; set; }
        public string Utility { get; set; }
        public DateTime? ProcessStart { get; set; }
        public DateTime? ProcessEnd { get; set; }
        public string EdiFileName { get; set; }
        public string EdiData { get; set; }
        public string DomainData { get; set; }
        public string StackTrace { get; set; }
        public string UserId { get; set; }
    }
}
