using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PhotoShare.Web.Models;
using System.Data.Entity;

namespace PhotoShare.Web.Repositories
{
    public class PhotoRepository
    {
        private readonly PhotoContext photoContext;
        public PhotoRepository(PhotoContext context)
        {
            photoContext = context;
        }

        public Photo GetPhoto(Guid id)
        {
            return photoContext.Photos
                .Include(p => p.File)
                .Include(p => p.User)
                .SingleOrDefault(p => p.Id == id);
        }

        public UploadedFile GetFile(Guid id)
        {
            return photoContext.Photos
                .Where(p => p.Id == id)
                .Select(p => p.File)
                .SingleOrDefault();
        }

        public void Save(Photo photo)
        {
            photoContext.Entry(photo).State = EntityState.Modified;
            photoContext.SaveChanges();
        }
    }
}