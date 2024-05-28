namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Color")]
    public partial class Color
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Color()
        {
            AnimalDetails = new HashSet<AnimalDetail>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Mã màu")]
        public string Code { get; set; }

        [StringLength(50)]
        [DisplayName("Màu lông")]
        public string Name { get; set; }

        [StringLength(50)]
        [DisplayName("Màu lông cụ thể")]
        public string ColorSpecific { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnimalDetail> AnimalDetails { get; set; }
    }
}
