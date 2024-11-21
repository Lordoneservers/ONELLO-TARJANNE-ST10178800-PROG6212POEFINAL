using System.ComponentModel.DataAnnotations;

namespace ONELLOTARJANNEST10178800PROG6212POEPART2.Controllers
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string Role { get; set; }

        // Add phone number
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}


