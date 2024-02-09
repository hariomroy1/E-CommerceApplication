using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class RegisterEntity
    {
        [Key]
        [Required]
        public int RegisterId { get; set; }
        [Required(ErrorMessage = "Please enter your Full Name")]
        [Display(Name = "Full Name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please enter your Email Address")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please provide a valid Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please enter your Phone Number")]
        [Display(Name = "Phone Number")]
        public long Phone { get; set; }


        [Required(ErrorMessage = "Please enter your Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password, ErrorMessage = "please provide a valid password")]
        public string? Password { get; set; }

        public DateTime MemberSince { get; set; }
     

    }
}
