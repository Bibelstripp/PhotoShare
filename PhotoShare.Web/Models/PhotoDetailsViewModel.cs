using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoShare.Web.Models
{
    public class PhotoDetailsViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public double Score { get; set; }
        public int Rating { get; set; }
        public DateTime Timestamp { get; set; }
        public string Comment { get; set; }

        public bool IsOwner { get; set; }
    }
}