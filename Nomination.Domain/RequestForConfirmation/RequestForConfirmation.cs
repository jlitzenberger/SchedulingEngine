using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Nomination.Domain.RequestForConfirmation
{
    public class RequestForConfirmation
    {
        public string PurchaseOrderNumber { get; set; }
        /// <summary>
        /// [Gas Day Start: YYYYMMDDHHMM]
        /// </summary>
        public string GasDayStart { get; set; }
        /// <summary>
        /// [Gas Day End: YYYYMMDDHHMM]
        /// </summary>
        public string GasDayEnd { get; set; }
        /// <summary>
        /// [Gas Day: UTC 2019-06-25T00:00:00Z]
        /// </summary>
        [Required]
        public DateTime GasDay { get; set; }
        /// <summary>
        /// [Naesb Cycle: 
        /// TIM=Timely,
        /// EVE=Evening,
        /// ID1=Intra Day 1 (Same Day),
        /// ID2=Intra Day 2 (Same Day),
        /// ID3=Intra Day 3 (Same Day)]
        /// </summary>
        [Required]
        public string Cycle { get; set; }
        [Required]
        public PartyIndentificaton PartyIndentificaton { get; set; }
        public List<Location> Locations { get; set; }

        public RequestForConfirmation()
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
        /// <summary>
        /// [Flow of the gas from the utility's perspective: 
        /// R=Receipt,
        /// D=Delivery]
        /// </summary>
        public string FlowIndicator { get; set; }
        public Stream Stream { get; set; }
        public NomsContractInfo NomsContractInfo { get; set; }
    }
    public class Stream
    {
        /// <summary>
        /// [Direction the gas is going from the utility's perspective: U=Upstream/D=Downstream]
        /// </summary>
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
