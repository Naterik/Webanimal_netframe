namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Specie")]
    public partial class Specie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Specie()
        {
            AnimalDetails = new HashSet<AnimalDetail>();
        }

        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Mã loài")]
        public string Code { get; set; }

        [StringLength(50)]
        [DisplayName("Tên loài")]
        public string Name { get; set; }

        [StringLength(50)]
        [DisplayName("Loài cụ thể")]
        public string NameSpecific { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnimalDetail> AnimalDetails { get; set; }
    }
}
