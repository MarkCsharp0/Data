using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public Guid BlobId { get; set; }
    }
}