using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Comment
    {
        public Comment()
        {

            CommentLikes = new HashSet<CommentLike>();
        }
        public int Id { get; set; }

        public string CommentText { get; set; }

        public virtual ICollection<CommentLike>  CommentLikes { get; set;  }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public virtual User User { get; set; }

        public virtual Post Post { get; set; }


    }
}
