using System.ComponentModel.DataAnnotations;

namespace SQLicious_ASP.NET_MVC.Models.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
