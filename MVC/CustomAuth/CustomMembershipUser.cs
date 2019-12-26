using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using BLL.DTO;
namespace MVC.CustomAuth
{
    public class CustomMembershipUser : MembershipUser
    {
        public int UserId { get; set; }

        public string LoginName { get; set; }
        public string Nickname { get; set; }
        public CustomMembershipUser(UserDTO user) 
        {
            UserId = user.Id;
            Nickname = user.Nickname;
        }
    }
}