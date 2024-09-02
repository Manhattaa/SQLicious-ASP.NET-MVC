using SQLicious_ASP.NET_MVC.Data.Repository.IRepository;

namespace SQLicious_ASP.NET_MVC.Data.Repository
{
    public class AdminRepository : IAdminRepository
    {
        public Task<IEnumerable<IAdminRepository>> GetAllAdmins()
        {
            throw new NotImplementedException();
        }
    }
}
