using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Nomination.Persistence.RequestForConfirmation.Dto
{
    public class RequestForConfirmation
    {
        public string PurchaseOrderNumber { get; set; }
        public string GasDayStart { get; set; }
        public string GasDayEnd { get; set; }
        public string GasDay { get; set; }
        public string Cycle { get; set; }

        public PartyIndentificaton PartyIndentificaton { get; set; }
        public List<Location> Locations { get; set; }

        public RequestForConfirmation()
        {

        }
    }

    public class PartyIndentificaton
    {
        public string PipelineEntityId { get; set; }
        public string UtilityEntityId { get; set; }
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
        public string FlowIndicator { get; set; }
        public Stream Stream { get; set; }
        public NomsContractInfo NomsContractInfo { get; set; }
    }
    public class Stream
    {
        public string Direction { get; set; }
        public string EntityId { get; set; }
        public string ContractId { get; set; }
    }
    public class NomsContractInfo
    {
        public string EntityId { get; set; }
        public string ContractId { get; set; }
    }






    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class HdrRecs
    {

        private HdrRecsHDR[] hDRField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("HDR")]
        public HdrRecsHDR[] HDR
        {
            get { return this.hDRField; }
            set { this.hDRField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HdrRecsHDR
    {

        private string partner_idField;

        private string standardField;

        private ushort versionField;

        private ushort transaction_setField;

        private string test_production_flagField;

        private HdrRecsHDRAdmRecs admRecsField;

        /// <remarks/>
        public string partner_id
        {
            get { return this.partner_idField; }
            set { this.partner_idField = value; }
        }

        /// <remarks/>
        public string standard
        {
            get { return this.standardField; }
            set { this.standardField = value; }
        }

        /// <remarks/>
        public ushort version
        {
            get { return this.versionField; }
            set { this.versionField = value; }
        }

        /// <remarks/>
        public ushort transaction_set
        {
            get { return this.transaction_setField; }
            set { this.transaction_setField = value; }
        }

        /// <remarks/>
        public string test_production_flag
        {
            get { return this.test_production_flagField; }
            set { this.test_production_flagField = value; }
        }

        /// <remarks/>
        public HdrRecsHDRAdmRecs AdmRecs
        {
            get { return this.admRecsField; }
            set { this.admRecsField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HdrRecsHDRAdmRecs
    {

        private HdrRecsHDRAdmRecsADM aDMField;

        /// <remarks/>
        public HdrRecsHDRAdmRecsADM ADM
        {
            get { return this.aDMField; }
            set { this.aDMField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HdrRecsHDRAdmRecsADM
    {

        private byte purpose_codeField;

        private string purchase_order_numberField;

        private uint confirmation_party_dunsField;

        private uint utility_dunsField;

        private HdrRecsHDRAdmRecsADMTOP[] topRecsField;

        /// <remarks/>
        public byte purpose_code
        {
            get { return this.purpose_codeField; }
            set { this.purpose_codeField = value; }
        }

        /// <remarks/>
        public string purchase_order_number
        {
            get { return this.purchase_order_numberField; }
            set { this.purchase_order_numberField = value; }
        }

        /// <remarks/>
        public uint confirmation_party_duns
        {
            get { return this.confirmation_party_dunsField; }
            set { this.confirmation_party_dunsField = value; }
        }

        /// <remarks/>
        public uint utility_duns
        {
            get { return this.utility_dunsField; }
            set { this.utility_dunsField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("TOP", IsNullable = false)]
        public HdrRecsHDRAdmRecsADMTOP[] TopRecs
        {
            get { return this.topRecsField; }
            set { this.topRecsField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HdrRecsHDRAdmRecsADMTOP
    {

        private byte confirmation_cycle_indField;

        private string begtoend_dateField;

        private ushort locationField;

        private string nomscontractnoField;

        private HdrRecsHDRAdmRecsADMTOPDET[] detRecsField;

        /// <remarks/>
        public byte confirmation_cycle_ind
        {
            get { return this.confirmation_cycle_indField; }
            set { this.confirmation_cycle_indField = value; }
        }

        /// <remarks/>
        public string begtoend_date
        {
            get { return this.begtoend_dateField; }
            set { this.begtoend_dateField = value; }
        }

        /// <remarks/>
        public ushort location
        {
            get { return this.locationField; }
            set { this.locationField = value; }
        }

        /// <remarks/>
        public string nomscontractno
        {
            get { return this.nomscontractnoField; }
            set { this.nomscontractnoField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DET", IsNullable = false)]
        public HdrRecsHDRAdmRecsADMTOPDET[] DetRecs
        {
            get { return this.detRecsField; }
            set { this.detRecsField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HdrRecsHDRAdmRecsADMTOPDET
    {

        private string nomination_idField;

        private byte quantityField;

        private uint upstream_entityField;

        private uint upstream_contract_numberField;

        private HdrRecsHDRAdmRecsADMTOPDETDt2Recs dt2RecsField;

        /// <remarks/>
        public string nomination_id
        {
            get { return this.nomination_idField; }
            set { this.nomination_idField = value; }
        }

        /// <remarks/>
        public byte quantity
        {
            get { return this.quantityField; }
            set { this.quantityField = value; }
        }

        /// <remarks/>
        public uint upstream_entity
        {
            get { return this.upstream_entityField; }
            set { this.upstream_entityField = value; }
        }

        /// <remarks/>
        public uint upstream_contract_number
        {
            get { return this.upstream_contract_numberField; }
            set { this.upstream_contract_numberField = value; }
        }

        /// <remarks/>
        public HdrRecsHDRAdmRecsADMTOPDETDt2Recs Dt2Recs
        {
            get { return this.dt2RecsField; }
            set { this.dt2RecsField = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HdrRecsHDRAdmRecsADMTOPDETDt2Recs
    {

        private HdrRecsHDRAdmRecsADMTOPDETDt2RecsDT2 dT2Field;

        /// <remarks/>
        public HdrRecsHDRAdmRecsADMTOPDETDt2RecsDT2 DT2
        {
            get { return this.dT2Field; }
            set { this.dT2Field = value; }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HdrRecsHDRAdmRecsADMTOPDETDt2RecsDT2
    {

        private string qty_typeField;

        private uint servrqstrdunsField;

        private string service_requester_contractField;

        private uint noms_confirmation_service_entity_codeField;

        /// <remarks/>
        public string qty_type
        {
            get { return this.qty_typeField; }
            set { this.qty_typeField = value; }
        }

        /// <remarks/>
        public uint servrqstrduns
        {
            get { return this.servrqstrdunsField; }
            set { this.servrqstrdunsField = value; }
        }

        /// <remarks/>
        public string service_requester_contract
        {
            get { return this.service_requester_contractField; }
            set { this.service_requester_contractField = value; }
        }

        /// <remarks/>
        public uint noms_confirmation_service_entity_code
        {
            get { return this.noms_confirmation_service_entity_codeField; }
            set { this.noms_confirmation_service_entity_codeField = value; }
        }
    }
}
