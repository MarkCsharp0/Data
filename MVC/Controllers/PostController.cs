using BLL;
using BLL.DTO;
using MVC.CustomAuth;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class PostController : Controller
    {
        // GET: Post

        public JsonResult GetComment(int? id)
        {
            var com = DbManager.GetComment(id.Value);

            return Json(new { String = com.CommentText });

        }

        public ActionResult GetPost(int? id)
        {
            var post = DbManager.GetPostById(id.Value);
            var fModel = new PostModel
            {
                ImageIds = new List<int>(),
                Comments = new List<int>(),
                Description = post.Description,
                Location = post.Location,
                UserId = post.UserId,
                Id = post.Id
            };
            post.ImageIds.ForEach(fModel.ImageIds.Add);
            post.Comments.ForEach(fModel.Comments.Add);
            return View(fModel);

        }
        [HttpGet]
        public ActionResult EditPost(int? id)
        {
            var post = DbManager.GetPostById(id.Value);
            var fModel = new PostModel
            {
                ImageIds = new List<int>(),
                Comments = new List<int>(),
                Description = post.Description,
                Location = post.Location,
                UserId = post.UserId,
                Id = post.Id
            };
            post.ImageIds.ForEach(fModel.ImageIds.Add);
            post.Comments.ForEach(fModel.Comments.Add);
            return View(fModel);

        }
        public ActionResult Index()
        {
            try
            {
                var postsDTO = DbManager.GetPosts();
                var model = new List<PostModel>();
                foreach (PostDTO post in postsDTO)
                {
                    // var CustomUser = (CustomPrincipal)User;
                    var fModel = new PostModel
                    {
                        ImageIds = new List<int>(),
                        Comments = new List<int>(),
                        Description = post.Description,
                        Location = post.Location,
                        UserId = post.UserId,
                        Id = post.Id
                    };
                    //   fModel.UserName = CustomUser.Nickname;
                    post.ImageIds.ForEach(fModel.ImageIds.Add);
                    post.Comments.ForEach(fModel.Comments.Add);
                    model.Add(fModel);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("CreatePost");

            }
        }

        public ActionResult View()
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
            catch (Exception ex)
            {
                return RedirectToAction("CreatePost");

            }
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            //  var CustomUser = (CustomPrincipal)User;
            //  ViewBag.Images = DbManager.GetMyImagesIds(CustomUser.UserId);
            var model = new PostModel { ImageIds = new List<int>(), Comments = new List<int>(), UserName = User.Identity.Name };
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
                var post = new PostDTO
                {
                    ImageIds = new List<int>(),
                    Comments = new List<int>(),
                    Description = model.Description,
                    Location = model.Location,
                    CreateDate = DateTime.Now,
                    UserId = CustomUser.UserId
                };
                if (model.Id != 0)
                {
                    post.Id = model.Id;
                }
                if (model.Comments != null)
                {
                    model.Comments.ForEach(post.Comments.Add);
                }
                if (model.ImageIds != null)
                {
                    model.ImageIds.ForEach(post.ImageIds.Add);
                }

                DbManager.CreateUpdatePost(post);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

            var res = Json(new { Success = true, Result = new CommentDTO() });
            try
            {
                var CustomUser = (CustomPrincipal)User;
                var userId = CustomUser.UserId;

                var comId = DbManager.CreateComment(new BLL.DTO.CommentDTO { UserId = userId, CommentText = commentText, PostId = postId });

                var post = DbManager.GetPostById(postId);
                var comments = post.Comments;
                // comments.Add(comId);
                post.Comments.Add(comId);
                DbManager.CreateUpdatePost(post);

                return Json(new { Result = comId });

            }
            catch (Exception ex)
            {
                // result.Success = false;
                //result.Result = ex.Message;
                return Json(new { Result = -1 });
            }
        }


    }
}