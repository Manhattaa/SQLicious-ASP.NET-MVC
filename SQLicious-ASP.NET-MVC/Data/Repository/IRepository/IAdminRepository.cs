namespace SQLicious_ASP.NET_MVC.Data.Repository.IRepository
{
    public interface IAdminRepository
    {
        Task<IEnumerable<IAdminRepository>> GetAllAdmins();
    }
}
