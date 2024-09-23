using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;

namespace SQLicious_ASP.NET_MVC.Controllers
{
    [ApiController]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        // injecting httpclientfactory instead of httpclient
        public AdminController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }
        [HttpGet("adminsettings")]
        public IActionResult AdminSettings()
        {
            return View();
        }

        //[HttpGet("login")]
        //public IActionResult Login(string email, string password)
        //{
        //    return RedirectToAction("Dashboard", "Admin");
        //}

        [HttpGet("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var loginData = new Dictionary<string, string>
            {
                { "email", email },
                { "password", password }
            };

            // No need to serialize to JSON; send as form data
            var client = _clientFactory.CreateClient();
            var content = new FormUrlEncodedContent(loginData);

            // Send form-encoded data
            var response = await client.PostAsync("https://localhost:7213/api/Admin/Login", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var jsonObject = JObject.Parse(result);
                string token = jsonObject["token"].ToString();
                //var token = JsonConvert.DeserializeObject<dynamic>(result)?.Token;

                if (token != null)
                {
                    // Store the JWT token in a cookie
                    Response.Cookies.Append("JWTToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });

                    ViewBag.Message = "Login Successful!";
                    return RedirectToAction("Dashboard", "Admin");
                }
            }

            ViewBag.Message = "Login failed! Please try again.";
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("JWTToken");
            ViewBag.Message = "Logged out successfully.";
            return RedirectToAction("SQLicious", "Home");
        }
    }
}
