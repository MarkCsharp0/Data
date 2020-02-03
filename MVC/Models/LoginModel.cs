using System.ComponentModel.DataAnnotations;
namespace MVC.Models
{
    public class LoginModel
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }

    }
}