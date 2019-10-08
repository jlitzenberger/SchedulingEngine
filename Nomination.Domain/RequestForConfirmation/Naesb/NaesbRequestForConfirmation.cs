using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nomination.Domain.RequestForConfirmation.Naesb
{
    [XmlRoot("RequestForConfirmation")]
    public class NaesbRequestForConfirmation
    {
        public Header Header { get; set; }
        public string PurposeCode { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public PartyIndentificaton PartyIndentificaton { get; set; }
        public List<Nomination> Nominations { get; set; }
    }

    public class Header
    {
        [XmlElement(ElementName = "PartnerID")]
        public string PartnerId { get; set; }
        public string Standard { get; set; }
        public string Version { get; set; }
        public string TransactionSet { get; set; }
        public string EnvironmentFlag { get; set; }
    }
    public class PartyIndentificaton
    {
        public string ConfirmingPartyDuns { get; set; }
        public string UtilityDunsNumber { get; set; }
    }
    public class Nomination
    {
        public string Cycle { get; set; }
        public string BegToEndDate { get; set; }
        public string Location { get; set; }
        public string NomsContractNumber { get; set; }
        public List<NominationDetail> NominationDetails { get; set; }
    }
    public class NominationDetail
    {
        public string NominationId { get; set; }
        public string Quantity { get; set; }
        /// <summary>
        /// Flow of the gas from the utility's perspective.
        /// </summary>
        public string FlowIndicator { get; set; }
        public string UpstreamEntity { get; set; }
        public string UpstreamContractNumber { get; set; }
        public string UpstreamPackage { get; set; }
        public string DownstreamEntity { get; set; }
        public string DownstreamContractNumber { get; set; }
        public string DownstreamPackage { get; set; }
        public NomsContractInfo NomsContractInfo { get; set; }
    }
    public class NomsContractInfo
    {
        public string EntityId { get; set; }
        public string ContractId { get; set; }
    }
}
