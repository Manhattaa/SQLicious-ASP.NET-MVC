using Microsoft.AspNetCore.Identity;

namespace SQLicious_ASP.NET_MVC.Models
{
    public class Admin : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
