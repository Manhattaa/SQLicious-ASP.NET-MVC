namespace SQLicious_ASP.NET_MVC.Models.DTOs
{

    public class MenuItemDTO
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 
        public float Price { get; set; } 
        public bool IsAvailable { get; set; } 
        public MenuType MenuType { get; set; } 
    }
}
