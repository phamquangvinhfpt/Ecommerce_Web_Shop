using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("Delivery")]
    public class Delivery
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Delivery()
        {
            Orders = new HashSet<Order>();
        }

        [Key] public int delivery_id { get; set; }

        [Required] [StringLength(100)] public string delivery_name { get; set; }

        [Column(TypeName = "money")] public decimal price { get; set; }

        public DateTime create_at { get; set; }

        [Required] [StringLength(20)] public string create_by { get; set; }

        [StringLength(1)] public string status { get; set; }

        [Required] [StringLength(20)] public string update_by { get; set; }

        public DateTime? update_at { get; set; }


        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}