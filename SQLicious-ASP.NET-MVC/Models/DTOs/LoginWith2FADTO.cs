using System.ComponentModel.DataAnnotations;

namespace SQLicious_ASP.NET_MVC.Models.DTOs
{
    public class LoginWith2FADTO
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
