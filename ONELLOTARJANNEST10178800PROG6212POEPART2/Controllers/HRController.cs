using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Data;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Models;

public class HRController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public HRController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // Get all lecturers for HR view
    [Authorize(Roles = "HR")]
    public async Task<IActionResult> Index()
    {
        var role = await _roleManager.FindByNameAsync("Lecturer");
        if (role == null)
        {
            return View("Error");
        }

        // Get users with 'Lecturer' role
        var lecturers = await _context.Users
            .Where(u => _context.UserRoles
                .Any(ur => ur.UserId == u.Id && ur.RoleId == role.Id))
            .ToListAsync();

        return View(lecturers);
    }

    // Edit Lecturer Details
    [Authorize(Roles = "HR")]
    public async Task<IActionResult> Edit(string id)
    {
        var lecturer = await _context.Users.FindAsync(id);
        if (lecturer == null)
        {
            return NotFound();
        }

        return View(lecturer);
    }

    
    [HttpPost]
    [Authorize(Roles = "HR")]
    public async Task<IActionResult> Edit(ApplicationUser lecturer)
    {
        if (ModelState.IsValid)
        {
            var existingLecturer = await _context.Users.FindAsync(lecturer.Id);
            if (existingLecturer != null)
            {
                existingLecturer.FirstName = lecturer.FirstName;
                existingLecturer.LastName = lecturer.LastName;
                existingLecturer.PhoneNumber = lecturer.PhoneNumber;
                existingLecturer.Email = lecturer.Email;

                _context.Update(existingLecturer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }
        return View(lecturer);
    }
}

