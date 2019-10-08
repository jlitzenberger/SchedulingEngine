namespace Nomination.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_naesb_event_monitor
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Pipeline { get; set; }

        [Required]
        [StringLength(3)]
        public string Utility { get; set; }

        [Required]
        [StringLength(10)]
        public string EventType { get; set; }

        [Required]
        [StringLength(3)]
        public string Cycle { get; set; }

        public short SortSeq { get; set; }

        [Required]
        [StringLength(255)]
        public string EventTypeDescription { get; set; }

        [Required]
        [StringLength(255)]
        public string CycleDescription { get; set; }

        public TimeSpan EventMonitorTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastCheckedTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ActiveStart { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ActiveEnd { get; set; }

        [Column(TypeName = "datetime2")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime row_lst_upd_dttm { get; set; }

        [Required]
        [StringLength(8)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string row_lst_upd_userid { get; set; }
    }
}
