using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Feedback")]
    public partial class Feedback
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Feedback()
        {
            ReplyFeedbacks = new HashSet<ReplyFeedback>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int feedback_id { get; set; }

        public int account_id { get; set; }

        public int product_id { get; set; }

        public int genre_id { get; set; }

        public int disscount_id { get; set; }

        public string content { get; set; }

        [Required]
        public int rate_star { get; set; }

        public DateTime create_at { get; set; }

        [Required]
        [StringLength(100)]
        public string create_by { get; set; }

        [StringLength(1)]
        public string stastus { get; set; }

        [Required]
        [StringLength(100)]
        public string update_by { get; set; }

        public DateTime? update_at { get; set; }

        public virtual Account Account { get; set; }

        public virtual Product Product { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReplyFeedback> ReplyFeedbacks { get; set; }
    }
}
