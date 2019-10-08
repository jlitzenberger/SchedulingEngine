namespace Nomination.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_naesb_pipelines
    {
        [Key]
        [StringLength(4)]
        public string PipelineCd { get; set; }

        [Required]
        [StringLength(50)]
        public string PipelineDesc { get; set; }

        [Required]
        [StringLength(17)]
        public string PipelineEntityId { get; set; }

        [StringLength(4)]
        public string PointCode { get; set; }

        public int? PipelineId { get; set; }

        public DateTime row_lst_upd_dttm { get; set; }

        [Required]
        [StringLength(8)]
        public string row_lst_upd_userid { get; set; }
    }
}
