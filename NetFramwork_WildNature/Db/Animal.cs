namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
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
        public string Code { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? AreaID { get; set; }

        public int? ConservationStatusID { get; set; }

        public int? CategoryID { get; set; }

        public bool? State { get; set; }

        [StringLength(3000)]
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
