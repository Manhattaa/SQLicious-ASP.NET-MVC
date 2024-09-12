using Microsoft.AspNetCore.Mvc;
using SQLicious_ASP.NET_MVC.Models;
using System.Diagnostics;

namespace SQLicious_ASP.NET_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        // minsida.se/Home/security/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SQLicious()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}