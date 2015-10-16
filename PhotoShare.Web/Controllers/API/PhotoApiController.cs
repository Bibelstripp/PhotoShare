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

namespace PhotoShare.Web.Controllers.API
{
    public class PhotoApiController : ApiController
    {
        private PhotoContext photoContext = new PhotoContext();

        [Route("api/photo/upload")]
        [HttpPost]
        public HttpResponseMessage Upload()
        {
            var request = HttpContext.Current.Request;
            var comment = request.Form["comment"];
            HttpPostedFile file = request.Files[0];

            var filename = GetUniqueFilename(file.FileName);
            var photo = new Photo
            {
                Id = Guid.NewGuid(),
                UserId = User.Identity.Name,
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

            //return Request.CreateResponse(HttpStatusCode.Redirect, "/photo/" + id);
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

        [Route("api/photo/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            var photo = photoContext.Photos
                .Include(p => p.File)
                .SingleOrDefault(p => p.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            photoContext.Files.Remove(photo.File);
            photoContext.Photos.Remove(photo);
            photoContext.SaveChanges();

            return Ok();
        }
    }
}