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
    
    public partial class Registers_Yields
    {
        public string Isin { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Conto { get; set; }
        public string Ticker { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> Sum_Registers { get; set; }
        public Nullable<decimal> Sum_Registers_Currency { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
