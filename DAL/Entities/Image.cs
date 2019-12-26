using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string MymeType { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual Avatar Avatar { get; set; }

        public virtual PostImage PostImage { get; set; }


    }
}
