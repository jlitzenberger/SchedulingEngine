using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.Domain.Naesb
{
    public class NaesbTransactionDetail
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public string RecordType { get; set; }
    }
}
