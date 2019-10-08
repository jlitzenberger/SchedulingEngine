using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.Domain.Naesb
{
    public class NaesbPipeline
    {
        public string Pipeline { get; set; }
        public string PipelineDescription { get; set; }
        public string PipelineEntityId { get; set; }
        public string PointCode { get; set; }
        public int? PipelineId { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string LastUpdateUserId { get; set; }

    }
}
