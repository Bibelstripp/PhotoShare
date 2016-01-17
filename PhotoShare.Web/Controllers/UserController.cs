using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoShare.Web.Models;
using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PhotoShare.Web.Repositories;

namespace PhotoShare.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly PhotoContext photoContext;
        private readonly UserRepository userRepository;

        public UserController()
        {
            photoContext = new PhotoContext();
            userRepository = new UserRepository(photoContext);
        }

        [Authorize]
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var photos = photoContext.Photos
                .Include(p => p.User)
                .Where(p => p.User.Id == userId)
                .ToList();

            return View(photos);
        }


        [Route("User/{username}", Name = "UserStream")]
        public ActionResult Index(string username)
        {
            var user = userRepository.GetUserByUsername(username);

            if (user == null)
            {
                return base.HttpNotFound();
            }

            var photos = photoContext.Photos
                .Include(p => p.User)
                .Where(p => p.User.Id == user.Id)
                .ToList();

            return View(photos);
        }

        [HttpGet]
        [Authorize]
        [Route("User/{username}/Edit", Name = "EditStream")]
        public ActionResult Edit(string username)
        {
            var user = userRepository.GetUserByUsername(username);

            if (user == null)
            {
                return HttpNotFound();
            }
            else if (user.Id != User.Identity.GetUserId())
            {
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [HttpPost]
        [Authorize]
        [Route("User/{username}/Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, string username)
        {
            var user = userRepository.GetUser(id);
            if (user == null)
            {
                return base.HttpNotFound();
            }

            var existingUser = userRepository.GetUserByUsername(username);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                ModelState.AddModelError("Username", "Username is in use. Please select another one.");
                return View(user);
            }

            user.Username = username;
            userRepository.Save(user);

            return RedirectToRoute("Default", new { action = "Index" });
        }
    }
}