using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLBPenziskiFond.Models
{
    public class EksportPrihodiViewModel
    {
        public double? ID_Evid { get; set; }

        public DateTime? Dat_Valute { get; set; }

        public float? Konto { get; set; }

        public string Ozn_ticker { get; set; }

        public string Valuta { get; set; }

        public double? Duguje { get; set; }

        public double? Potrazuje { get; set; }

        public double? Razlika { get; set; }

        public double? Kolicnik { get; set; }

        public string ISIN { get; set; }

        public int Id { get; set; }
    }
}