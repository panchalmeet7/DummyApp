using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyApp.Entities.ViewModels
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Last Name is required!")]
        public string LastName { get; set; } = null!;
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please Enter a valid email!")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone Number is required!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Enter valid phone number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = null!;
       // [Required(ErrorMessage = "Please Choose Your Gender!")]
        public string? Gender { get; set; }
        [Required(ErrorMessage = "Please enter the password!")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage = "Confirm password is required!")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

    }
}
