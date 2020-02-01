using BLL;
using BLL.DTO;
using MVC.CustomAuth;
using MVC.Models;
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
            try
            {
                var postsDTO = DbManager.GetPosts();
                var model = new List<PostModel>();
                foreach (PostDTO post in postsDTO)
                {
                   // var CustomUser = (CustomPrincipal)User;
                    var fModel = new PostModel();
                    fModel.ImageIds = new List<int>();
                    fModel.Description = post.Description;
                    fModel.Location = post.Location;
                    fModel.UserId = post.UserId;
                    fModel.Id = post.Id;
                 //   fModel.UserName = CustomUser.Nickname;
                    post.ImageIds.ForEach(fModel.ImageIds.Add);
                    model.Add(fModel);
                }
                return View(model);
            } 
            catch(Exception ex)
            {
                return RedirectToAction("CreatePost");

            }
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
          //  var CustomUser = (CustomPrincipal)User;
          //  ViewBag.Images = DbManager.GetMyImagesIds(CustomUser.UserId);
            var model = new PostModel { ImageIds = new List<int>() };
            return View(model);
        }
        [HttpPost]
        public JsonResult CreatePost(MVC.Models.PostModel model)
        {

           // var t = System.Web.HttpContext.Current.Request.Form["Images"];
           // Console.WriteLine(t);
            //var result = new JsonResultResponse { Success = true };
            try
            {
                var CustomUser = (CustomPrincipal)User;
                var post = new PostDTO();
                post.ImageIds = new List<int>();
                post.Description = model.Description;
                post.Location = model.Location;
                post.CreateDate = DateTime.Now;
                post.UserId = CustomUser.UserId;
                
                model.ImageIds.ForEach(post.ImageIds.Add);
                
                DbManager.CreatePost(post);
            }
            catch (Exception ex)
            {
               // result.Success = false;
                //result.Result = ex.Message;
            }
            return new JsonResult();
        }

      /*  public JsonResult GetComments(int id)
        {
            var model = DbManager.GetPostById(id);
            return Json(new { model.Comments });
        }*/

        [HttpPost]
        public JsonResult AddComment(int postId, string commentText)
        {
            //var result = new JsonResultResponse { Success = true };
           // var 
            try
            {
                var CustomUser = (CustomPrincipal)User;
                var userId = CustomUser.UserId;

                var comId = DbManager.CreateComment(new BLL.DTO.CommentDTO { UserId = userId, CommentText = commentText, PostId = postId});

             //   result.Result = BLL.Data.GetComment(comId);

            }
            catch (Exception ex)
            {
               // result.Success = false;
                //result.Result = ex.Message;
            }
            return Json(new { Baka = "adw"});
        }


    }
}