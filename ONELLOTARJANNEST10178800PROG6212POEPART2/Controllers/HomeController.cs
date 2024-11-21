using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Models;
using System.Diagnostics;

namespace ONELLOTARJANNEST10178800PROG6212POEPART2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        
             public IActionResult ClaimSuccess()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Claim()
        {
            return View();
        }
        [Authorize(Roles = "HR")]
        public IActionResult HR()
        {
            return View();
        }
        [AllowAnonymous]  // Allow anonymous access to the Access Denied page
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult ClaimView()
        {
            return View();
        }
        [Authorize(Roles = "Lecturer")]
        public IActionResult Post()
        {
            return View();
        }
        public IActionResult Track()
        {
            return View();
        }
        public IActionResult SubmitClaim() 
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
