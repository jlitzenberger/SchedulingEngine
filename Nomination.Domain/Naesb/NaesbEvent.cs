using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.Domain.Naesb
{
    public class NaesbEvent
    {
        public int Id { get; set; }
        public string FileType { get; set; }
        public string Cycle { get; set; }
        public string Pipeline { get; set; }
        public string Utility { get; set; }
        public DateTime? ProcessedTime { get; set; }
        public TimeSpan? CycleStart { get; set; }
        public TimeSpan? CycleEnd { get; set; }
        public short? OffSet { get; set; }
        public bool? On { get; set; }
        //public bool QuickResponse { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string LastUpdateUserId { get; set; }
    }
}
