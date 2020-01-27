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
                    dbUser.Id = User.Id;
                    dbUser.LoginName = User.LoginName;
                    dbUser.Nickname = User.Nickname;
                    dbUser.PasswordHash = User.PasswordHash;
                    dbUser.CreateTime = User.CreateTime;
                    dbUser.Salt = User.Salt;
                    dbUser.IsProfileShared = User.IsProfileShared;

                   if (User.ImageAvatarId is int imgId)
                        dbUser.Avatars.Add(new Data.Entities.Avatar { ImageId = imgId });
                    ctx.SaveChanges();

                    return dbUser.Id;
                }
            }
            catch (Exception ex)
            {
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


        public static void CreatePost(PostDTO post)
        {
            using (var ctx = new DbEntities())
            {
                //var imgIds = post.Images.Select(x => x.Id).ToArray();
                //var images = ctx.Images.Where(x => imgIds.Contains(x.Id)).ToList();

                //if (images.Any(x => x.PostId.HasValue))
                //  throw new Exception("Это чужие картинки!!!");

                //var dbPost = AutoMapper.Mapper.Map<DAL.Post>(post);
                var dbPost = new Post();
                //  dbPost.Images.Clear();
                //images.ForEach((x) => dbPost.Images.Add(x));
                dbPost.CreateDate = DateTime.Now;
              //  if (post. is int imgId)
                //    dbUser.Avatars.Add(new Data.Entities.Avatar { ImageId = imgId });

                ctx.Posts.Add(dbPost);
                ctx.SaveChanges();
            }
        }
        public static int CreateImage (ImageDTO Image)
        {
            using (var ctx = new DbEntities())
            {
                var image = new Image();
                image.BlobId = Image.BlobId;
                image.MymeType = Image.MymeType;
                image.UserId = Image.UserId;
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
                var ImageDTO = new ImageDTO();
                ImageDTO.Id = image.Id;
                ImageDTO.MymeType = image.MymeType;
                ImageDTO.UserId = image.UserId;
                ImageDTO.BlobId = image.BlobId;
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
                        UserDTO user = new UserDTO();
                        user.BirthDate = dbUser.BirthDate;
                        user.Id = dbUser.Id;
                        user.LoginName = dbUser.LoginName;
                        user.Nickname = dbUser.Nickname;
                        user.PasswordHash = dbUser.PasswordHash;
                        user.CreateTime = dbUser.CreateTime;
                        user.Salt = dbUser.Salt;
                        user.IsProfileShared = dbUser.IsProfileShared;
                        var Avatar = dbUser.Avatars.FirstOrDefault(x => x.UserId == user.Id);
                        if (Avatar is Avatar)
                        {
                            user.ImageAvatarId = Avatar.ImageId;
                        }
                        else
                        {
                            user.ImageAvatarId = null;
                        }
                       // user.ImageAvatarId = dbUser.Avatars.FirstOrDefault(x => x.UserId == user.Id).ImageId;
                        //throw new Exception($"No User with ID:{id}");
                        return user;
                    }

                    throw new Exception($"Not found User with ID:{id}");
                }
            }
            catch (Exception ex)
            {
                //throw;
                return null;
            }

        }

    }
}
