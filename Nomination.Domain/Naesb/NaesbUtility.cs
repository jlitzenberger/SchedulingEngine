using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.Domain.Naesb
{
    public class NaesbUtility
    {
        public string Utility { get; set; }
        public string UtilityShortDescription { get; set; }
        public string UtilityName { get; set; }
        public int UtilityId { get; set; }
        public string UtilityEntityId { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string LastUpdateUserId { get; set; }
    }
}
