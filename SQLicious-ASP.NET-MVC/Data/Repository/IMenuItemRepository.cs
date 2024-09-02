using Microsoft.EntityFrameworkCore;
using SQLicious_ASP.NET_MVC.Data.Repository.IRepository;
using SQLicious_ASP.NET_MVC.Models;

namespace SQLicious_ASP.NET_MVC.Data.Repository
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly RestaurantContext _context;

        public MenuItemRepository(RestaurantContext context)
        {
            _context = context;
        }
        public async Task CreateMenuItemAsync(MenuItems menuItem)
        {
            await _context.MenuItems.AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMenuItemAsync(int menuItemId)
        {
            var menuItem = await _context.MenuItems.FindAsync();

            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<MenuItems>> GetAllMenuItemsAsync()
        {
            var listOfAllMenuItems = await _context.MenuItems.ToListAsync();
            return listOfAllMenuItems;
        }

        public async Task<MenuItems> GetMenuItemByIdAsync(int menuItemId)
        {
            var menuItem = await _context.MenuItems.FindAsync(menuItemId);
            return menuItem;
        }

        public async Task<IEnumerable<MenuItems>> GetThisWeeksMenu()
        {
            var listOfAllMenuItems = await _context.MenuItems.ToListAsync();
            return listOfAllMenuItems;
        }

        public async Task UpdateMenuItemAsync(MenuItems menuItem)
        {
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
        }
    }
}
