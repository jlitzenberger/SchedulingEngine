using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nomination.Domain.ScheduledQuantities.Naesb
{
    [XmlRoot("OperatorScheduledQuantity")]
    public class NaesbScheduledQuantities
    {
        public Header Header { get; set; }
        public List<ConfirmingParty> ConfirmingParties { get; set; }
        public List<Nomination> Nominations { get; set; }
    }

    public class Header
    {
        public string TransactionSetPurposeCode { get; set; }
        public string TransactionIdentifier { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTypeCode { get; set; }
    }
    public class ConfirmingParty
    {
        public string EntityIdentifierCode { get; set; }
        public string ConfirmingId { get; set; }
    }
    public class Nomination
    {
        public string DateTimeQualifier { get; set; }
        public string DateTimeFormatQualifier { get; set; }
        public string DateTimePeriod { get; set; }
        public Cycle Cycle { get; set; }
        public Location Location { get; set; }
    }
    public class Cycle
    {
        public string Qualifier { get; set; }
        public string Indicator { get; set; }
    }
    public class Location
    {
        public string GasLocation { get; set; }
        public string IdCodeQualifier { get; set; }
        public string Code { get; set; }
        public ContractSummary ContractSummary { get; set; }
        public List<NominationDetail> NominationDetails { get; set; }
    }
    public class ContractSummary
    {
        public string Contract { get; set; }
        public ContractIdentification ContractIdentification { get; set; }
    }
    public class ContractIdentification
    {
        public string IdentifierCode { get; set; }
        public string IdentificationCodeQualifier { get; set; }
        public string IdentificationId { get; set; }
    }
    public class NominationDetail
    {
        //[XmlElement(ElementName = "ConfirmationTrackingID")]
        public string ConfirmationTrackingId { get; set; }
        public string RelationshipCode { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }

        public List<AdditionalInformation> AdditionalInformations { get; set; }
        public List<Identifier> Identifiers { get; set; }
    }
    public class AdditionalInformation
    {
        public string Indentifier { get; set; }
        public string Code { get; set; }
    }
    public class Identifier
    {
        public string EntityIdCode { get; set; }
        public string IdCodeQualifier { get; set; }
        public string Code { get; set; }
        public List<IdentifierDetail> IdentifierDetails { get; set; }
    }
    public class IdentifierDetail
    {
        public string Qualifier { get; set; }
        public string Code { get; set; }
    }
}
