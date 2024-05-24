namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            Animals = new HashSet<Animal>();
        }

        public int ID { get; set; }

        [DisplayName("Mã loại động vật")]
        [Required(ErrorMessage = "Mã không được để trống")]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(50)]
        [DisplayName("Tên loại động vật")]
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Animal> Animals { get; set; }
    }
}
