namespace Nomination.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_naesb_event_process
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal EventProcessId { get; set; }
        
        [StringLength(10)]
        public string FileType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? GasDay { get; set; }

        [StringLength(3)]
        public string CycleCd { get; set; }

        [StringLength(10)]
        public string PipelineCd { get; set; }

        [StringLength(3)]
        public string CompanyCd { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ProcessStart { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ProcessEnd { get; set; }

        [StringLength(255)]
        public string EDIFileName { get; set; }

        [Column(TypeName = "xml")]
        public string EDIData { get; set; }

        [Column(TypeName = "xml")]
        public string BUSData { get; set; }

        [Column(TypeName = "text")]
        public string StackTrace { get; set; }

        [Column(TypeName = "datetime2")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime row_lst_upd_dttm { get; set; }
        
        [StringLength(8)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string row_lst_upd_userid { get; set; }
    }
}
