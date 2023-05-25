using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentCar.Models.CAS
{
    public class CasUserMD
    {
        public string CR_Cas_User_Information_Id { get; set; }
        public string CR_Cas_User_Information_PassWord { get; set; }
        public string CR_Cas_User_Information_Lessor_Code { get; set; }
        public Nullable<bool> CR_Cas_User_Information_Auth_Branch { get; set; }
        public Nullable<bool> CR_Cas_User_Information_Auth_System { get; set; }
        public Nullable<bool> CR_Cas_User_Information_Auth_Owners { get; set; }
        public string CR_Cas_User_Information_Branch_Code { get; set; }
        public Nullable<decimal> CR_Cas_User_Information_Balance { get; set; }
        public string CR_Cas_User_Information_Ar_Name { get; set; }
        public string CR_Cas_User_Information_En_Name { get; set; }
        public string CR_Cas_User_Information_Fr_Name { get; set; }
        public string CR_Cas_User_Information_Mobile { get; set; }
        public string CR_Cas_User_Information_Signature { get; set; }
        public string CR_Cas_User_Information_Image { get; set; }
        public string CR_Cas_User_Information_Emaile { get; set; }
        public string CR_Cas_User_Information_Status { get; set; }
        public string CR_Cas_User_Information_Reasons { get; set; }

        public int ReceiptDebitCount { get; set; }
        public int ReceiptCreditCount { get; set; }
    }
}