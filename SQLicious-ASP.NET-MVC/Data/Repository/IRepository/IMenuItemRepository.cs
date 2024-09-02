using SQLicious_ASP.NET_MVC.Models;

namespace SQLicious_ASP.NET_MVC.Data.Repository.IRepository
{
    public interface IMenuItemRepository
    {
        Task<IEnumerable<MenuItems>> GetAllMenuItemsAsync();
        Task<IEnumerable<MenuItems>> GetThisWeeksMenu();
        Task<MenuItems> GetMenuItemByIdAsync(int menuItemId);
        Task CreateMenuItemAsync(MenuItems menuItem);
        Task UpdateMenuItemAsync(MenuItems menuItem);
        Task DeleteMenuItemAsync(int menuItemId);
    }
}