using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{


    public partial class Oder_Detail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int product_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int genre_id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int disscount_id { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int order_id { get; set; }

        public double price { get; set; }

        [StringLength(1)]
        public string status { get; set; }

        [Required]
        [StringLength(50)]
        public string transection { get; set; }
        public int quantity { get; set; }
        [Required]
        [StringLength(20)]
        public string create_by { get; set; }

        public DateTime create_at { get; set; }

        [Required]
        [StringLength(20)]
        public string update_by { get; set; }

        public DateTime update_at { get; set; }


        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
