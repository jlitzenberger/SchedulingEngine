using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.Domain.Naesb
{
    public class NaesbEventMonitor
    {
        public int Id { get; set; }
        public string Cycle { get; set; }
        public string CycleDescription { get; set; }
        public short SortSeq { get; set; }
        public string Pipeline { get; set; }
        public string Utility { get; set; }
        public string EventType { get; set; }
        public string EventTypeDescription { get; set; }
        public TimeSpan EventMonitorTime { get; set; }
        public DateTime? LastCheckedTime { get; set; }
        public DateTime ActiveStart { get; set; }
        public DateTime ActiveEnd { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string LastUpdateUserId { get; set; }

    }
}
