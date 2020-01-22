using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public int? ImageAvatarId { get; set; }

        public string LoginName { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public string Nickname { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsProfileShared { get; set; }
    }
}
