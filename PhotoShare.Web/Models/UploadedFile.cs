using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoShare.Web.Models
{
    public class UploadedFile
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}