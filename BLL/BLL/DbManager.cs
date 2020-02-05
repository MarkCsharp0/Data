using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
using BLL.DTO;
namespace BLL
{
    public static class DbManager
    {
        public static int CreateOrUpdateUser(UserDTO User)
        {
            try
            {
                using (var ctx = new DbEntities())
                {
                    var dbUser = ctx.Users.FirstOrDefault(x => x.Id == User.Id) ?? ctx.Users.Add(new Data.Entities.User());

                    if (ctx.Users.Any(x => x.LoginName == User.LoginName && x.Id != dbUser.Id))
                        throw new Exception($"User with loginName :{User.LoginName} exist");

                    if (!User.CreateTime.Equals(DateTime.MinValue))
                        dbUser.CreateTime = User.CreateTime;

                   // throw new Exception("Hi");
                    dbUser.BirthDate = User.BirthDate;
                //   dbUser.Id = User.Id;
                    dbUser.LoginName = User.LoginName;
                    dbUser.Nickname = User.Nickname;
                    dbUser.PasswordHash = User.PasswordHash;
                    dbUser.CreateTime = User.CreateTime;
                    dbUser.Salt = User.Salt;
                    dbUser.IsProfileShared = User.IsProfileShared;
                    User.PostsId.Select(x => new Post()
                    {
                        Id = x,
                        CreateDate = GetPostById(x).CreateDate,
                        Description = GetPostById(x).Description,
                        Location = GetPostById(x).Location
                    }).ToList().ForEach(dbUser.Posts.Add);
                    if (User.ImageAvatarId is int imgId)
                        dbUser.Avatars.Add(new Data.Entities.Avatar { ImageId = imgId });
                    ctx.SaveChanges();

                    return dbUser.Id;
                }
            }
            catch (Exception ex)
            {
                //ex.Message
                throw;
            }
            // return -1;
        }
        public static bool ValidateUser(string login, string password)
        {
          // return true;
            var user = GetUser(Login: login);
            if (user != null)
            {
                var salt = Convert.FromBase64String(user.Salt);
                var passhash = BLL.Hash.GenerateSaltedHash(password, salt);
           
                var oldHash = Convert.FromBase64String(user.PasswordHash);
               // return true;
                if (BLL.Hash.CompareByteArrays(passhash, oldHash))
                    return BLL.Hash.CompareByteArrays(passhash, oldHash);
            }
            return false;
        }


