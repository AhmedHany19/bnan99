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
    
    public partial class CR_Mas_Basic_Contract
    {
        public string CR_Mas_Basic_Contract_No { get; set; }
        public string CR_Mas_Basic_Contract_Year { get; set; }
        public string CR_Mas_Basic_Contract_Sector { get; set; }
        public string CR_Mas_Basic_Contract_Code { get; set; }
        public string CR_Mas_Basic_Contract_Lessor { get; set; }
        public Nullable<int> CR_Mas_Basic_Contract_Com_Code { get; set; }
        public Nullable<System.DateTime> CR_Mas_Basic_Contract_Date { get; set; }
        public Nullable<System.DateTime> CR_Mas_Basic_Contract_Start_Date { get; set; }
        public Nullable<System.DateTime> CR_Mas_Basic_Contract_End_Date { get; set; }
        public Nullable<decimal> CR_Mas_Basic_Contract_Annual_Fees { get; set; }
        public Nullable<decimal> CR_Mas_Basic_Contract_Service_Fees { get; set; }
        public Nullable<decimal> CR_Mas_Basic_Contract_Discount_Rate { get; set; }
        public Nullable<decimal> CR_Mas_Basic_Contract_Tax_Rate { get; set; }
        public string CR_Mas_Basic_Contract_Tamm_User_Id { get; set; }
        public string CR_Mas_Basic_Contract_Tamm_User_PassWord { get; set; }
        public string CR_Mas_Basic_Contract_Status { get; set; }
        public string CR_Mas_Basic_Contract_Reasons { get; set; }
    
        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
        public virtual CR_Mas_Sup_Sector CR_Mas_Sup_Sector { get; set; }
    }
}
