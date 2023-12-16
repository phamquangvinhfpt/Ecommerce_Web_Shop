using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("OrderAddress")]
    public class OrderAddress
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int orderAddressId { get; set; }
        //tỉnh thành phố
        public int? province_id { get; set; }
        //quận, huyện
        public int? district_id { get; set; }
        //phường xã
        public int? ward_id { get; set; }
        [StringLength(10)]
        public string orderPhonenumber { get; set; }
        //số diện
        [StringLength(20)]
        public string orderUsername { get; set; }
        [StringLength(150)]
        public string content { get; set; }
        public int timesEdit { get; set; }
        public virtual Wards Wards { get; set; }
        public virtual Districts Districts { get; set; }
        public virtual Provinces Provinces { get; set; }
    }
}