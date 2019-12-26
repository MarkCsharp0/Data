using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Like
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public bool IsActive { get; set; }

        public virtual PostLike PostLike { get; set; }

        public virtual CommentLike CommentLike { get; set; }
    }
}
