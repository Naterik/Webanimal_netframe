namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Donate")]
    public partial class Donate
    {
        public int ID { get; set; }

        public double? Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public int? VolunteerID { get; set; }

        public bool? State { get; set; }

        public virtual Volunteer Volunteer { get; set; }
    }
}
