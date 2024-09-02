using SQLicious_ASP.NET_MVC.Data.Repository.IRepository;
using SQLicious_ASP.NET_MVC.Services.IServices;

namespace SQLicious_ASP.NET_MVC.Services
{
    public class AdminService : IAdminService
    {
        public Task<IEnumerable<IAdminRepository>> GetAllAdmins()
        {
            throw new NotImplementedException();
        }
    }
}
