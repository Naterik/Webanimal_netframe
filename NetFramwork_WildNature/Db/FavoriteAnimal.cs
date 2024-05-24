namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FavoriteAnimal")]
    public partial class FavoriteAnimal
    {
        public int ID { get; set; }
        [DisplayName("Tên động vật")]
        public int AnimalID { get; set; }
        [DisplayName("Tên tình nguyện")]
        public int VolunteerID { get; set; }

        public virtual Animal Animal { get; set; }

        public virtual Volunteer Volunteer { get; set; }
    }
}
