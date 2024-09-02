using SQLicious_ASP.NET_MVC.Data.Repository.IRepository;
using SQLicious_ASP.NET_MVC.Models.DTO;
using SQLicious_ASP.NET_MVC.Models;
using SQLicious_ASP.NET_MVC.Services.IServices;

namespace SQLicious_ASP.NET_MVC.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }
        public async Task CreateMenuItemAsync(MenuItemDTO menuItem)
        {
            try
            {
                var newMenuItem = new MenuItems
                {
                    Name = menuItem.Name,
                    Price = menuItem.Price,
                    IsAvailable = menuItem.IsAvailable
                };

                await _menuItemRepository.CreateMenuItemAsync(newMenuItem);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while trying to create the Menu Item: {ex.Message}");
            }
        }

        public async Task DeleteMenuItemAsync(int menuItemId)
        {
            try
            {
                await _menuItemRepository.DeleteMenuItemAsync(menuItemId);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while trying to delete the Menu Item: {ex.Message}");
            }
        }

        public async Task<IEnumerable<MenuItems>> GetAllMenuItemsAsync()
        {
            try
            {
                var listOfMenuItems = await _menuItemRepository.GetAllMenuItemsAsync();

                return listOfMenuItems.Select(i => new MenuItems
                {
                    MenuItemId = i.MenuItemId,
                    Name = i.Name,
                    Price = i.Price,
                    IsAvailable = i.IsAvailable
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task<MenuItems> GetMenuItemByIdAsync(int menuItemId)
        {
            try
            {
                var menuItem = await _menuItemRepository.GetMenuItemByIdAsync(menuItemId);

                return menuItem;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        //Future Implementation
        public async Task<IEnumerable<MenuItems>> GetThisWeeksMenu()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateMenuItemAsync(MenuItems menuItem)
        {
            try
            {
                if (menuItem != null)
                {
                    var updatedMenuItem = new MenuItems
                    {
                        MenuItemId = menuItem.MenuItemId,
                        Name = menuItem.Name,
                        Price = menuItem.Price,
                        IsAvailable = menuItem.IsAvailable
                    };

                    await _menuItemRepository.UpdateMenuItemAsync(updatedMenuItem);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while trying to update the Menu Item: {ex.Message}");
            }

        }
    }
}