namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FavoriteAnimal")]
    public partial class FavoriteAnimal
    {
        public int ID { get; set; }

        public int AnimalID { get; set; }

        public int VolunteerID { get; set; }

        public virtual Animal Animal { get; set; }

        public virtual Volunteer Volunteer { get; set; }
    }
}
