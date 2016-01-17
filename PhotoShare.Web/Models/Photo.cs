using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoShare.Web.Models
{
    public class Photo
    {
        public Guid Id { get; set; }
        public UploadedFile File { get; set; }
        public string Comment { get; set; }
        public virtual User User { get; set; } //denne må man ofte include for ikke å få NULL når man skal hente den ut, for å slippe å include kan man sette den til Virtual
        //EntityFramework: hvis man har en object referanse og man har den som virtual da vil den automatisk lastes fra db i stedet for å måtte velge include i select setningen
        public DateTime Timestamp { get; set; }

        public virtual ICollection<UserRating> Ratings { get; set; }

        PhotoContext photoContext = new PhotoContext();

        public double GetScore(Guid id)
        {
            double ratingCount = photoContext.UserRating
                .Where(a => a.Photo.Id == id)
                .Select(a => a.Rating).DefaultIfEmpty(0).Count();

            double ratingSum = photoContext.UserRating
                .Where(a => a.Photo.Id == id)
                .Select(a => a.Rating).DefaultIfEmpty(0).Sum();

            var averageRating = ratingSum / ratingCount;
            /////TODO: Calculate the average of all ratings for this photo.
            //var score = new Random().NextDouble() * 5.0;

            return averageRating;
        }

        public int GetRating(string userId, Guid photoId)
        {
            int rating = photoContext.UserRating
                .Where(r => r.User.Id == userId)
                .Where(r => r.Photo.Id == photoId)
                .Select(r => r.Rating)
                .FirstOrDefault();
            return rating;
        }

        public void Rate(int rating, User user)
        {
            ///TODO: Add or update rating for user.
        }
    }
}