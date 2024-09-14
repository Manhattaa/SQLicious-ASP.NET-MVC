using System.ComponentModel.DataAnnotations;

namespace SQLicious_ASP.NET_MVC.Models.DTO
{
    public class CreateAccountRequestDTO
    {
        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Email address confirmation is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string EmailConfirmed { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(256, MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation is required.")]
        [DataType(DataType.Password)]
        [StringLength(256, MinimumLength = 8)]
        public string PasswordConfirmed { get; set; }
    }
}
