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
    
    public partial class CR_Cas_Sup_Features
    {
        public string CR_Cas_Sup_Features_Serial_No { get; set; }
        public string CR_Cas_Sup_Features_Code { get; set; }
        public string CR_Cas_Sup_Features_Model_Code { get; set; }
        public string CR_Cas_Sup_Features_Model_Year { get; set; }
        public string CR_Cas_Sup_Features_Lessor_Code { get; set; }
    
        public virtual CR_Cas_Sup_Car_Information CR_Cas_Sup_Car_Information { get; set; }
        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
        public virtual CR_Mas_Sup_Features CR_Mas_Sup_Features { get; set; }
        public virtual CR_Mas_Sup_Model CR_Mas_Sup_Model { get; set; }
    }
}
