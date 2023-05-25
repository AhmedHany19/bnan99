using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentCar.Models.MAS
{
    public class MasTracingMD
    {
        public string CR_Cas_Administrative_Procedures_No { get; set; }
        public Nullable<System.DateTime> CR_Cas_Administrative_Procedures_Date { get; set; }
        public Nullable<System.TimeSpan> CR_Cas_Administrative_Procedures_Time { get; set; }
        public string CR_Cas_Administrative_Procedures_Year { get; set; }
        public string CR_Cas_Administrative_Procedures_Sector { get; set; }
        public string CR_Cas_Administrative_Procedures_Code { get; set; }
        public Nullable<int> CR_Cas_Administrative_Int_Procedures_Code { get; set; }
        public string CR_Cas_Administrative_Procedures_Lessor { get; set; }
        public string CR_Cas_Administrative_Procedures_Targeted_Action { get; set; }
        public string CR_Cas_Administrative_Procedures_Com_Supporting { get; set; }
        public Nullable<decimal> CR_Cas_Administrative_Procedures_Value { get; set; }
        public string CR_Cas_Administrative_Procedures_Doc_No { get; set; }
        public Nullable<System.DateTime> CR_Cas_Administrative_Procedures_Doc_Date { get; set; }
        public Nullable<System.DateTime> CR_Cas_Administrative_Procedures_Doc_Start_Date { get; set; }
        public Nullable<System.DateTime> CR_Cas_Administrative_Procedures_Doc_End_Date { get; set; }
        public string CR_Cas_Administrative_Procedures_From_Branch { get; set; }
        public string CR_Cas_Administrative_Procedures_To_Branch { get; set; }
        public Nullable<bool> CR_Cas_Administrative_Procedures_Action { get; set; }
        public string CR_Cas_Administrative_Procedures_User_Insert { get; set; }
        public string CR_Cas_Administrative_Procedures_Type { get; set; }
        public string CR_Cas_Administrative_Procedures_Reasons { get; set; }
        public int ContractNumber { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
    }
}