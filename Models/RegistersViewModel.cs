using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLBPenziskiFond.Models
{
    public class RegistersViewModel
    {
        public int Id { get; set; }

        public string Id_Record { get; set; }

        public DateTime? Date { get; set; }

        public string Conto { get; set; }

        public string Ticker { get; set; }

        public string Currency { get; set; }

        public string Type { get; set; }

        public decimal? Daily_Sum { get; set; }

        public decimal? Daily_Sum_Currency { get; set; }

        public decimal? Owe { get; set; }

        public decimal? Claim { get; set; }

        public decimal? Owe_Currency { get; set; }

        public decimal? Claim_Currency { get; set; }
    }
}