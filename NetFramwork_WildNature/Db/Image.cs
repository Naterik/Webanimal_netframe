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
        [DisplayName("Tên ảnh")]
        public string Name { get; set; }

        [StringLength(50)]
        [DisplayName("Loại ảnh")]
        public string FileType { get; set; }

        [StringLength(100)]
        public string Link { get; set; }

        public int AnimalDetailID { get; set; }

        public virtual AnimalDetail AnimalDetail { get; set; }
    }
}
