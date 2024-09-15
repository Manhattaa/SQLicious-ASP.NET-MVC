using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;
using SQLicious_ASP.NET_MVC.Models;
using SQLicious_ASP.NET_MVC.Models.DTO;
using SQLicious_ASP.NET_MVC.Services.IServices;
using System.Security.Claims;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SQLicious_ASP.NET_MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: api/Admin/all
        [HttpGet("all")]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _adminService.GetAllAdmins();
            return Ok(admins);
        }

        // GET: api/Admin/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _adminService.GetAdminById(id);
            if (admin == null)
            {
                return NotFound();
            }
            return Ok(admin);
        }

        // POST: api/Admin/create
        [HttpPost("create")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAccountRequestDTO request)
        {
            // Validate the incoming model based on the Data Annotations in CreateAccountRequestDTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the service method to create the admin
            var result = await _adminService.CreateAdminAsync(request);

            // Check if the operation succeeded
            if (result.Success)
            {
                // Return the success message along with the generated JWT token
                return Ok(new { Message = "Admin created successfully", Token = result.Token });
            }

            // If there are errors, return them in the response
            return BadRequest(new { Message = result.Errors });
        }



        // PUT: api/Admin/edit
        [HttpPut("edit")]
        public async Task<IActionResult> EditAdmin([FromBody] Admin admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _adminService.UpdateAdminAsync(admin);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Admin updated successfully" });
            }

            return BadRequest(result.Errors);
        }

        // DELETE: api/Admin/delete/{id}
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id, [FromBody] DeleteAdminRequestModel model)
        {
            var result = await _adminService.DeleteAdminAsync(model.Password, User);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Admin deleted successfully" });
            }

            return BadRequest(result.Errors);
        }

        // POST: api/Admin/login
        [Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAdmin([FromBody] LoginRequestDTO model)
        {
            var result = await _adminService.LoginAsync(model.Email, model.Password);
            if (result.Success)
            {
                return Ok(new { Message = "Login successful" });
            }

            return Unauthorized(result.ErrorMessage);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequestDTO model)
        {
            var result = await _adminService.SendEmailVerificationAsync(model.UserId, model.Code);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Email confirmed successfully" });
            }

            return BadRequest(new { Message = "Email verification failed" });
        }

        [NonAction]
        public IActionResult Login()
        {
            return View();
        }
    }
}
