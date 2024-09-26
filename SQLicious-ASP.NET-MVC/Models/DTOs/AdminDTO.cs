namespace SQLicious_ASP.NET_MVC.Models.DTOs
{
    public class AdminDTO
    {
        public string AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
    }
}
