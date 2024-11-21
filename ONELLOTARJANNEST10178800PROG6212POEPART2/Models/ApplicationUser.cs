using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ONELLOTARJANNEST10178800PROG6212POEPART2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}