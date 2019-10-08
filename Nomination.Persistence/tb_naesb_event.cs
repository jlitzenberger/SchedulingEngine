namespace Nomination.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_naesb_event
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal EventId { get; set; }

        [Required]
        [StringLength(10)]
        public string FileType { get; set; }

        [Required]
        [StringLength(10)]
        public string PipelineCd { get; set; }

        [Required]
        [StringLength(3)]
        public string CompanyCd { get; set; }

        [Required]
        [StringLength(3)]
        public string CycleCd { get; set; }

        public DateTime? ProcessedTime { get; set; }

        public TimeSpan? CycleStart { get; set; }

        public TimeSpan? CycleEnd { get; set; }

        public short? OffSet { get; set; }

        [Column("On/Off")]
        public bool? On_Off { get; set; }

        [Column(TypeName = "datetime2")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? row_lst_upd_dttm { get; set; }

        [Required]
        [StringLength(8)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string row_lst_upd_userid { get; set; }

        //public bool QuickResponse { get; set; }
    }
}
