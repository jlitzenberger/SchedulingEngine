using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.Domain.Naesb
{
    public class NaesbTransaction
    {
        public int Id { get; set; }
        public string FileType { get; set; }
        public string Pipeline { get; set; }
        public string Company { get; set; }
        public DateTime GasDay { get; set; }
        public string Cycle { get; set; }
        public string ConfirmingEntityId { get; set; }
        public string UtiltiyEntityId { get; set; }
        public string TransportationId { get; set; }
        public DateTime? CycleStart { get; set; }
        public DateTime? CycleEnd { get; set; }
        public DateTime? TransactionTime { get; set; }
        public string UserId { get; set; }
        public List<NaesbTransactionDetail> Details { get; set; }
    }
}
