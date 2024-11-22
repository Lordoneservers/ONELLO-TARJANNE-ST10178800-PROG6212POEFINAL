using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Data;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Models;
using System.Threading.Tasks;

namespace ONELLOTARJANNEST10178800PROG6212POEPART2.Controllers
{
    public class ClaimsController : Controller
    {
        private readonly AddDbContext _context;

        public ClaimsController(AddDbContext context)
        {
            _context = context;
        }
        //Submit claim method
        [HttpPost]
        public async Task<IActionResult> SubmitClaim(Claim claim, IFormFile uploadedFile)
        {
            // Validation of  numeric values to ensure they are not negative
            if (claim.LecturerId < 0)
            {
                ModelState.AddModelError("LecturerId", "Lecturer ID cannot be negative.");
            }

            if (claim.Rate < 0)
            {
                ModelState.AddModelError("Rate", "Rate cannot be negative.");
            }

            if (claim.Hours < 0)
            {
                ModelState.AddModelError("Hours", "Hours worked cannot be negative.");
            }

           
            if (!ModelState.IsValid)
            {
                return View("post", claim);
            }

            
            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                ModelState.AddModelError("UploadedFilePath", "File is required.");
                return View("post", claim); 
            }

          
            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            var filePath = Path.Combine(uploadsDirectory, Path.GetFileName(uploadedFile.FileName));

            
            if (System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError("UploadedFilePath", "A file with this name already exists. Please rename your file.");
                return View("post", claim); 
            }
            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                ModelState.AddModelError("UploadedFilePath", "File is required.");
                return View("post", claim);
            }

            // Validate file type (PDF, DOCX, TXT only)
            string[] allowedExtensions = new[] { ".pdf", ".docx", ".txt" };
            string fileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("UploadedFilePath", "Only PDF, DOCX, and TXT files are allowed.");
                return View("post", claim);
            }


            if (uploadedFile.Length > 30 * 1024 * 1024) // 30MB limit for files
            {
                ModelState.AddModelError("UploadedFilePath", "File size must not exceed 30MB.");
                return View("post", claim);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

           
            claim.UploadedFilePath = filePath;

            // Save the claim to the database
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Redirect to the success page
            return RedirectToAction("ClaimSuccess", "Home");
        }
       

        [HttpPost]
        public IActionResult DownloadApprovedClaimsPdf()
        {
            var approvedClaims = _context.Claims.Where(c => c.ClaimStatus == "Approved").ToList();
            var totalAmount = approvedClaims.Sum(c => c.ClaimAmount);

            using var document = new PdfSharp.Pdf.PdfDocument();
            var page = document.AddPage();
            var graphics = PdfSharp.Drawing.XGraphics.FromPdfPage(page);
            var font = new PdfSharp.Drawing.XFont("Arial", 12);

            int y = 40; // Starting y-coordinate
            graphics.DrawString("Approved Claims Report", font, PdfSharp.Drawing.XBrushes.Black, 40, y);

            y += 20;
            graphics.DrawString("Claim ID | Lecturer ID | Name | Hours | Rate | Claim Amount", font, PdfSharp.Drawing.XBrushes.Black, 40, y);

            foreach (var claim in approvedClaims)
            {
                y += 20;
                graphics.DrawString(
                    $"{claim.ClaimId} | {claim.LecturerId} | {claim.Name} | {claim.Hours} | {claim.Rate} | {claim.ClaimAmount}",
                    font, PdfSharp.Drawing.XBrushes.Black, 40, y);
            }

            y += 40;
            graphics.DrawString($"Total Amount: {totalAmount}", font, PdfSharp.Drawing.XBrushes.Black, 40, y);

            var stream = new MemoryStream();
            document.Save(stream, false);
            stream.Position = 0;
            return File(stream, "application/pdf", "ApprovedClaimsReport.pdf");
        }
        [HttpPost]
        public IActionResult ShowClaimByLecturer(int lecturerId)
        {
            // Use raw SQL to fetch claims
            var claims = _context.Claims
                .FromSqlInterpolated($"SELECT * FROM Claims WHERE LecturerId = {lecturerId}")
                .ToList();

            if (claims == null || !claims.Any())
            {
                ViewData["Title"] = "No Claims Found";
                return View(new List<Claim>());
            }

            ViewData["Title"] = "Claims for Lecturer";
            return View("ShowClaimByLecturer", claims);
        }



        ///Download file method
        [HttpGet]
        public IActionResult DownloadFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return NotFound(); 
            }

            var fileName = Path.GetFileName(filePath);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            if (!System.IO.File.Exists(fullPath))
            {
                return NotFound(); 
            }

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
        //Approve the claim method
        [HttpPost]
        public async Task<IActionResult> ApproveClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                claim.ClaimStatus = "Approved";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Track");
        }
        //Reject claim method
        [HttpPost]
        public async Task<IActionResult> RejectClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                claim.ClaimStatus = "Rejected";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Track");
        }
        public IActionResult ClaimView()
        {
            var claims = _context.Claims.ToList(); 
            return View("ClaimView", claims); 
        }

        //Method to regard all claims as pending until the stautus is changed by admin
        public IActionResult Track()
        {
            var pendingClaims = _context.Claims.Where(c => c.ClaimStatus == "Pending").ToList();
            return View("Track", pendingClaims);
        }
    }
}
