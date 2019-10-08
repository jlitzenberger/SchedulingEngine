namespace Nomination.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tb_company
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string company_code { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string company_short_desc { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string company_name { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int company_id { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(17)]
        public string company_entity_id { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime row_lst_upd_dttm { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(8)]
        public string row_lst_upd_userid { get; set; }
    }
}
