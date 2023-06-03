using System.ComponentModel.DataAnnotations;

namespace CoraetionTask.ViewModels
{
    public class RegisterationViewModel
    {
        [Required(ErrorMessage = "FullName is required")]
        public string FullName  { get; set; }

        
        [Required(ErrorMessage = "Mobile is required")]
        [StringLength(11, ErrorMessage = "Mobile cannot exceed 11 characters")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
