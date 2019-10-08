using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nomination.Domain.QuickResponse.Naesb
{
    [XmlRoot("QuickResponse")]
    public class NaesbQuickResponse
    {
        public Header Header { get; set; }
        public string PurposeCode { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public PartyIndentificaton PartyIndentificaton { get; set; }
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
}
