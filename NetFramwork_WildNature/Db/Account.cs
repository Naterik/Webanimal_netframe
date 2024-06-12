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

        public int ID { get; set; }

        [StringLength(50)]
        [DisplayName("Tên người dùng")]
        public string Name { get; set; }

        [StringLength(10)]
        [DisplayName("Mã tài khoản")]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Mật khẩu ")]
        public string Password { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày tạo ")]
        public DateTime? CreateDate { get; set; }
        [DisplayName("Quyền ")]

        public int? RoleID { get; set; }
        [DisplayName("Trạng thái ")]

        public bool? State { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<Donate> Donates { get; set; }

        public virtual ICollection<Volunteer> Volunteers { get; set; }
    }
}
