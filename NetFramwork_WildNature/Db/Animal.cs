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
            FavoriteAnimals = new HashSet<FavoriteAnimal>();
            Images = new HashSet<Image>();
            News = new HashSet<News>();
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
        public int AreaID { get; set; }
        [DisplayName("Loại động vật")]
        public int CategoryID { get; set; }
        [DisplayName("Trạng thái")]
        public bool? State { get; set; }

        public string Decription { get; set; }
        [DisplayName("Tình trạng bảo tồn")]
        public int? ConservationID { get; set; }

        public virtual Category Category { get; set; }

        public virtual Conservation Conservation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AnimalDetail> AnimalDetails { get; set; }

        public virtual Area Area { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FavoriteAnimal> FavoriteAnimals { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News> News { get; set; }
    }
}
