using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Wards")]
    public class Wards
    {
        //subdistrict id
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ward_id { get; set; }
        //district id
        [Required]
        public int district_id { get; set; }
        //tag name
        [Required]
        [StringLength(50)]
        public string ward_name { get; set; }
        //type
        [Required]
        [StringLength(20)]
        public string type { get; set; }
        public virtual Districts Districts { get; set; }
        public virtual ICollection<AccountAddress> AccountAddresses { get; set; }
        public virtual ICollection<OrderAddress> OrderAddress { get; set; }

    }
}