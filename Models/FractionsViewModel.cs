using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLBPenziskiFond.Models
{
    public class FractionsViewModel
    {
        public int Id { get; set; }

        public string Isin { get; set; }

        public DateTime? Date { get; set; }

        public string Conto { get; set; }

        public string Ticker { get; set; }

        public string Currency { get; set; }

        public string Type { get; set; }

        public decimal? Sum { get; set; }

        public decimal? Sum_Currency { get; set; }
    }
}