using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Controllers;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Models;
using ONELLOTARJANNEST10178800PROG6212POEPART2.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[TestClass]
public class ClaimsControllerTests
{
    private ClaimsController _controller;
    private Mock<AddDbContext> _mockContext;

    [TestInitialize]
    public void Setup()
    {
        _mockContext = new Mock<AddDbContext>();
        _controller = new ClaimsController(_mockContext.Object);
    }

    [TestMethod]
    public async Task SubmitClaim_LecturerIdNegative_ReturnsWithError()
    {
        
        var claim = new Claim { LecturerId = -1, Rate = 100, Hours = 10 };
        var file = new Mock<IFormFile>();

      
        var result = await _controller.SubmitClaim(claim, file.Object) as ViewResult;

        Assert.IsNotNull(result);
        Assert.IsTrue(_controller.ModelState.ContainsKey("LecturerId"));
        Assert.AreEqual("Lecturer ID cannot be negative.", _controller.ModelState["LecturerId"].Errors[0].ErrorMessage);
    }

    [TestMethod]
    public async Task SubmitClaim_RateNegative_ReturnsWithError()
    {
       
        var claim = new Claim { LecturerId = 1, Rate = -5, Hours = 10 };
        var file = new Mock<IFormFile>();

      
        var result = await _controller.SubmitClaim(claim, file.Object) as ViewResult;

        
        Assert.IsNotNull(result);
        Assert.IsTrue(_controller.ModelState.ContainsKey("Rate"));
        Assert.AreEqual("Rate cannot be negative.", _controller.ModelState["Rate"].Errors[0].ErrorMessage);
    }

    [TestMethod]
    public async Task SubmitClaim_HoursNegative_ReturnsWithError()
    {
      
        var claim = new Claim { LecturerId = 1, Rate = 50, Hours = -5 };
        var file = new Mock<IFormFile>();

        
        var result = await _controller.SubmitClaim(claim, file.Object) as ViewResult;

       
        Assert.IsNotNull(result);
        Assert.IsTrue(_controller.ModelState.ContainsKey("Hours"));
        Assert.AreEqual("Hours worked cannot be negative.", _controller.ModelState["Hours"].Errors[0].ErrorMessage);
    }

    [TestMethod]
    public async Task SubmitClaim_FileNotUploaded_ReturnsWithError()
    {
       
        var claim = new Claim { LecturerId = 1, Rate = 50, Hours = 10 };
        IFormFile file = null;

      
        var result = await _controller.SubmitClaim(claim, file) as ViewResult;

       
        Assert.IsNotNull(result);
        Assert.IsTrue(_controller.ModelState.ContainsKey("UploadedFilePath"));
        Assert.AreEqual("File is required.", _controller.ModelState["UploadedFilePath"].Errors[0].ErrorMessage);
    }

    [TestMethod]
    public async Task SubmitClaim_InvalidFileType_ReturnsWithError()
    {
      
        var claim = new Claim { LecturerId = 1, Rate = 50, Hours = 10 };
        var file = new Mock<IFormFile>();
        file.Setup(f => f.FileName).Returns("test.exe");
        file.Setup(f => f.Length).Returns(1024);

       
        var result = await _controller.SubmitClaim(claim, file.Object) as ViewResult;

        
        Assert.IsNotNull(result);
        Assert.IsTrue(_controller.ModelState.ContainsKey("UploadedFilePath"));
        Assert.AreEqual("Only PDF, DOCX, and TXT files are allowed.", _controller.ModelState["UploadedFilePath"].Errors[0].ErrorMessage);
    }

    [TestMethod]
    public async Task SubmitClaim_ValidData_SubmitsSuccessfully()
    {
       
        var claim = new Claim { LecturerId = 1, Rate = 50, Hours = 10 };
        var file = new Mock<IFormFile>();
        file.Setup(f => f.FileName).Returns("test.pdf");
        file.Setup(f => f.Length).Returns(1024);

       
        var result = await _controller.SubmitClaim(claim, file.Object) as RedirectToActionResult;

      
        Assert.IsNotNull(result);
        Assert.AreEqual("ClaimSuccess", result.ActionName);
    }
}
