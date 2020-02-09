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
            var user = DbManager.GetUser(com.UserId);
            return Json(new { Text = com.CommentText, Nick = user.Nickname, Id = com.Id});

        }

        public JsonResult ChangePhoto(int oldId, int newId)
        {
            var post = DbManager.GetPostByImageId(oldId);
            post.ImageIds.Remove(oldId);
            post.ImageIds.Add(newId);
            DbManager.CreateUpdatePost(post);

            return Json(new { Success = true });
        }

        [HttpGet]
        public JsonResult GetMorePosts(int id)
        {
            var postsDTO = DbManager.GetPosts(id);
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
                    UserName = DbManager.GetUser(id: post.UserId).Nickname,
                    Id = post.Id
                };
                fModel.CanEdit = User.Identity.IsAuthenticated && fModel.UserId == ((CustomAuth.CustomPrincipal)User).UserId;
                //   fModel.UserName = CustomUser.Nickname;
                post.ImageIds.ForEach(fModel.ImageIds.Add);
                post.Comments.ForEach(fModel.Comments.Add);
                model.Add(fModel);
            }
           

            return Json(model, JsonRequestBehavior.AllowGet);
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
                UserName = DbManager.GetUser(id:post.UserId).Nickname,
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
                  
                    var fModel = new PostModel
                    {
                        ImageIds = new List<int>(),
                        Comments = new List<int>(),
                        Description = post.Description,
                        Location = post.Location,
                        UserId = post.UserId,
                        UserName = DbManager.GetUser(id: post.UserId).Nickname,
                        Id = post.Id
                    };
                    fModel.CanEdit = User.Identity.IsAuthenticated && fModel.UserId == ((CustomAuth.CustomPrincipal)User).UserId;
                   
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
               
                    var fModel = new PostModel();
                    fModel.ImageIds = new List<int>();
                    fModel.Description = post.Description;
                    fModel.Location = post.Location;
                    fModel.UserId = post.UserId;
                    fModel.Id = post.Id;
                   
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
        [Authorize]
        public ActionResult CreatePost(int? id)
        {
              var CustomUser = (CustomPrincipal)User;
            //  ViewBag.Images = DbManager.GetMyImagesIds(CustomUser.UserId);
            if (id.HasValue)
            {
                var post = DbManager.GetPostById(id.Value);
                var userId = CustomUser.UserId;
                if (userId != post.UserId)
                {
                    ModelState.AddModelError("", "We don't have access");
                    return RedirectToAction("Index");
                }
                var model = new PostModel
                {
                    Id = post.Id,
                    Location = post.Location,
                    UserId = post.UserId,
                    Comments = new List<int>(),
                    ImageIds = new List<int>(),
                    Description = post.Description,
                    UserName = DbManager.GetUser(id: post.UserId).Nickname
                };
                post.Comments.ForEach(model.Comments.Add);
                post.ImageIds.ForEach(model.ImageIds.Add);
                return View(model);
            }
            else
            {
                var model = new PostModel { ImageIds = new List<int>(), Comments = new List<int>(), UserName = User.Identity.Name, UserId = DbManager.GetUser(Login:User.Identity.Name).Id };
                return View(model);
            }
            
        }
        [HttpPost]
        public ActionResult CreatePost(MVC.Models.PostModel model)
        {
            if (!ModelState.IsValid)
            {
                  ModelState.AddModelError("", "Description invalid");
                  return View(model);
            
            }         
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
                    UserId = model.UserId
                };
                
                if (model.Id != 0)
                {
                    post.Id = model.Id;
                    post.UserId = model.UserId;
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
                //Console.WriteLine(ex.Message);
                return Json(new { Success = false });
                // result.Success = false;
                //result.Result = ex.Message;
            }
            return Json(new { Success = true});
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