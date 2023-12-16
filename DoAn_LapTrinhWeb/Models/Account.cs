using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DoAn_LapTrinhWeb.Model;

namespace DoAn_LapTrinhWeb.Models
{
    [Table("Account")]
    public class Account
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            Feedbacks = new HashSet<Feedback>();
            Orders = new HashSet<Order>();
            ReplyFeedbacks = new HashSet<ReplyFeedback>();
        }
        [Key] public int account_id { get; set; }
        // [Required(ErrorMessage = "Nhập mật khẩu")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])).{8,}$", 
        ErrorMessage = "Mật khẩu tổi thiếu 8 ký tự bao gồm: chữ thường, chừ hoa, và chữ số")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required(ErrorMessage = "Nhập Email")]
        [StringLength(100, ErrorMessage = "Email tối thiểu 6 ký tự", MinimumLength = 6)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
        public string Email { get; set; }
        public string Requestcode { get; set; }
         public int Role { get; set; }
        [Required(ErrorMessage = "Nhập họ tên")]
        [StringLength(50)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [StringLength(10, ErrorMessage = "Số điện thoại phải đúng 10 chữ số", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Column(TypeName = "text")]
        public string Avatar { get; set; }
        [StringLength(100)]
        public string create_by { get; set; }
        public DateTime create_at { get; set; }
        [StringLength(100)]
        public string update_by { get; set; }
        public DateTime update_at { get; set; }
        [StringLength(1)] public string status { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplyFeedback> ReplyFeedbacks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}