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
    
    public partial class CR_Cas_Sup_Follow_Up_Mechanism
    {
        public string CR_Cas_Sup_Follow_Up_Mechanism_Lessor_Code { get; set; }
        public string CR_Cas_Sup_Follow_Up_Mechanism_Procedures_Code { get; set; }
        public string CR_Cas_Sup_Procedures_Type { get; set; }
        public Nullable<bool> CR_Cas_Sup_Follow_Up_Mechanism_Activate_Service { get; set; }
        public Nullable<decimal> CR_Cas_Sup_Follow_Up_Mechanism_Befor_Credit_Limit { get; set; }
        public Nullable<int> CR_Cas_Sup_Follow_Up_Mechanism_Befor_Expire { get; set; }
        public Nullable<int> CR_Cas_Sup_Follow_Up_Mechanism_Befor_KM { get; set; }
        public Nullable<int> CR_Cas_Sup_Follow_Up_Mechanism_After_Expire { get; set; }
        public Nullable<int> CR_Cas_Sup_Follow_Up_Mechanism_Default_KM { get; set; }
    
        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
        public virtual CR_Mas_Sup_Procedures CR_Mas_Sup_Procedures { get; set; }
    }
}
