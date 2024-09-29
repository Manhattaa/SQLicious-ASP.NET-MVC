using Microsoft.AspNetCore.Mvc;
using SQLicious_ASP.NET_MVC.Models.DTOs;
using System.Net.Http;
using System.Threading.Tasks;

namespace SQLicious.MVC.Controllers
{
    public class TwoFactorAuthController : Controller
    {
        private readonly HttpClient _httpClient;

        public TwoFactorAuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("APIClient");
        }

        // Helper to add JWT token to authorization header
        private void AddAuthorizationHeader()
        {
            var token = HttpContext.Request.Cookies["JWTToken"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Enable2FA()
        {
            // Get the JWT token (authentication)
            var token = HttpContext.Request.Cookies["JWTToken"];
            if (token == null)
            {
                return RedirectToAction("Login", "Admin"); // Redirect to login if no token
            }

            // Pass the token in the Authorization header to the API
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Request to generate the QR code for enabling 2FA from API
            var response = await _httpClient.GetAsync("https://localhost:7213/api/TwoFactorAuth/generate-qr-code");

            if (response.IsSuccessStatusCode)
            {
                // Return the Enable2FA view, passing the QR code URI as a model
                var qrCodeUri = await response.Content.ReadAsStringAsync();
                var model = new TwoFactorAuthDTO { QrCodeUrl = qrCodeUri };
                return View(model);  // This view will allow the user to scan and verify
            }

            // Handle API error
            TempData["ErrorMessage"] = "Failed to load QR code.";
            return RedirectToAction("AdminSettings", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Enable2FA(TwoFactorAuthDTO model)
        {
            // Simple logging to verify the form is being submitted
            Console.WriteLine("Form submitted with code: " + model.Code);

            // Get the JWT token
            var token = HttpContext.Request.Cookies["JWTToken"];
            if (token == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            // Set the Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Make a request to API to verify the entered 2FA code
            var response = await _httpClient.PostAsync($"https://localhost:7213/api/TwoFactorAuth/verify-2fa?code={model.Code}", null);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Two-Factor Authentication has been enabled!";
                return RedirectToAction("AdminSettings", "Admin");
            }

            TempData["ErrorMessage"] = "Invalid authentication code.";
            return View(model); // Re-render the Enable2FA page
        }


        // Generate QR code for 2FA setup
        [HttpGet]
        public async Task<IActionResult> GenerateQrCode()
        {
            AddAuthorizationHeader();

            var response = await _httpClient.GetAsync("https://localhost:7213/api/TwoFactorAuth/generate-qr-code");

            if (response.IsSuccessStatusCode)
            {
                var qrCode = await response.Content.ReadAsByteArrayAsync();
                return File(qrCode, "image/png");
            }

            TempData["ErrorMessage"] = "QR-koden kunde inte genereras.";
            return RedirectToAction("AdminSettings", "Admin");
        }

        // Verify the 2FA code
        [HttpPost]
        public async Task<IActionResult> Verify2FA(Verify2FADTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AddAuthorizationHeader();

            var response = await _httpClient.PostAsync($"https://localhost:7213/api/TwoFactorAuth/verify-2fa?code={model.Code}", null);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Inloggning lyckades!";
                return RedirectToAction("Dashboard", "Admin");
            }

            TempData["ErrorMessage"] = "Ogiltig autentiseringskod.";
            return View("Verify2FA", model); // Stay on the same form with the model.
        }

        // Disable 2FA
        [HttpPost]
        public async Task<IActionResult> Disable2FA()
        {
            AddAuthorizationHeader();

            var response = await _httpClient.PostAsync("https://localhost:7213/api/TwoFactorAuth/disable-2fa", null);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "2FA har inaktiverats.";
                return RedirectToAction("AdminSettings", "Admin");
            }

            TempData["ErrorMessage"] = "Inaktiveringen av 2FA misslyckades.";
            return RedirectToAction("AdminSettings", "Admin");
        }
    }
}
