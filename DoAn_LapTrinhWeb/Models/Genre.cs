using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{
    [Table("Genre")]
    public class Genre
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Genre()
        {
            Products = new HashSet<Product>();
        }

        [Key] public int genre_id { get; set; }

        [Required] [StringLength(50)] public string genre_name { get; set; }


        public DateTime create_at { get; set; }

        [Required] [StringLength(100)] public string create_by { get; set; }

        [Required] [StringLength(100)] public string update_by { get; set; }

        public DateTime update_at { get; set; }


        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }
    }
}