using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using BLL;
using System.Web.Security;
using MVC.CustomAuth;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.LoginName, model.Password))
                {
                    var user = (CustomMembershipUser)Membership.GetUser(model.LoginName, false);
                    if (user != null)
                    {
                        UserSerializeModel userModel = new UserSerializeModel()
                        {
                            UserId = user.UserId,
                            Nickname = user.Nickname
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket
                            (
                            1, model.LoginName, DateTime.Now, DateTime.Now.AddMinutes(30), false, userData
                            );

                        string enTicket = FormsAuthentication.Encrypt(authTicket);
                        HttpCookie cookie = new HttpCookie("Cookie32", enTicket);
                        Response.Cookies.Add(cookie);
                    }

          
                       return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Username or Password invalid");
            return View(model);
        }

        [HttpGet]
        public ActionResult RegUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {


            return View(model);
        }

        [HttpPost]
        public ActionResult RegUser(RegUserModel model) 
        {
            var salt = Hash.CreateSalt(16);
            var passhash = Hash.GenerateSaltedHash(model.Password, salt);
           // Console.WriteLine("Start");
            var user = new BLL.DTO.UserDTO
            {
                BirthDate = model.BirthDate,
                IsProfileShared = model.SharedProfile,
                LoginName = model.LoginName,
                Nickname = model.Nickname,
                CreateTime = DateTime.Now,
                Salt = Convert.ToBase64String(salt),
                PasswordHash = Convert.ToBase64String(passhash)
            };

            try
            {
                DbManager.CreateOrUpdateUser(user);
            }
            catch (Exception ex)
            {
             //   Console.WriteLine("Error");
                ViewBag.Error = ex.Message;
                model.Password = null;
               // model.RetryPassword = null;
                return View(model);
            }

            return RedirectToAction("Login");
        }
    }
}