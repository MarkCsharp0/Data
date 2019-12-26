using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class ChangePasswordModel
    {
        public string LoginName { get; set; }
        public string OldPassword { get; set; }
        
        public string NewPassword { get; set; }
    }
}