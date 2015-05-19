using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLBPenziskiFond.Models
{
    public class RecordAssetsViewModel
    {
        public int Id { get; set; }

        public string Isin { get; set; }

        public string Id_Record { get; set; }

        public string Ticker { get; set; }

        public string Type { get; set; }

        public string Currency { get; set; }

        public DateTime? Entry_Date { get; set; }

        public string Full_Name { get; set; }

        public string Sector { get; set; }

        public string Country { get; set; }

    }
}