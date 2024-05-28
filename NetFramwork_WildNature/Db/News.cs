namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Titile { get; set; }

        [StringLength(50)]
        public string Images { get; set; }

        public int ẠnimalID { get; set; }

        public string Decription { get; set; }

        public DateTime? Date { get; set; }

        public virtual Animal Animal { get; set; }
    }
}
