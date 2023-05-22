using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Entities.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please Enter a valid email!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Please enter the password!")]


        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public int UserId { get; set; }
    }
}
