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
        public User User { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual ICollection<UserRating> Ratings { get; set; }
        public double GetScore()
        {
            ///TODO: Calculate the average of all ratings for this photo.
            var score = new Random().NextDouble() * 5.0;

            return score;
        }

        public int GetRating(string userId)
        {
            ///TODO: Get rating for specific user. Return zero if user has not rated.
            var rating = new Random().Next(1, 5);

            return rating;
        }

        public void Rate(int rating, User user)
        {
            ///TODO: Add or update rating for user.
        }
    }
}