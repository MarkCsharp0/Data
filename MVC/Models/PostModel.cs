using System.Collections.Generic;

namespace MVC.Models
{
    public class PostModel
    {
        public int Id { get; set; }

        public string Location { get; set; }


        public string Description { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public bool CanEdit { get; set; }

        public List<int> ImageIds { get; set; }

        public List<int> Comments { get; set; }

    }
}