using System.ComponentModel.DataAnnotations;

namespace ONELLOTARJANNEST10178800PROG6212POEPART2.Controllers
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

