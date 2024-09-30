using Microsoft.AspNetCore.Mvc;
using SQLicious_ASP.NET_MVC.Models.DTOs;
using System.Net.Http;
using System.Threading.Tasks;

namespace SQLicious.MVC.Controllers
{
    public class TwoFactorAuthController : Controller
    {
        private readonly HttpClient _httpClient;

        // Inject the named HttpClient
        public TwoFactorAuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("APIClient");
        }

        // Show the Enable 2FA page
        [HttpGet]
        public IActionResult Enable2FA()
        {
            return View();
        }

        // Generate QR code for 2FA setup
        [HttpGet]
        public async Task<IActionResult> GenerateQrCode()
        {
            var token = HttpContext.Request.Cookies["JWTToken"];
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                return Unauthorized("JWT Token not found. Please log in again.");
            }

            var response = await _httpClient.GetAsync("https://localhost:7213/api/TwoFactorAuth/generate-qr-code");
            if (response.IsSuccessStatusCode)
            {
                var qrCode = await response.Content.ReadAsByteArrayAsync();
                return File(qrCode, "image/png");
            }

            return BadRequest("Could not generate QR code");
        }

        // POST method to enable 2FA and verify the code
        [HttpPost("enable-2fa")]
        public async Task<IActionResult> POSTEnable2FA(Verify2FADTO model)
        {
            var token = HttpContext.Request.Cookies["JWTToken"];
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                return Unauthorized("JWT Token not found. Please log in again!");
            }

            var content = new StringContent($"{{\"code\":\"{model.Code}\"}}", System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7213/api/TwoFactorAuth/enable-2fa", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Congratulations! Two Factor Authentication is now enabled; You are safe... For now.";
                return RedirectToAction("AdminSettings", "Admin");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to enable Two Factor Authentication.";
                return RedirectToAction("Enable2FA", "TwoFactorAuth");
            }
        }

        // Verify the 2FA setup
        [HttpPost]
        public async Task<IActionResult> Verify2FA(Verify2FADTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = HttpContext.Request.Cookies["JWTToken"];
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                return Unauthorized("JWT Token not found. Please log in again.");
            }

            var content = new StringContent($"{{\"code\":\"{model.Code}\"}}", System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"https://localhost:7213/api/TwoFactorAuth/verify-2fa", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Success"); // Redirect to a success page after verification
            }

            ModelState.AddModelError(string.Empty, "Invalid code.");
            return View(model);
        }

        // Disable 2FA
        [HttpPost]
        public async Task<IActionResult> Disable2FA()
        {
            var token = HttpContext.Request.Cookies["JWTToken"];
            if (token != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                return Unauthorized("JWT Token not found. Please log in again.");
            }

            var response = await _httpClient.PostAsync("https://localhost:7213/api/TwoFactorAuth/disable-2fa", null);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Success");
            }

            ModelState.AddModelError(string.Empty, "Failed to disable 2FA.");
            return View();
        }
    }
}
