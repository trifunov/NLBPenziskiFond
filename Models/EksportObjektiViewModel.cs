using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLBPenziskiFond.Models
{
    public class EksportObjektiViewModel
    {
        public double? ID_Evid { get; set; }

        public DateTime? Dat_Valute { get; set; }

        public double? Konto { get; set; }

        public string Ozn_ticker { get; set; }

        public string Valuta { get; set; }

        public double? Duguje { get; set; }

        public double? Potrazuje { get; set; }

        public double? Razlika { get; set; }

        public string ISIN { get; set; }

        public int Id { get; set; }
    }
}