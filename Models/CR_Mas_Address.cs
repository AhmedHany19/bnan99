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
    
    public partial class CR_Mas_Address
    {
        public string CR_Mas_Address_Id_Code { get; set; }
        public string CR_Mas_Address_Short_Code { get; set; }
        public string CR_Mas_Address_Regions { get; set; }
        public string CR_Mas_Address_City { get; set; }
        public string CR_Mas_Address_Ar_District { get; set; }
        public string CR_Mas_Address_En_District { get; set; }
        public string CR_Mas_Address_Fr_District { get; set; }
        public string CR_Mas_Address_Ar_Street { get; set; }
        public string CR_Mas_Address_En_Street { get; set; }
        public string CR_Mas_Address_Fr_Street { get; set; }
        public Nullable<int> CR_Mas_Address_Building { get; set; }
        public string CR_Mas_Address_Unit_No { get; set; }
        public Nullable<int> CR_Mas_Address_Zip_Code { get; set; }
        public Nullable<int> CR_Mas_Address_Additional_Numbers { get; set; }
        public Nullable<System.DateTime> CR_Mas_Address_UpDate_Post { get; set; }
        public string CR_Mas_Address_Status { get; set; }
        public string CR_Mas_Address_Reasons { get; set; }
    
        public virtual CR_Mas_Sup_City CR_Mas_Sup_City { get; set; }
        public virtual CR_Mas_Sup_Regions CR_Mas_Sup_Regions { get; set; }
    }
}
