using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string MymeType { get; set; }

        public int UserId { get; set; }

        public Guid BlobId { get; set; }

    }
}
