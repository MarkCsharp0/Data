using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        public DateTime BirthDate { get; set; }
        //    [Required]
        [Display(Name = "Настройки приватности")]
        public bool SharedProfile { get; set; }

    }
}