using System;

namespace RentCar.Models.CAS
{
    public class CarInfoModel
    {
        public string CR_Cas_Sup_Car_Serail_No { get; set; }
        public string CR_Cas_Sup_Car_Lessor_Code { get; set; }
        public string CR_Cas_Sup_Car_Owner_Branch_Code { get; set; }
        public string CR_Cas_Sup_Car_Location_Branch_Code { get; set; }
        public string CR_Cas_Sup_Car_Owner_Code { get; set; }
        public string CR_Cas_Sup_Car_Beneficiary_Code { get; set; }
        public string CR_Cas_Sup_Car_Customs_No { get; set; }
        public string CR_Cas_Sup_Car_Structure_No { get; set; }
        public string CR_Cas_Sup_Car_Registration_Code { get; set; }
        public string CR_Cas_Sup_Car_Brand_Code { get; set; }
        public string CR_Cas_Sup_Car_Model_Code { get; set; }
        public Nullable<int> CR_Cas_Sup_Car_Year { get; set; }
        public string CR_Cas_Sup_Car_Category_Code { get; set; }
        public string CR_Cas_Sup_Car_Out_Main_Color_Code { get; set; }
        public string CR_Cas_Sup_Car_Out_Secondary_Color_Code { get; set; }
        public string CR_Cas_Sup_Car_In_Main_Color_Code { get; set; }
        public string CR_Cas_Sup_Car_In_Secondary_Color_Code { get; set; }
        public string CR_Cas_Sup_Car_Plate_Ar_No { get; set; }
        public string CR_Cas_Sup_Car_Plate_En_No { get; set; }
        public Nullable<int> CR_Cas_Sup_Car_No_Current_Meter { get; set; }
        public string CR_Cas_Sup_Car_Traffic_License_Img { get; set; }
        public Nullable<System.DateTime> CR_Cas_Sup_Car_Joined_Fleet_Date { get; set; }
        public Nullable<System.DateTime> CR_Cas_Sup_Car_Left_Fleet_Date { get; set; }
        public string CR_Cas_Sup_Car_Last_Pictures { get; set; }
        public Nullable<bool> CR_Cas_Sup_Car_Documentation_Status { get; set; }
        public Nullable<bool> CR_Cas_Sup_Car_Maintenance_Status { get; set; }
        public string CR_Cas_Sup_Car_Status { get; set; }
        public string CR_Cas_Sup_Car_Reasons { get; set; }
        public string CR_Cas_Sup_Car_Collect_Ar_Name { get; set; }
        public string CR_Cas_Sup_Car_Collect_Ar_Short_Name { get; set; }
        public string CR_Cas_Sup_Car_Collect_En_Name { get; set; }
        public string CR_Cas_Sup_Car_Collect_En_Short_Name { get; set; }
        public string CR_Cas_Sup_Car_Collect_Fr_Name { get; set; }
        public string CR_Cas_Sup_Car_Collect_Fr_Short_Name { get; set; }
        public string CR_Cas_Sup_Car_Price_Status { get; set; }

        public bool Status { get; set; }

        public virtual CR_Cas_Sup_Beneficiary CR_Cas_Sup_Beneficiary { get; set; }
        public virtual CR_Cas_Sup_Branch CR_Cas_Sup_Branch { get; set; }
        public virtual CR_Cas_Sup_Owners CR_Cas_Sup_Owners { get; set; }
        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
        public virtual CR_Mas_Sup_Brand CR_Mas_Sup_Brand { get; set; }
        public virtual CR_Mas_Sup_Category_Car CR_Mas_Sup_Category_Car { get; set; }
        public virtual CR_Mas_Sup_Color CR_Mas_Sup_Color { get; set; }
        public virtual CR_Mas_Sup_Model CR_Mas_Sup_Model { get; set; }
        public virtual CR_Mas_Sup_Registration_Car CR_Mas_Sup_Registration_Car { get; set; }
    }
}