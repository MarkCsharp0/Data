using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Data.Entities
{
    public class Avatar
    {
        [Key]
        [ForeignKey("Image")]
        public int ImageId { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

      
        public virtual Image Image { get; set; }

    }
}