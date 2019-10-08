namespace Nomination.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_naesb_event_notifications
    {
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Source { get; set; }

        [Required]
        [StringLength(2000)]
        public string To { get; set; }

        [Required]
        [StringLength(500)]
        public string From { get; set; }

        [StringLength(2000)]
        public string CC { get; set; }

        [Required]
        [StringLength(255)]
        public string Priority { get; set; }

        [Required]
        [StringLength(255)]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public bool Html { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ProcessedTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GasDay { get; set; }

        [StringLength(10)]
        public string PipelineCd { get; set; }

        [StringLength(3)]
        public string CompanyCd { get; set; }

        [StringLength(3)]
        public string CycleCd { get; set; }

        [StringLength(255)]
        public string Ref1 { get; set; }

        [StringLength(255)]
        public string Ref2 { get; set; }

        [StringLength(255)]
        public string Ref3 { get; set; }

        [Column(TypeName = "datetime2")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime row_cr_dttm { get; set; }

        [Required]
        [StringLength(8)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string row_cr_userid { get; set; }
    }
}
