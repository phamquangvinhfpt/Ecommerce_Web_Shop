using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Brand")]
    public class Brand
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Brand()
        {
            Products = new HashSet<Product>();
        }
        [Key] public int brand_id { get; set; }
        [Required] [StringLength(50)] public string brand_name { get; set; }
        [Required] [StringLength(100)] public string create_by { get; set; }
        public DateTime create_at { get; set; }
        [Required] [StringLength(100)] public string update_by { get; set; }
        public DateTime update_at { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}