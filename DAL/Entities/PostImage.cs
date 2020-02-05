using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class PostImage
    {

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PostId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 1)]
        public int ImageId { get; set; }

      //  public virtual Post Post { get; set; }
        
        public virtual Image Image { get; set; }

        public int Position { get; set; }
    }
}
