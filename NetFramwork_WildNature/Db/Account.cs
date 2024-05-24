namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            Donates = new HashSet<Donate>();
            Volunteers = new HashSet<Volunteer>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        [DisplayName("Tên người dùng")]
        public string Name { get; set; }

        [StringLength(10)]
        [DisplayName("Mã tài khoản")]
        public string Code { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(50)]
        public string Email { get; set; }

        [DisplayName("Mật khẩu ")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [MinLength(4, ErrorMessage = "Mật khẩu không được ít hơn 4 ký tự")]
        [StringLength(50)]
        public string Password { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày tạo tài khoản")]
        public DateTime? CreateDate { get; set; }
        [DisplayName("Quyền ")]
        public int? RoleID { get; set; }
        [DisplayName("Trạng thái ")]
        public bool? State { get; set; }

        public virtual Role Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Donate> Donates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volunteer> Volunteers { get; set; }
    }
}
