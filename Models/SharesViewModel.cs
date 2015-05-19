using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLBPenziskiFond.Models
{
    public class SharesViewModel
    {
        public int Id { get; set; }

        public string Isin { get; set; }

        public DateTime? Date { get; set; }

        public decimal? Share { get; set; }

        public string Currency { get; set; }
    }
}