using System;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class UserModel
    {
        [Required]
        public string LoginName { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string Nickname { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime Birthdate { get; set; }
        public bool SharedProfile { get; set; }
        public int Id { get; set; }

        public int? ImageAvatarId { get; set; }
    }
}