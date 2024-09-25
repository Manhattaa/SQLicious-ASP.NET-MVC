using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SQLicious_ASP.NET_MVC.Models.DTOs;
using System.Net.Http;
using Newtonsoft.Json;
using SQLicious_ASP.NET_MVC.Helpers;

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

            // Fetch all bookings
            HttpResponseMessage bookingResponse = await client.GetAsync("https://localhost:7213/api/Booking");
            if (!bookingResponse.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var bookingJsonResponse = await bookingResponse.Content.ReadAsStringAsync();
            var bookingList = JsonConvert.DeserializeObject<IEnumerable<BookingDTO>>(bookingJsonResponse);

            // Sorting logic
            ViewData["BookingIDSort"] = String.IsNullOrEmpty(sortOrder) ? "bookingID_desc" : "";
            ViewData["NameSort"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["EmailSort"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["DateSort"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["GuestsSort"] = sortOrder == "guests" ? "guests_desc" : "guests";
            ViewData["TableSort"] = sortOrder == "table" ? "table_desc" : "table";

            if (!String.IsNullOrEmpty(searchString))
            {
                bookingList = bookingList.Where(
                    b => b.Customer.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase)                        
                    || b.Customer.LastName.Contains(searchString, StringComparison.OrdinalIgnoreCase)                      
                    || b.Customer.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase)                   
                    || b.BookingId.ToString().Contains(searchString));
            }



            switch (sortOrder)
            {
                case "bookingID_desc":
                    bookingList = bookingList.OrderByDescending(b => b.BookingId);
                    break;
                case "name":
                    bookingList = bookingList.OrderBy(b => b.Customer.FirstName);
                    break;
                case "name_desc":
                    bookingList = bookingList.OrderByDescending(b => b.Customer.FirstName);
                    break;
                case "email":
                    bookingList = bookingList.OrderBy(b => b.Customer.Email);
                    break;
                case "email_desc":
                    bookingList = bookingList.OrderByDescending(b => b.Customer.Email);
                    break;
                case "date":
                    bookingList = bookingList.OrderBy(b => b.BookedDateTime);
                    break;
                case "date_desc":
                    bookingList = bookingList.OrderByDescending(b => b.BookedDateTime);
                    break;
                case "guests":
                    bookingList = bookingList.OrderBy(b => b.AmountOfCustomers);
                    break;
                case "guests_desc":
                    bookingList = bookingList.OrderByDescending(b => b.AmountOfCustomers);
                    break;
                case "table":
                    bookingList = bookingList.OrderBy(b => b.TableId);
                    break;
                case "table_desc":
                    bookingList = bookingList.OrderByDescending(b => b.TableId);
                    break;
                default:
                    bookingList = bookingList.OrderBy(b => b.BookingId);
                    break;
            }

            // Pagination logic
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


        [HttpPut]
        public async Task<IActionResult> Edit(BookingDTO booking)
        {
            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.PutAsJsonAsync("https://localhost:7213/api/Booking/update", booking);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View("Index");
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
    }
}