using System.ComponentModel.DataAnnotations;

namespace SQLicious_ASP.NET_MVC.Models
{
    public class MenuItems
    {
        [Key]
        public int MenuItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
