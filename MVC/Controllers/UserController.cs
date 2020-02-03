using BLL;
using MVC.CustomAuth;
using MVC.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Testik()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            HttpCookie cookie = new HttpCookie("Cookie25", "");
            cookie.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie);
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", null);
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
                    if (model.LoginName.Equals("") || model.LoginName == null)
                    {
                        return RedirectToAction("reguser", "user");
                    }
                    var user = (CustomMembershipUser)Membership.GetUser(model.LoginName, false);
                    //  var user  = DbManager.GetUser(Login: model.LoginName);
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
                        HttpCookie cookie = new HttpCookie("Cookie25", enTicket);
                        Response.Cookies.Add(cookie);
                        //   return RedirectToAction("reguser", "user");
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
        [Authorize]
        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordModel();
            model.LoginName = User.Identity.Name;
            // var t = DbManager.GetUser(Login: model.LoginName);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something wrong");
                return View(model);
            }
            // var user = (CustomMembershipUser)Membership.GetUser(model.LoginName, false);
            //user.ChangePassword()
            var result = DbManager.ChangePassword(model.LoginName, model.OldPassword, model.NewPassword);
            if (result)
            {
                ModelState.AddModelError("", "Something wrong");
                return RedirectToAction("ViewProfile");
            }
            else
            {
                return View(model);
            }
        }

        [Authorize]
        public ActionResult ViewProfile()
        {
            // var model = AutoMapper.Mapper.Map<Models.UserModel>(Data.BLL.Db.GetUser(User.Identity.Name));
            var t = BLL.DbManager.GetUser(Login: User.Identity.Name);
            UserModel model = new UserModel();
            model.LoginName = t.LoginName;
            model.Birthdate = t.BirthDate;
            model.Nickname = t.Nickname;
            model.ImageAvatarId = t.ImageAvatarId;
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditProfile()
        {
            //var model = AutoMapper.Mapper.Map<Models.UserModel>(Data.BLL.Db.GetUser(User.Identity.Name));
            var t = BLL.DbManager.GetUser(Login: User.Identity.Name);
            UserModel model = new UserModel();
            model.Id = t.Id;
            model.LoginName = t.LoginName;
            model.Birthdate = t.BirthDate;
            model.Nickname = t.Nickname;
            model.SharedProfile = t.IsProfileShared;
            model.ImageAvatarId = t.ImageAvatarId;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(UserModel model)
        {
            BLL.DTO.UserDTO UserDTO = new BLL.DTO.UserDTO();
            var user = DbManager.GetUser(Login: User.Identity.Name);
            UserDTO.Id = model.Id;
            UserDTO.LoginName = model.LoginName;
            UserDTO.IsProfileShared = model.SharedProfile;
            UserDTO.BirthDate = model.Birthdate;
            UserDTO.CreateTime = user.CreateTime;
            UserDTO.Nickname = model.Nickname;
            UserDTO.PasswordHash = user.PasswordHash;
            UserDTO.Salt = user.Salt;
            UserDTO.ImageAvatarId = model.ImageAvatarId;
            try
            {
                DbManager.CreateOrUpdateUser(UserDTO);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Something wrong");
                return View(model);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegUser(RegUserModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Username or Password invalid");
                return View(model);
            }

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
                ViewBag.Error = ex.Message;
                model.Password = null;

                return View(model);
            }
            return RedirectToAction("Login");
        }

        private static string _blobStoragePath = ConfigurationManager.AppSettings["BlobStoragePath"];

        /*  [HttpPost] public JsonResult UploadPostImage(HttpPostedFileBase upload)
          {



          }*/


        [HttpGet]

        public ActionResult TestFilter()
        {
            return View();


        }

        [HttpGet]

        public ActionResult TestGeoLocation()
        {
            return View();


        }

        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase upload)
        {
            var blobId = Guid.NewGuid();
            var filename = blobId.ToString("N");
            // var r = System.Web.HttpContext.Current.Request.Form["das"];
            var pic = System.Web.HttpContext.Current.Request.Files["file"];
            upload = new HttpPostedFileWrapper(pic);

            if (upload != null)
            {
                upload.SaveAs(System.IO.Path.Combine(HostingEnvironment.ApplicationPhysicalPath, _blobStoragePath, filename));
            }
            else
            {
                return Json(new { ImageId = -1 });
            }
            var img = new BLL.DTO.ImageDTO
            {
                BlobId = blobId,
                MymeType = upload.ContentType,
                UserId = DbManager.GetUser(Login: User.Identity.Name).Id
            };
            var id = BLL.DbManager.CreateImage(img);
            return Json(new { ImageId = id });

        }
    }
}