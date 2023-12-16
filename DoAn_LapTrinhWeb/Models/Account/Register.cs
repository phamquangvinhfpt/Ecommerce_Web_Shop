using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoAn_LapTrinhWeb.Models
{
    public class Register
    {
      
        [Required]
        public string username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        public string Name { get; set; }
        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Addres_1 { get; set; }
    }
}