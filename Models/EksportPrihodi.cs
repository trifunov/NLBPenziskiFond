//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NLBPenziskiFond.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EksportPrihodi
    {
        public Nullable<double> ID_Evid { get; set; }
        public Nullable<System.DateTime> Dat_Valute { get; set; }
        public Nullable<double> Konto { get; set; }
        public string Ozn_ticker { get; set; }
        public string Valuta { get; set; }
        public Nullable<double> Duguje { get; set; }
        public Nullable<double> Potrazuje { get; set; }
        public string ISIN { get; set; }
        public int Id { get; set; }
    }
}