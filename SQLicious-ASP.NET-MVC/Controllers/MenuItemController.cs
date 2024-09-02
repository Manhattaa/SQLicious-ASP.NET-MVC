using Microsoft.AspNetCore.Mvc;
using SQLicious_ASP.NET_MVC.Models.DTO;
using SQLicious_ASP.NET_MVC.Models;
using SQLicious_ASP.NET_MVC.Services.IServices;

namespace SQLicious_ASP.NET_MVC.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        // GET: MenuItem
        public async Task<IActionResult> Index()
        {
            var listOfMenuItems = await _menuItemService.GetAllMenuItemsAsync();
            return View(listOfMenuItems);
        }

        // GET: MenuItem/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }

        // GET: MenuItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuItem/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItemDTO menuItem)
        {
            if (ModelState.IsValid)
            {
                await _menuItemService.CreateMenuItemAsync(menuItem);
                return RedirectToAction(nameof(Index));
            }
            return View(menuItem);
        }

        // GET: MenuItem/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItem/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItems menuItem)
        {
            if (id != menuItem.MenuItemId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _menuItemService.UpdateMenuItemAsync(menuItem);
                return RedirectToAction(nameof(Index));
            }
            return View(menuItem);
        }

        // GET: MenuItem/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            return View(menuItem);
        }

        // POST: MenuItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _menuItemService.DeleteMenuItemAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}