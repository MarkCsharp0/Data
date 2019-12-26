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

                    dbUser.BirthDate = User.BirthDate;
                    //     dbUser. = user.Description;
                    dbUser.Id = User.Id;
                    dbUser.LoginName = User.LoginName;
                    dbUser.Nickname = User.Nickname;
                    dbUser.PasswordHash = User.PasswordHash;
                    dbUser.CreateTime = User.CreateTime;
                    dbUser.Salt = User.Salt;
                    dbUser.IsProfileShared = User.IsProfileShared;

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
            var user = GetUser(Login: login);
            if (user != null)
            {
                var salt = Convert.FromBase64String(user.Salt);
                var passhash = BLL.Hash.GenerateSaltedHash(password, salt);

                var oldHash = Convert.FromBase64String(user.PasswordHash);
                if (BLL.Hash.CompareByteArrays(passhash, oldHash))

                    return BLL.Hash.CompareByteArrays(passhash, oldHash);
            }
            return false;
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
                    var newPasshash = Hash.GenerateSaltedHash(newPassword, salt);
                    user.PasswordHash = Convert.ToBase64String(newPasshash);
                    user.Salt = Convert.ToBase64String(newSalt);

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
