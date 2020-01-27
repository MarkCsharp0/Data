using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
      
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string Location { get; set; }

        public string Description { get; set; }

        public List<PostImageDTO> PostImages { get; set; }
    }
}
