using BLL;
using MVC.CustomAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult Index()
        {

            return View();
        
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            var CustomUser = (CustomPrincipal)User;
            ViewBag.Images = DbManager.GetMyImagesIds(CustomUser.UserId);
          
            return View();
        }
        [HttpPost]
        public JsonResult CreatePost(BLL.DTO.PostDTO model)
        {

            var t = System.Web.HttpContext.Current.Request.Form["Images"];
            Console.WriteLine(t);
            //var result = new JsonResultResponse { Success = true };
            try
            {
                var CustomUser = (CustomPrincipal)User;
                model.UserId = CustomUser.UserId;
             
                DbManager.CreatePost(model);
            }
            catch (Exception ex)
            {
               // result.Success = false;
                //result.Result = ex.Message;
            }
            return new JsonResult();
        }


    }
}