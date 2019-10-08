namespace Nomination.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_naesb_transaction_detail
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal TransDetailId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal TransMasterId { get; set; }

        [Required]
        [StringLength(10)]
        public string RecordType { get; set; }

        [Required]
        [StringLength(17)]
        public string LocationCode { get; set; }

        [Required]
        [StringLength(50)]
        public string ConfirmationTrackingId { get; set; }

        public int? Quantity { get; set; }

        [StringLength(17)]
        public string SupplierEntityId { get; set; }

        [StringLength(17)]
        public string ContractId { get; set; }

        [StringLength(17)]
        public string ServiceRequesterId { get; set; }

        [StringLength(50)]
        public string PipelineContractId { get; set; }

        [StringLength(1)]
        public string SolicitedInd { get; set; }

        [StringLength(1)]
        public string Flow { get; set; }

        [StringLength(3)]
        public string ReductionReason { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime row_lst_upd_dttm { get; set; }

        [Required]
        [StringLength(8)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string row_lst_upd_userid { get; set; }

        public virtual tb_naesb_transaction_master tb_naesb_transaction_master { get; set; }
    }
}
