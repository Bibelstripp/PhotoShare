using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoShare.Web.Models;

namespace PhotoShare.Web.Controllers
{
    public class HomeController : Controller
    {
        private PhotoContext photoContext = new PhotoContext();

        public ActionResult Index()
        {
            var photos = photoContext.Photos;
                //.OrderByDescending(p => p.Timestamp)
                //.Take(9)
                //.ToArray();

            return View(photos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}