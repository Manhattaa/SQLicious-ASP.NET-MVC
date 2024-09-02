using SQLicious_ASP.NET_MVC.Data.Repository.IRepository;

namespace SQLicious_ASP.NET_MVC.Services.IServices
{
    public interface IAdminService
    {
        Task<IEnumerable<IAdminRepository>> GetAllAdmins();
    }
}
