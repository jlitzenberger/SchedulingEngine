using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchedulingEngine.Web.Models
{
    public class NaesbEvent2
    {
        public string Id { get; set; }
        public string FileType { get; set; }
        public string Cycle { get; set; }
        public string Pipeline { get; set; }
        public string Utility { get; set; }
        public string ProcessedTime { get; set; }
        public string CycleStart { get; set; }
        public string CycleEnd { get; set; }
        public string OffSet { get; set; }
        public string On { get; set; }
        public string LastUpdateTime { get; set; }
        public string LastUpdateUserId { get; set; }
    }
}