using SQLicious_ASP.NET_MVC.Models;
using SQLicious_ASP.NET_MVC.Models.DTO;

namespace SQLicious_ASP.NET_MVC.Services.IServices
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItems>> GetAllMenuItemsAsync();
        Task<IEnumerable<MenuItems>> GetThisWeeksMenu();
        Task<MenuItems> GetMenuItemByIdAsync(int menuItemId);
        Task CreateMenuItemAsync(MenuItemDTO menuItem);
        Task UpdateMenuItemAsync(MenuItems menuItem);
        Task DeleteMenuItemAsync(int menuItemId);
    }
}
