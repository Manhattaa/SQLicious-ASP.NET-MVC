using SQLicious_ASP.NET_MVC.Data.Repository.IRepository;
using SQLicious_ASP.NET_MVC.Models;

namespace SQLicious_ASP.NET_MVC.Data.Repository
{
    public class AdminRepository : IAdminRepository
    {
        public Task<Admin> CreateAdmin(Admin admin)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> DeleteAdmin(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> GetAdminById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IAdminRepository>> GetAllAdmins()
        {
            throw new NotImplementedException();
        }

        public Task<Admin> Login(Admin admin)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> Logout(Admin admin)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> UpdateAdmin(Admin admin)
        {
            throw new NotImplementedException();
        }
    }
}
