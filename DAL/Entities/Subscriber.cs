using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class Subscriber
    {
        public int Id { get; set; }

        public int SubscriberUserId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual User SubscriberUser { get; set; }
    }
}
