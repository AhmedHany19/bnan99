//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RentCar.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CR_Mas_Service_Tamm_Contract
    {
        public string CR_Mas_Service_Tamm_Contract_No { get; set; }
        public string CR_Mas_Service_Tamm_Contract_Code { get; set; }
        public Nullable<decimal> CR_Mas_Service_Tamm_Contract_Fees { get; set; }
    
        public virtual CR_Cas_Company_Contract CR_Cas_Company_Contract { get; set; }
        public virtual CR_Mas_Sup_Service_Fee_Tamm CR_Mas_Sup_Service_Fee_Tamm { get; set; }
    }
}
