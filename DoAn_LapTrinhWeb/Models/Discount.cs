using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("Discount")]
    public class Discount
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Discount()
        {
            Products = new HashSet<Product>();
        }

        [Key] public int disscount_id { get; set; }

        [Required] [StringLength(100)] public string discount_name { get; set; }

        public DateTime discount_star { get; set; }

        public DateTime discount_end { get; set; }

        public double discount_price { get; set; }
        public int quantity { get; set; }
        [StringLength(10)] public string discount_code { get; set; }

        public DateTime create_at { get; set; }

        [Required] [StringLength(100)] public string create_by { get; set; }


        [Required] [StringLength(100)] public string update_by { get; set; }

        public DateTime update_at { get; set; }


        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}