using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Post
    {

        public Post()
        {
            Tags = new HashSet<Tag>();
            Comments = new HashSet<Comment>();
            PostImages = new HashSet<PostImage>();
            PostLikes = new HashSet<PostLike>();
        }
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("User")]
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Location { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<PostImage> PostImages { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual User User { get; set; }
    }
}