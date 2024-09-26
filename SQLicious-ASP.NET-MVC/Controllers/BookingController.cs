using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQLicious_ASP.NET_MVC.Models.DTOs;
using System.Net.Http;
using Newtonsoft.Json;
using SQLicious_ASP.NET_MVC.Helpers;
using System.Text;

namespace SQLicious_ASP.NET_MVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public BookingController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1, int pageSize = 10)
        {
            var client = _clientFactory.CreateClient();

            // Fetch all bookings from the API
            HttpResponseMessage bookingResponse = await client.GetAsync("https://localhost:7213/api/Booking");
            if (!bookingResponse.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var bookingJsonResponse = await bookingResponse.Content.ReadAsStringAsync();
            var bookingList = JsonConvert.DeserializeObject<IEnumerable<BookingDTO>>(bookingJsonResponse);

            // Fetch all customers in parallel to avoid blocking
            var customerTasks = bookingList.Select(async booking =>
            {
                if (booking.CustomerId != 0)  // Ensure customer ID is valid
                {
                    HttpResponseMessage customerResponse = await client.GetAsync($"https://localhost:7213/api/Customer/{booking.CustomerId}");
                    if (customerResponse.IsSuccessStatusCode)
                    {
                        var customerJsonResponse = await customerResponse.Content.ReadAsStringAsync();
                        booking.Customer = JsonConvert.DeserializeObject<CustomerDTO>(customerJsonResponse);
                    }
                }
            });

            // Wait for all customer fetch tasks to complete
            await Task.WhenAll(customerTasks);

            // Proceed with sorting, filtering, and pagination logic
            var pagedBookings = PagedResult<BookingDTO>.Create(bookingList, page, pageSize);

            return View(pagedBookings);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7213/api/Booking/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var booking = JsonConvert.DeserializeObject<BookingDTO>(jsonResponse);
                return View(booking);
            }

            return View("Error");
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, BookingDTO booking)
        {
            var client = _clientFactory.CreateClient();

            // Convert booking data to JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(booking), Encoding.UTF8, "application/json");

            // Send the PUT request to the API
            HttpResponseMessage response = await client.PutAsync($"https://localhost:7213/api/Booking/update", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                // Return JSON response indicating success
                return Json(new { success = true });
            }

            // Return JSON response indicating failure
            return Json(new { success = false });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync($"https://localhost:7213/api/Booking/{id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var booking = JsonConvert.DeserializeObject<BookingDTO>(jsonResponse);
                return View(booking);
            }

            return View("Error");
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.DeleteAsync($"https://localhost:7213/api/Booking/delete?bookingId={id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Error");
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7213/api/Booking"); 
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to fetch bookings.");
            }

            var bookingJson = await response.Content.ReadAsStringAsync();
            var bookings = JsonConvert.DeserializeObject<IEnumerable<BookingDTO>>(bookingJson);
            return Json(bookings); 
        }
    }
}