namespace SQLicious_ASP.NET_MVC.Models
{
    public enum MenuType
    {
        Frukost,
        Brunch,
        Lunch,
        Middag,
        Julbord
    }
    public class MenuItem
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public bool IsAvailable { get; set; }
        public MenuType MenuType { get; set; }
    }
}
