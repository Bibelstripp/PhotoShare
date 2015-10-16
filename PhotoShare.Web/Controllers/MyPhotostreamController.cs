using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoShare.Web.Controllers
{
    public class MyPhotostreamController : Controller
    {
        // GET: MyPhotostream
        public ActionResult Index()
        {
            return View();
        }
    }
}