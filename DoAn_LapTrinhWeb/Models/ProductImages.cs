using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductImages")]
    public partial class ProductImages
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int product_img_id { get; set; }

        public int product_id { get; set; }

        public int genre_id { get; set; }
        public int disscount_id { get; set; }

        [StringLength(500)]
        public string image { get; set; }

        public virtual Product Product { get; set; }

    }
}
