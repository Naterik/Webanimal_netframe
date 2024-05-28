namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Image")]
    public partial class Image
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string FileType { get; set; }

        [StringLength(100)]
        public string Link { get; set; }
        [DisplayName("Tên động vật")]
        public int AnimalID { get; set; }

        public virtual Animal Animal { get; set; }
    }
}