        public static void CreateUpdatePost(PostDTO post)
        {
            using (var ctx = new DbEntities())
            {
                try
                {

                    if (ctx.Posts.Find(post.Id) is Post oldPost)
                    {
                        // AutoMapper.Mapper.Map(post, dbPost);
                        oldPost.CreateDate = DateTime.Now;
                        oldPost.Description = post.Description;
                       // oldPost.UserId = post.UserId;
                        oldPost.Location = post.Location;
                        oldPost.PostImages.Clear();
                        //oldPost.Comments.Add();
                        //oldPost.Comments.Clear();
                        var i = 0;
                        post.ImageIds.Select(x => new PostImage()
                        {
                            ImageId = x,
                            Position = ++i
                        }).ToList().ForEach(oldPost.PostImages.Add);
                            

                    }
                    else
                    {

                        Post dbPost = new Post
                        {
                            CreateDate = DateTime.Now,
                            Description = post.Description,
                            UserId = post.UserId,
                            Location = post.Location
                        };
                        dbPost.PostImages.Clear();
                        
                //        dbPost.Comments.Clear();
                        var i = 0;
                        post.ImageIds.Select(x => new PostImage()
                        {
                            ImageId = x,
                            Position = ++i
                        }).ToList().ForEach(dbPost.PostImages.Add);
                      /*      if (post.Comments != null)
                           {
                               post.Comments.Select(x => new Comment()
                               {
                                   Id = x,
                                   CommentText = GetComment(x).CommentText,
                                   UserId = GetComment(x).UserId,
                                   PostId = GetComment(x).PostId
                               }).ToList().ForEach(dbPost.Comments.Add);
                           }*/

                        ctx.Posts.Add(dbPost);
                    }

                    ctx.SaveChanges();
                }

            catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public static int CreateImage (ImageDTO Image)
        {
            using (var ctx = new DbEntities())
            {
                var image = new Image
                {
                    BlobId = Image.BlobId,
                    MymeType = Image.MymeType,
                    UserId = Image.UserId
                };
                // image.Avatar
                var dbImg = ctx.Images.Add(image);
                ctx.SaveChanges();
                return dbImg.Id;
            }

        }

        public static List<int> GetMyImagesIds(int UserId)
        {
            var res = new List<int>();
            using (var ctx = new DbEntities())
            {
                //  res.AddRange(ctx.Images.Find(x => x.UserId == UserId);
                res.AddRange(ctx.Images.Where(x => x.UserId == UserId).Select(x => x.Id));
               // ctx.Images.All();
            }

            return res;
        }

        public static void DelImage(long ImageId)
        {
            using (var ctx = new DbEntities())
            {
                var img = ctx.Images.FirstOrDefault(x => x.Id == ImageId);
                if (img == null)
                    throw new Exception("Not found image");

                ctx.Images.Remove(img);

                ctx.SaveChanges();
            }

        }

        public static DTO.ImageDTO GetImage(int id)
        {
            using (var ctx = new DbEntities())
            {
                var image = ctx.Images.Find(id);
                var ImageDTO = new ImageDTO
                {
                    Id = image.Id,
                    MymeType = image.MymeType,
                    UserId = image.UserId,
                    BlobId = image.BlobId
                };
                return ImageDTO;
            }
        }

        public static bool ChangePassword(string login, string oldPassword, string newPassword)
        {
            var user = GetUser(Login: login);
            if (user != null)
            {
                var salt = Convert.FromBase64String(user.Salt);
                var passhash = BLL.Hash.GenerateSaltedHash(oldPassword, salt);

                var oldHash = Convert.FromBase64String(user.PasswordHash);
                if (BLL.Hash.CompareByteArrays(passhash, oldHash))
                {
                    var newSalt = Hash.CreateSalt(16);
                    var newPasshash = Hash.GenerateSaltedHash(newPassword, newSalt);
                    user.PasswordHash = Convert.ToBase64String(newPasshash);
                    user.Salt = Convert.ToBase64String(newSalt);
                    return true;
                }
                else
                {
                    return false;
                }
                // return BLL.Hash.CompareByteArrays(passhash, oldHash);
            }
            return false;
        }

        public static UserDTO GetUser(int? id = null, string Login = null)
        {
            if (!id.HasValue && string.IsNullOrEmpty(Login))
                return null;
                //throw new Exception($"Not  User with ID:");
          //  return null;
            try
            {
                using (var ctx = new DbEntities())
                {
                    var dbUser = ctx.Users.FirstOrDefault(x => (x.Id == id || x.LoginName == Login));
                    if (dbUser != null)
                    {
                        UserDTO user = new UserDTO
                        {
                            BirthDate = dbUser.BirthDate,
                            Id = dbUser.Id,
                            PostsId = new List<int>(),
                            LoginName = dbUser.LoginName,
                            Nickname = dbUser.Nickname,
                            PasswordHash = dbUser.PasswordHash,
                            CreateTime = dbUser.CreateTime,
                            Salt = dbUser.Salt,
                            IsProfileShared = dbUser.IsProfileShared
                        };
                        dbUser.Posts.Select(x => x.Id).ToList().ForEach(user.PostsId.Add);
                        var Avatar = dbUser.Avatars.FirstOrDefault(x => x.UserId == user.Id);
                        if (Avatar is Avatar)
                        {
                            user.ImageAvatarId = Avatar.ImageId;
                        }
                        else
                        {
                            user.ImageAvatarId = null;
                        }
                      
                     
                        return user;
                    }

                    throw new Exception($"Not found User with ID:{id}");
                }
            }
            catch (Exception ex)
            { //ex.Message
                //throw;
                return null;
            }

        }

        public static int CreateComment(CommentDTO com)
        {
            using (var ctx = new DbEntities())
            {
                var dbCom = new Comment
                {
                    CommentText = com.CommentText,
                    PostId = com.PostId,
                    UserId = com.UserId
                };
                ctx.Comments.Add(dbCom);
                ctx.SaveChanges();

                return dbCom.Id;
            }
        }

        public static PostDTO GetPostById(int id)
        {
            //var res = (PostDTO)null;
            using (var ctx = new DbEntities())
            {
                var dbpost = ctx.Posts.Where(x => x.Id == id).FirstOrDefault();
                var post = new PostDTO
                {
                    ImageIds = new List<int>(),
                    Comments = new List<int>(),
                    CreateDate = dbpost.CreateDate,
                    Description = dbpost.Description,
                    Location = dbpost.Location,
                    UserId = dbpost.UserId
                };
                dbpost.PostImages.Select(x => x.ImageId).ToList().ForEach(post.ImageIds.Add);
                dbpost.Comments.Select(x => x.Id).ToList().ForEach(post.Comments.Add);
                return post;
            }

            
        }

        public static CommentDTO GetComment(int id)
        {
            using (var ctx = new DbEntities())
            {
                var ct = ctx.Comments
                    .Where(x => x.Id == id)
                    .FirstOrDefault();
                var comDTO = new CommentDTO
                {
                    UserId = ct.UserId,
                    PostId = ct.PostId,
                    CommentText = ct.CommentText,
                    Id = ct.Id
                };

                return comDTO;
            }
        }

        public static List<UserDTO> GetUsers(int? lastId = null)
        {
            using (var ctx = new DbEntities())
            {
                var users = ctx.Users.AsNoTracking().AsQueryable();
                if (lastId.HasValue)
                    users = users.Where(x => x.Id < lastId);

                var usersDTO = new List<UserDTO>();
                var dbusers = users.OrderByDescending(x => x.Id).Take(3).ToList();
                foreach (User dbUser in dbusers)
                {
                    var user = new UserDTO
                    {
                        BirthDate = dbUser.BirthDate,
                        Id = dbUser.Id,
                        LoginName = dbUser.LoginName,
                        Nickname = dbUser.Nickname,
                        PasswordHash = dbUser.PasswordHash,
                        CreateTime = dbUser.CreateTime,
                        Salt = dbUser.Salt,
                        IsProfileShared = dbUser.IsProfileShared
                    };
                    dbUser.Posts.Select(x => x.Id).ToList().ForEach(user.PostsId.Add);
                    var Avatar = dbUser.Avatars.FirstOrDefault(x => x.UserId == user.Id);
                    if (Avatar is Avatar)
                    {
                        user.ImageAvatarId = Avatar.ImageId;
                    }
                    else
                    {
                        user.ImageAvatarId = null;
                    }
                    usersDTO.Add(user);
                }

                return usersDTO;

            }
        }

        public static List<PostDTO> GetPosts(int? lastId = null)
        {
            using (var ctx = new DbEntities())
            {
                var posts = ctx.Posts.AsNoTracking().AsQueryable();
                if (lastId.HasValue)
                    posts = posts.Where(x => x.Id < lastId);

                var postsDTO = new List<PostDTO>();
                var dbposts = posts.OrderByDescending(x => x.Id).Take(3).ToList();
                foreach (Post dbpost in dbposts)
                {
                    var post = new PostDTO
                    {
                        ImageIds = new List<int>(),
                        Comments = new List<int>(),
                        CreateDate = dbpost.CreateDate,
                        Description = dbpost.Description,
                        Location = dbpost.Location,
                        UserId = dbpost.UserId,
                        Id = dbpost.Id
                    };
                    dbpost.PostImages.Select(x => x.ImageId).ToList().ForEach(post.ImageIds.Add);
                    dbpost.Comments.Select(x => x.Id).ToList().ForEach(post.Comments.Add);
                    postsDTO.Add(post);
                }

                return postsDTO;

            }
        }

    }
}
