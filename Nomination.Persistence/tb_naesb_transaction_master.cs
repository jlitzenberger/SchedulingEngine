namespace Nomination.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_naesb_transaction_master
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_naesb_transaction_master()
        {
            tb_naesb_transaction_detail = new HashSet<tb_naesb_transaction_detail>();
        }

        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal TransMasterId { get; set; }

        [Required]
        [StringLength(10)]
        public string FileType { get; set; }

        [Required]
        [StringLength(10)]
        public string PipelineCd { get; set; }

        [Required]
        [StringLength(3)]
        public string CompanyCd { get; set; }

        [Column(TypeName = "date")]
        public DateTime GasDay { get; set; }

        [Required]
        [StringLength(3)]
        public string CycleCd { get; set; }

        [Required]
        [StringLength(17)]
        public string ConfirmingEntityId { get; set; }

        [Required]
        [StringLength(17)]
        public string UtilityEntityId { get; set; }
        
        [StringLength(255)]
        public string TransportationId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? NomCycleStart { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? NomcycleEnd { get; set; }

        public DateTime? TransactionTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime row_lst_upd_dttm { get; set; }

        [Required]
        [StringLength(8)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string row_lst_upd_userid { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_naesb_transaction_detail> tb_naesb_transaction_detail { get; set; }
    }
}
