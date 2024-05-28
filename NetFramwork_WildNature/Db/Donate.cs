namespace NetFramwork_WildNature.Db
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Donate")]
    public partial class Donate
    {
        public int ID { get; set; }
        [DisplayName("Số tiền")]

        public double? Amount { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Ngày chuyển")]
        public DateTime? Date { get; set; }
        [DisplayName("Tên tình nguyện ")]

        public int? VolunteerID { get; set; }
        [DisplayName("Trạng thái")]

        public virtual Account Account { get; set; }
    }
}
