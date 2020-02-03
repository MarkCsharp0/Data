using System;
using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class RegUserModel
    {

        [Display(Name = "Имя пользователя")]
        [Required(ErrorMessage = "Поле не заполнено")]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string Nickname { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime BirthDate { get; set; }
        //    [Required]
        [Display(Name = "Настройки приватности")]
        public bool SharedProfile { get; set; }
        // public int Id { get; set; }


    }
}