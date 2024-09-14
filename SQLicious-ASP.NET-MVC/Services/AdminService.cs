using Microsoft.AspNetCore.Identity;
using SQLicious_ASP.NET_MVC.Data.Repository.IRepository;
using SQLicious_ASP.NET_MVC.Models;
using SQLicious_ASP.NET_MVC.Models.DTO;
using SQLicious_ASP.NET_MVC.Options;
using SQLicious_ASP.NET_MVC.Services.IServices;
using System.Security.Claims;

namespace SQLicious_ASP.NET_MVC.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IEnumerable<Admin>> GetAllAdmins()
        {
            throw new NotImplementedException();
        }

        public async Task<Admin> GetAdminById(int id)
        {
            return await _adminRepository.GetAdminById(id);
        }

        // Update CreateAdminAsync to not require IAdminRepository as a parameter
        public async Task<IdentityResult> CreateAdminAsync(CreateAccountRequestDTO request)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.EmailConfirmed) ||
                string.IsNullOrEmpty(request.Password) ||
                string.IsNullOrEmpty(request.PasswordConfirmed))
            {
                return IdentityResult.Failed(new IdentityError { Description = "All fields are required." });
            }

            // Ensure Email and EmailConfirmed match
            if (request.Email != request.EmailConfirmed)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Emails do not match." });
            }

            // Ensure Password and PasswordConfirmed match
            if (request.Password != request.PasswordConfirmed)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Passwords do not match." });
            }

            // Check if email already exists
            var existingAdmin = await _adminRepository.GetAdminByEmailAsync(request.Email);
            if (existingAdmin != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email is already taken." });
            }

            // Create new admin
            var admin = new Admin
            {
                Email = request.Email,
                // Set other properties as needed
            };

            // Save the new admin to the database
            var result = await _adminRepository.CreateAdminAsync(admin, request.Password);

            return result;
        }

        public async Task<IdentityResult> UpdateAdminAsync(Admin admin)
        {
            return await _adminRepository.UpdateAdminAsync(admin);
        }

        public async Task<IdentityResult> DeleteAdminAsync(string password, ClaimsPrincipal currentUser)
        {
            return await _adminRepository.DeleteAdminAsync(password, currentUser);
        }

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            return await _adminRepository.LoginAsync(email, password);
        }

        public async Task<IdentityResult> SendEmailVerificationAsync(string userId, string code)
        {
            return await _adminRepository.SendEmailVerificationAsync(userId, code);
        }
    }
}
