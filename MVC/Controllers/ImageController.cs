using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
namespace MVC.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        public ActionResult Index() { 
        
            return View();
        }

       public ActionResult Get (int? id)
        {
            if (id.HasValue)
            {
                var image = DbManager.GetImage(id.Value);
                var ba = Utilities.FileManager.ReadFile(image.BlobId);
                return File(ba, image.MymeType);

            } else
            {
                return HttpNotFound("File not found");
            }
            //return File()
        }



    }
}