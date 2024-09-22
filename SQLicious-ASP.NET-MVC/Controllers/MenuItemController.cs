using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQLicious_ASP.NET_MVC.Models.DTOs;
using System.Text;

namespace SQLicious_ASP.NET_MVC.Controllers
{
        [ApiController]
        [Route("menuitem")]
        public class MenuItemController : Controller
        {
            private readonly IHttpClientFactory _clientFactory;

            public MenuItemController(IHttpClientFactory clientFactory)
            {
                _clientFactory = clientFactory;
            }

            public IActionResult Menu()
            {
                return View();
            }
        [HttpGet("menusettings")]
        public async Task<IActionResult> MenuSettings()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7213/api/MenuItem");

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response into a list of MenuItemDTO objects
                var menuItems = JsonConvert.DeserializeObject<IEnumerable<MenuItemDTO>>(await response.Content.ReadAsStringAsync());

                // Pass the list of menu items to the view
                return View(menuItems);
            }

            // Handle case when API call fails
            return View(new List<MenuItemDTO>()); // Return an empty list to prevent null references in the view
        }

        // Publicly accessible, no authentication required
        [HttpGet("index")]
            public async Task<IActionResult> Index()
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync("https://localhost:7213/api/MenuItem");

                if (response.IsSuccessStatusCode)
                {
                    var menuItems = JsonConvert.DeserializeObject<IEnumerable<MenuItemDTO>>(await response.Content.ReadAsStringAsync());
                    return View(menuItems); 
                }

                return View("Error"); // Handle error cases
            }

            // Only accessible by users with the "Admin" role
            [Authorize(Roles = "Admin")]
            [HttpGet("create")]
            public IActionResult Create()
            {
                return View();
            }

            //[Authorize(Roles = "Admin")]
            [HttpPost("create")]
            public async Task<IActionResult> Create(MenuItemCreationDTO menuItemDto)
            {
                var client = _clientFactory.CreateClient();
                var jsonContent = new StringContent(JsonConvert.SerializeObject(menuItemDto), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:7213/api/MenuItem/create", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(menuItemDto);
            }

            //[Authorize(Roles = "Admin")]
            [HttpGet("edit/{id}")]
            public async Task<IActionResult> Edit(int id)
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync($"https://localhost:7213/api/MenuItem/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var menuItem = JsonConvert.DeserializeObject<MenuItemDTO>(await response.Content.ReadAsStringAsync());
                    return View(menuItem);
                }

                return View("Error");
            }

            //[Authorize(Roles = "Admin")]
            [HttpPost("edit/{id}")]
            public async Task<IActionResult> Edit(int id, MenuItemDTO menuItemDto)
            {
                var client = _clientFactory.CreateClient();
                var jsonContent = new StringContent(JsonConvert.SerializeObject(menuItemDto), Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"https://localhost:7213/api/MenuItem/{id}", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View(menuItemDto);
            }

            //[Authorize(Roles = "Admin")]
            [HttpPost("delete/{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var client = _clientFactory.CreateClient();
                var response = await client.DeleteAsync($"https://localhost:7213/api/MenuItem/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }

                return View("Error");
            }
        }
}