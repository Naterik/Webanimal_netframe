namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Animal")]
    public partial class Animal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Animal()
        {
            AnimalDetails = new HashSet<AnimalDetail>();
            News = new HashSet<News>();
            Volunteers = new HashSet<Volunteer>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Mã động vật")]
        public string Code { get; set; }

        [StringLength(50)]
        [DisplayName("Tên động vật")]
        public string Name { get; set; }
        [DisplayName("Khu vực")]
        public int? AreaID { get; set; }
        [DisplayName("Tình trạng bảo tồn")]
        public int? ConservationStatusID { get; set; }
        [DisplayName("Loại động vật")]
        public int? CategoryID { get; set; }
        [DisplayName("Trạng thái")]
        public bool? State { get; set; }

        [StringLength(50)]
        [DisplayName("Mô tả động vật")]
        public string Description { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnimalDetail> AnimalDetails { get; set; }

        public virtual Area Area { get; set; }

        public virtual ConservationStatu ConservationStatu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News> News { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volunteer> Volunteers { get; set; }
    }
}
