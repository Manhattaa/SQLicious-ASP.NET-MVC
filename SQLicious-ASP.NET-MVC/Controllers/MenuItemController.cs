using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQLicious_ASP.NET_MVC.Models.DTOs;
using System.Text;

namespace SQLicious_ASP.NET_MVC.Controllers
{
    public class MenuItemController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;


        public MenuItemController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> Menu()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7213/api/MenuItem/mostrecentpdfs");

            if (response.IsSuccessStatusCode)
            {
                var mostRecentPdfs = JsonConvert.DeserializeObject<IEnumerable<MenuPDFDTO>>(await response.Content.ReadAsStringAsync());
                return View(mostRecentPdfs);  
            }

            return View(new List<MenuPDFDTO>()); 
        }

        [HttpGet("menusettings")]
        public async Task<IActionResult> MenuSettings()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7213/api/MenuItem");

            if (response.IsSuccessStatusCode)
            {
                var menuItems = JsonConvert.DeserializeObject<IEnumerable<MenuItemDTO>>(await response.Content.ReadAsStringAsync());

                return View(menuItems);
            }

            return View(new List<MenuItemDTO>());
        }

        // Publicly accessible, no authentication required
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] MenuItemCreationDTO menuItemDto)
        {
            var client = _clientFactory.CreateClient();

            var jsonContent = new StringContent(JsonConvert.SerializeObject(menuItemDto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7213/api/MenuItem/create", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MenuSettings");
            }

            return View(menuItemDto);
        }




        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7213/api/MenuItem/update{id}");

            if (response.IsSuccessStatusCode)
            {
                var menuItem = JsonConvert.DeserializeObject<MenuItemDTO>(await response.Content.ReadAsStringAsync());
                return View(menuItem);
            }

            return View("Error");
        }

        [HttpPut("edit/{id}")]
        public async Task<IActionResult> Edit([FromForm] int id, MenuItemDTO menuItemDto)
        {

            var client = _clientFactory.CreateClient();
            var jsonContent = new StringContent(JsonConvert.SerializeObject(menuItemDto), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"https://localhost:7213/api/MenuItem/update/{id}", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MenuSettings");
            }

            return View(menuItemDto);
        }



        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:7213/api/MenuItem/delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("MenuSettings");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GenerateMenuPdf(string menuType)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7213/api/MenuItem/generatepdf/{menuType}");

            if (response.IsSuccessStatusCode)
            {
                var pdf = await response.Content.ReadAsByteArrayAsync();
                return File(pdf, "application/pdf", $"{menuType}_Menu.pdf");
            }

            return View("Error");
        }

        [HttpPost("generatepdf/{menuType}")]
        public async Task<IActionResult> GenerateAndSavePdf(string menuType)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:7213/api/MenuItem/generatepdf/{menuType}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var pdfBytes = await response.Content.ReadAsByteArrayAsync();

            var pdfDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdfs");
            var pdfFileName = $"{menuType}_Menu.pdf";  // Overwrite with a consistent name
            var filePath = Path.Combine(pdfDirectory, pdfFileName);

            
            if (!Directory.Exists(pdfDirectory))
            {
                Directory.CreateDirectory(pdfDirectory);
            }

            
            await System.IO.File.WriteAllBytesAsync(filePath, pdfBytes);

            
            var pdfUrl = $"{Request.Scheme}://{Request.Host}/pdfs/{pdfFileName}";
            return Ok(new { pdfUrl });
        }
    }
}