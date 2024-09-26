using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;
using SQLicious_ASP.NET_MVC.Models.DTOs;

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
                string Message = jsonObject["message"].ToString();
                if (token != null)
                {
                    // Store the JWT token in a cookie
                    Response.Cookies.Append("JWTToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });
                    if (Message == "The Admin account needs to have Two Factor Authentication.")
                    {
                        ViewBag.Message = "2FA works!!!";
                    }
                    else
                    {
                        ViewBag.Message = "Login Successful!";
                    }
                    return RedirectToAction("Dashboard", "Admin");
                }
                else return Ok();
            }
            else { return RedirectToAction("LoginWith2FA", "Admin"); }

            
        }
        [HttpGet]
        public async Task<IActionResult> LoginWith2FA()
        {
            var model = new LoginWith2FADTO();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("JWTToken");
            ViewBag.Message = "Logged out successfully.";
            return RedirectToAction("SQLicious", "Home");
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var client = _clientFactory.CreateClient();

            var passwordData = new { currentPassword, newPassword, confirmPassword };
            var content = new StringContent(JsonConvert.SerializeObject(passwordData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7213/api/Admin/ChangePassword", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Password changed successfully!";
            }
            else
            {
                ViewBag.Message = "Password change failed!";
            }

            return View("AdminSettings");
        }
        //[HttpGet]
        //public async Task<IActionResult> AllAdmins()
        //{
        //    var client = _clientFactory.CreateClient();
        //    var response = await client.GetAsync("https://localhost:7213/api/Admin");

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        ViewBag.Message = "Failed to fetch admin accounts.";
        //        return View("AdminSettings");
        //    }

        //    var admins = JsonConvert.DeserializeObject<IEnumerable<AdminDTO>>(await response.Content.ReadAsStringAsync());
        //    return View("AdminSettings", admins);
        //}

    }
}
