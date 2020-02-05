using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;
namespace Data
{
    class Program
    {
        static void Main(string[] args)
        {
            using (DbEntities db = new DbEntities())
            {

                User first = new User
                {

                    LoginName = "Tom",
                    PasswordHash = "x/dafafw254",
                    Salt = "4134t2r1rf",
                    Nickname = "Tommy",
                    BirthDate = new DateTime(2018, 5, 1, 8, 30, 52),
                    CreateTime = new DateTime(2018, 5, 1, 8, 30, 52),
                    IsProfileShared = false
                };
               db.Users.Add(first);
                db.SaveChanges();
            }
            Console.WriteLine("adwfewrg");
        }
    }
}
