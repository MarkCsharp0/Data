using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Data.Entities
{
    public class Tag
    {
        [Key]
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
