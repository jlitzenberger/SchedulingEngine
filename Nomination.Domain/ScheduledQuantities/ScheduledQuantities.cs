using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nomination.Domain.ScheduledQuantities
{
    public class ScheduledQuantities
    {
        public string PurchaseOrderNumber { get; set; }
        public string GasDayStart { get; set; }
        public string GasDayEnd { get; set; }
        public DateTime GasDay { get; set; }
        public string Cycle { get; set; }
        public PartyIndentificaton PartyIndentificaton { get; set; }
        public List<Location> Locations { get; set; }

        public ScheduledQuantities()
        {
            
        }
    }
    public class PartyIndentificaton
    {
        public string PipelineEntity { get; set; }
        public string UtilityEntity { get; set; }
    }
    public class Location
    {
        public string Id { get; set; }
        public List<ContractNomination> ContractNominations { get; set; }
    }
    public class ContractNomination
    {
        public string Id { get; set; }
        public List<Nomination> Nominations { get; set; }
    }
    public class Nomination
    {
        public string Id { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public string FlowIndicator { get; set; }
        public string SolicitedIndicator { get; set; }
        public string ReductionReason { get; set; }
        public Stream Stream { get; set; }
        public NomsContractInfo NomsContractInfo { get; set; }
    }
    public class Stream
    {
        public string Direction { get; set; }
        public string EntityId { get; set; }
        public string ContractId { get; set; }
        public string PackageId { get; set; }
    }
    public class NomsContractInfo
    {
        public string EntityId { get; set; }
        public string ContractId { get; set; }
    }
}