using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string LoginName { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}