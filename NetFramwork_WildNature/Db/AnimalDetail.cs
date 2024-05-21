namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnimalDetail")]
    public partial class AnimalDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AnimalDetail()
        {
            Images = new HashSet<Image>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string Weight { get; set; }

        [StringLength(50)]
        public string Height { get; set; }

        [StringLength(50)]
        public string Origin { get; set; }

        public int? ColorID { get; set; }

        public int? AnimalID { get; set; }

        public int? SpecieID { get; set; }

        public virtual Animal Animal { get; set; }

        public virtual Color Color { get; set; }

        public virtual Specie Specie { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }
    }
}
