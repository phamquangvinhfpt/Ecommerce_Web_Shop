using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using DoAn_LapTrinhWeb.Model;


namespace DoAn_LapTrinhWeb.Models
{
    [Table("Product")]
    public class Product
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            Feedbacks = new HashSet<Feedback>();
            Oder_Detail = new HashSet<Oder_Detail>();
            ProductImages = new HashSet<ProductImages>();
        }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key] [Column(Order = 0)] public int product_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "Vui lòng chọn thể loại")]
        public int genre_id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int brand_id { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required(ErrorMessage = "Vui lòng chọn chương trình giảm giá")]
        public int disscount_id { get; set; }


        [StringLength(200, ErrorMessage = "Tên sản phẩm không được quá 200 ký tự")]
        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        public string product_name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        public double price { get; set; }

        public long view { get; set; }

        public long buyturn { get; set; }


        private string _quantity;
        [StringLength(10)]
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        public string quantity
        {
            get { return ((this._quantity != "" && this._quantity != null) ? this._quantity.Trim() : this._quantity); }
            set { this._quantity = (value == null) ? "" : value.Trim(); }
        }

        [StringLength(1)] public string status { get; set; }

        [Required] [StringLength(100)] public string create_by { get; set; }

        public DateTime create_at { get; set; }
        [StringLength(100)]
        public string update_by { get; set; }

        public DateTime update_at { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập loại sản phẩm")]
        public int? type { get; set; }

        public string specifications { get; set; }

        public string image { get; set; }

        public string description { get; set; }


        public virtual Brand Brand { get; set; }

        public virtual Discount Discount { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }

        public virtual Genre Genre { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Oder_Detail> Oder_Detail { get; set; }

        public virtual ICollection<ProductImages> ProductImages { get; set; }

        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }


        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]
        [NotMapped]
        public HttpPostedFileBase[] ImageUploadMulti { get; set; }
    }
}
//public class ValidateFileAttribute : RequiredAttribute
//{
//    public override bool IsValid(object value)
//    {
//        var file = value as HttpPostedFileBase;
//        if (file == null)
//        {
//            return false;
//        }

//        if (file.ContentLength > 1 * 1024 * 1024)
//        {
//            return false;
//        }

//        //try
//        //{
//        //    using (var img = Image.FromStream(file.InputStream))
//        //    {
//        //        return img.RawFormat.Equals(ImageFormat.Png);
//        //    }
//        //}
//        //catch { }
//        return true;
//    }
//}