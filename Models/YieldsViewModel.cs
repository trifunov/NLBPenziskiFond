using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLBPenziskiFond.Models
{
    public class YieldsViewModel
    {
        public int Id { get; set; }

        public string Isin { get; set; }

        public DateTime? Date { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public decimal? Yield { get; set; }

        public decimal? Yield_Currency { get; set; }

        public decimal? Share { get; set; }

        public decimal? Production { get; set; }

        public string Currency { get; set; }

        public string Type { get; set; }
    }
}