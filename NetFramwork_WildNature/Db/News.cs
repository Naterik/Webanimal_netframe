namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        public int ID { get; set; }

        [StringLength(50)]
        [DisplayName("Tựa đề")]
        public string Titile { get; set; }

        [StringLength(50)]
        [DisplayName("Ảnh")]
        public string Images { get; set; }
        [DisplayName("Tên động vật")]
        public int ẠnimalID { get; set; }
        [DisplayName("Nội dung")]
        public string Decription { get; set; }
        [DisplayName("Ngày tạo")]
        public DateTime? Date { get; set; }

        public virtual Animal Animal { get; set; }
    }
}
