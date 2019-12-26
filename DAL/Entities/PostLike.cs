using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Data.Entities
{
    public class PostLike
    {
        [Key]
        [ForeignKey("Like")]
        public int LikeId { get; set; }


        public int PostId { get; set; }

        public virtual Post Post { get; set; }

      
        public virtual Like Like { get; set; }
    }
}
