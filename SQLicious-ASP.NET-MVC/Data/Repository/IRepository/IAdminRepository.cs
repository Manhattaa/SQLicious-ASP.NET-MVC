using SQLicious_ASP.NET_MVC.Models;

namespace SQLicious_ASP.NET_MVC.Data.Repository.IRepository
{
    public interface IAdminRepository
    {
        Task<IEnumerable<IAdminRepository>> GetAllAdmins();
        Task<Admin> GetAdminById(int id);
        Task<Admin> CreateAdmin(Admin admin);
        Task<Admin> UpdateAdmin(Admin admin);
        Task<Admin> DeleteAdmin(int id);
        Task<Admin> Login(Admin admin);
        Task<Admin> Logout(Admin admin);
    }
}
