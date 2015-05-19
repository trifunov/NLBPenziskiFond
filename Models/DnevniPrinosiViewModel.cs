using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLBPenziskiFond.Models
{
    public class DnevniPrinosiViewModel
    {
        public int Id { get; set; }

        public string Id_Evid { get; set; }

        public DateTime? Datum_valuta { get; set; }

        public double? Prinos { get; set; }

        public string Isin { get; set; }

        public string Izdavatelj { get; set; }

        public string Sektor { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public string DateFromAktiven { get; set; }

        public string DateToAktiven { get; set; }
    }
}