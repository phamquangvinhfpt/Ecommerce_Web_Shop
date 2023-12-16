using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Provinces")]
    public class Provinces
    {
        //tag id
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int province_id { get; set; }
        [Required(ErrorMessage = "Nhập tên Tỉnh/Thành Phố")]
        //tag name
        [StringLength(50)]
        public string province_name { get; set; }

        [Required]
        [StringLength(20)]
        public string type { get; set; }
        public virtual ICollection<Districts> Districts { get; set; }
        public virtual ICollection<AccountAddress> AccountAddresses { get; set; }
        public virtual ICollection<OrderAddress> OrderAddress { get; set; }
    }
}