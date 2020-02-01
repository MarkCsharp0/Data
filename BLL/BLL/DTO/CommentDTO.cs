using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public string CommentText { get; set; }
    }
}
