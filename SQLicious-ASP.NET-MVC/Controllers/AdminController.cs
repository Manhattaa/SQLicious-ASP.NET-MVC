using Microsoft.AspNetCore.Mvc;

namespace SQLicious_ASP.NET_MVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
