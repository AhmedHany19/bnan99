using System;

namespace RentCar.Models
{
    public class BranchValidityModel
    {
        public string CR_Cas_User_Branch_Validity_Id { get; set; }
        public string CR_Cas_User_Branch_Validity_Branch { get; set; }

        public string CR_Cas_Sup_Branch_Code { get; set; }
        public string CR_Cas_Sup_Branch_Ar_Name { get; set; }
        public string CR_Cas_Sup_Branch_Ar_Short_Name { get; set; }
        public string CR_Cas_Sup_Branch_En_Name { get; set; }
        public string CR_Cas_Sup_Branch_En_Short_Name { get; set; }
        public string CR_Cas_Sup_Branch_Fr_Name { get; set; }
        public string CR_Cas_Sup_Branch_Fr_Short_Name { get; set; }
        public Nullable<bool> check { get; set; }
    }
}