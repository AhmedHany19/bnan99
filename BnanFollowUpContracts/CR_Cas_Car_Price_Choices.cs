//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BnanFollowUpContracts
{
    using System;
    using System.Collections.Generic;
    
    public partial class CR_Cas_Car_Price_Choices
    {
        public string CR_Cas_Car_Price_Choices_No { get; set; }
        public string CR_Cas_Car_Price_Choices_Code { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Choices_Value { get; set; }
    
        public virtual CR_Cas_Car_Price_Basic CR_Cas_Car_Price_Basic { get; set; }
        public virtual CR_Mas_Sup_Choices CR_Mas_Sup_Choices { get; set; }
    }
}
