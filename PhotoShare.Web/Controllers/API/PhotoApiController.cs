using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PhotoShare.Web.Models;
using System.Data.Entity;
using PhotoShare.Web.Repositories;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;

namespace PhotoShare.Web.Controllers.API
{    
    public class PhotoApiController : ApiController
    {
        private PhotoContext photoContext;
        private UserRepository  userRepository;
        private PhotoRepository photoRepository;

        public PhotoApiController()
        {
            photoContext = new PhotoContext();
            userRepository = new UserRepository(photoContext);
            photoRepository = new PhotoRepository(photoContext);
        }

        [Route("api/photo/upload")]
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            var request = HttpContext.Current.Request;
            var comment = request.Form["comment"];
            HttpPostedFile file = request.Files[0];

            var user = userRepository.GetUser(User.Identity.GetUserId());

            var filename = GetUniqueFilename(file.FileName);
            var photo = new Photo
            {
                Id = Guid.NewGuid(),
                User = user,
                Comment = comment,
                File = new UploadedFile
                {
                    Name = filename,
                    Content = ConvertToByteArray(file.InputStream)
                },
                Timestamp = DateTime.Now
            };


            photoContext.Photos.Add(photo);
            photoContext.SaveChanges();

            var id = photo.Id;

            return Request.CreateResponse(HttpStatusCode.OK, id);
        }

        private string GetUniqueFilename(string filename)
        {
            var file = photoContext.Files.SingleOrDefault(f => f.Name == filename);

            if (file == null)
            {
                return filename;
            }
            else
            {
                var lastDotPosition = filename.LastIndexOf('.');

                var mainPart = filename.Substring(0, lastDotPosition);
                var extension = filename.Substring(lastDotPosition);
                var newName = mainPart + "1" + extension;

                return GetUniqueFilename(newName);
            }
        }

        private byte[] ConvertToByteArray(Stream stream)
        {
            var memoryStream = new MemoryStream();

            stream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }

        [Route("api/photo/{id:guid}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            var photo = photoRepository.GetPhoto(id);

            if (photo == null){
                return NotFound();
            }

            photoContext.Photos.Remove(photo); 
            photoContext.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Route("api/photo/{id:guid}/rate/{rating:int}")]
        public IHttpActionResult PutRate(Guid id, int rating)
        {
            ///TODO: Rate photo, return new score.
            var newScore = 1.1;

            return Ok(new RateResult(newScore));
        }

        public class UpdateCommentRequest
        {
            [Required]
            public string Comment { get; set; }
        }

        [HttpPut]
        [Route("api/photo/{id:guid}/comment")]
        [Authorize]
        public IHttpActionResult Comment(Guid id, [FromBody]UpdateCommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var photo = photoRepository.GetPhoto(id);
            if (photo == null)
            {
                return NotFound();
            }
            else if (photo.User.Id != User.Identity.GetUserId())
            {
                return BadRequest();
            }

            photo.Comment = request.Comment;
            photoRepository.Save(photo);

            return Ok();
        }

    }
}