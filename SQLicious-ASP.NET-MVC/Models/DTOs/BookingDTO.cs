using Microsoft.Identity.Client;

namespace SQLicious_ASP.NET_MVC.Models.DTOs
{
    public class BookingDTO
    {
        public int BookingId { get; set; }
        public int AmountOfCustomers {  get; set; }
        public int CustomerId { get; set; }
        public int TableId { get; set; }
        public DateTime BookedDateTime { get; set; }

        public CustomerDTO Customer { get; set; }
    }
}
