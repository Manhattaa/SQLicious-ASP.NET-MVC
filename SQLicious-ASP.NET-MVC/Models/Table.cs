using System.ComponentModel.DataAnnotations;

namespace SQLicious_ASP.NET_MVC.Models
{
    public class Table
    {
        [Key]
        public int TableId { get; set; }
        public int Capacity { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
