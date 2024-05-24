namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Area")]
    public partial class Area
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Area()
        {
            Animals = new HashSet<Animal>();
        }

        public int ID { get; set; }

        [DisplayName("Mã khu vực")]
        [Required(ErrorMessage = "Mã khu vực không được để trống")]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        [DisplayName("Tên khu vực")]
        public string Name { get; set; }

        [StringLength(50)]
        [DisplayName("Địa điểm")]
        public string Location { get; set; }
        [DisplayName("Mô tả")]
        public string Decription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Animal> Animals { get; set; }
    }
}
