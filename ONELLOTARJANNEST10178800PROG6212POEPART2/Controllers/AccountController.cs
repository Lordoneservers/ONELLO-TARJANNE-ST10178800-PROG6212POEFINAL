using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Controllers;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Models;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // Lecturer Registration Form
    [HttpGet]
    public IActionResult RegisterLecturer()
    {
        return View("Register", new RegisterViewModel { Role = "Lecturer" });
    }

    // Manager Registration Form
    [HttpGet]
    public IActionResult RegisterManager()
    {
        return View("Register", new RegisterViewModel { Role = "Manager" });
    }

    // HR Registration Form
    [HttpGet]
    public IActionResult RegisterHR()
    {
        return View("Register", new RegisterViewModel { Role = "HR" });
    }

    // POST: Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if the email is already registered
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                // Add an error to ModelState if the user already exists
                ModelState.AddModelError(string.Empty, "This email is already registered.");
                return View(model); // Return the view with the error
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber // Set the phone number here
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign role based on form
                var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to assign the role.");
                    return View(model);
                }

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    // GET: Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Dashboard", "Home"); // Redirect to home page after logout
    }

    // POST: Login
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
        }
        return View(model);
    }
}

