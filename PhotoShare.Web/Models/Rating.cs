using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoShare.Web.Models
{
    public class UserRating
    {
        public Guid Id { get; set; }
        public virtual Photo Photo { get; set; }
        public virtual User User { get; set; }
        public int Rating { get; set; }
    }
}