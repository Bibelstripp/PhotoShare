using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PhotoShare.Web.Models;
using PhotoShare.Web.Controllers;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Microsoft.AspNet.Identity;
using PagedList;
using PhotoShare.Web.Repository;

namespace PhotoShare.Web.Controllers
{
    public class PhotoController : Controller
    {
    private PhotoRepository photoRepository = new PhotoRepository(new PhotoContext());

        [HttpGet]
        [Route("Images/{id:guid}")]
        public FileResult Index(Guid id, int? thumb)
        {

            var file = photoRepository.GetFile(id);

            byte[] content;
            if (thumb.HasValue)
            {
                content = ResizeImage(file.Content, thumb.Value);
            }
            else
            {
                content = file.Content;
            }

            return File(content, "image/jpeg");
        }


        private PhotoContext photoContext = new PhotoContext();
        
        // GET: Photo
        public ActionResult View(Guid id)
        {
            var photo = photoRepository.GetPhoto(id);
            
            if (photo == null)
            {
                return HttpNotFound();
            }
            else
            {
                var viewModel = new PhotoDetailsViewModel
                {
                    Id = photo.Id,
                    Comment = photo.Comment,
                    Username = photo.User.Username,
                    Timestamp = photo.Timestamp,
                    Score = photo.GetScore(),
                    Rating = photo.GetRating(User.Identity.GetUserId()),
                };
                return View(viewModel);
            }
        }

        public ViewResult Upload()
        {
            return View();
        }

        public static byte[] ResizeImage(byte[] bytes, int size)
        {
            var image = Image.FromStream(new MemoryStream(bytes));
            int height;
            int width;
            if (image.Width > image.Height)
            {
                var ratio = (double)image.Height / (double) image.Width;
                height = size;
                width = (int) ((double)size / ratio);
            }
            else
            {
                var ratio = (double)image.Width / (double) image.Height;
                width = size;
                height = (int) ((double)size / ratio);
            }

            var bitmap = ResizeImage(image, width, height);
            var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Jpeg);

            return stream.ToArray();
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}