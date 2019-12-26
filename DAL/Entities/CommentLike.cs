using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class CommentLike
    {
        [Key]
        [ForeignKey("Like")]
        public int LikeId { get; set; }

        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        public virtual Like Like { get; set; }
    }

}
