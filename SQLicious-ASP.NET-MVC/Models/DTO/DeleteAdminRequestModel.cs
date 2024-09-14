using System.ComponentModel.DataAnnotations;

namespace SQLicious_ASP.NET_MVC.Models.DTO
{
    public class DeleteAdminRequestModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(256, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
