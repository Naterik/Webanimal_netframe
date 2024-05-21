namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Volunteer")]
    public partial class Volunteer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Volunteer()
        {
            Donates = new HashSet<Donate>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int? AnimalID { get; set; }

        public int? RoleID { get; set; }

        public virtual Animal Animal { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Donate> Donates { get; set; }

        public virtual Role Role { get; set; }
    }
}
