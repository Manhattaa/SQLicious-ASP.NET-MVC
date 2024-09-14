using Microsoft.AspNetCore.Identity;
using SQLicious_ASP.NET_MVC.Models;
using SQLicious_ASP.NET_MVC.Options;
using System.Security.Claims;

namespace SQLicious_ASP.NET_MVC.Data.Repository.IRepository
{
    public interface IAdminRepository
    {
        Task<IEnumerable<Admin>> GetAllAdmins();
        Task<Admin> GetAdminById(int id);
        Task<IdentityResult> CreateAdminAsync(Admin admin, string password);
        Task<IdentityResult> UpdateAdminAsync(Admin admin);
        Task<IdentityResult> DeleteAdminAsync(string password, ClaimsPrincipal currentUser);
        Task<LoginResult> LoginAsync(string email, string password);
        Task<IdentityResult> SendEmailVerificationAsync(string userId, string code);
        Task<Admin> GetAdminByEmailAsync(string email);
    }
}
