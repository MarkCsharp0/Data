
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User
    {

        public User()
        {
        
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            Subscribers = new HashSet<Subscriber>();
            Subscriptions = new HashSet<Subscriber>();
            Posts = new HashSet<Post>();
        }
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string PasswordHash { get; set; }

        public string Salt { get; set; }

        public string Nickname { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsProfileShared { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Avatar> Avatars { get; set; }
        public virtual ICollection<Subscriber> Subscribers { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Subscriber> Subscriptions { get; set; }

        public virtual ICollection<Like> Likes { get; set; }



    }
}
