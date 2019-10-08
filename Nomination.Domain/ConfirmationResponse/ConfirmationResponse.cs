using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nomination.Domain.ConfirmationResponse
{
    public class ConfirmationResponse
    {
        public string PurchaseOrderNumber { get; set; }
        public string GasDayStart { get; set; }
        public string GasDayEnd { get; set; }
        public DateTime GasDay { get; set; }
        public string Cycle { get; set; }

        public PartyIndentificaton PartyIndentificaton { get; set; }
        public List<Location> Locations { get; set; }

        public ConfirmationResponse()
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
        //[XmlElement(IsNullable = true)]
        public string Direction { get; set; }
        //[XmlElement(IsNullable = true)]
        public string EntityId { get; set; }
        //[XmlElement(IsNullable = true)]
        public string ContractId { get; set; }
        //[XmlElement(IsNullable = true)]
        public string PackageId { get; set; }
    }
    public class NomsContractInfo
    {
        public string EntityId { get; set; }
        public string ContractId { get; set; }
    }
}