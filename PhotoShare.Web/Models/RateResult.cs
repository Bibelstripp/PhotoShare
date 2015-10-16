using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoShare.Web.Models
{
    public class RateResult
    {
        public RateResult(double newScore)
        {
            NewScore = newScore;
        }
        public double NewScore { get; set; }
    }
}