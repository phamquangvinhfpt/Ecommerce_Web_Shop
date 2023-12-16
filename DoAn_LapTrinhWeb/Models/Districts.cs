using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Districts")]
    public class Districts
    {
        //district id
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int district_id { get; set; }
        [Required]
        //city id
        public int province_id { get; set; }
        //district name
        [Required]
        [StringLength(50)]
        public string district_name { get; set; }
        //district type
        [Required]
        [StringLength(20)]
        public string type { get; set; }
        public virtual Provinces Provinces { get; set; }
        public virtual ICollection<Wards> Wards { get; set; }
        public virtual ICollection<AccountAddress> AccountAddresses { get; set; }
        public virtual ICollection<OrderAddress> OrderAddress { get; set; }


    }
}