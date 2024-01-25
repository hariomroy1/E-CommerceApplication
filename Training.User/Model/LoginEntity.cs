using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class LoginEntity
    {
        [Required(ErrorMessage = "Please enter your Email Address")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "please provide a valid ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Password")]
        [Display(Name = "Password")]
        [DataType(DataType.Password, ErrorMessage = "Please Provide a valid Password")]
        public string Password { get; set; }
    }
}
