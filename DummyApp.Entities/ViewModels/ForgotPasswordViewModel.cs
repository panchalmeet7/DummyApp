using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Entities.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please Enter a valid email!")]
        public string? Email { get; set; }
    }
}
