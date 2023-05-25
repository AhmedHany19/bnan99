using System;
using System.Collections.Generic;

namespace RentCar.Models.BranchSys
{
    public class CarInfoPrice
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

        public virtual CR_Cas_Sup_Beneficiary CR_Cas_Sup_Beneficiary { get; set; }
        public virtual CR_Cas_Sup_Branch CR_Cas_Sup_Branch { get; set; }
        public virtual CR_Cas_Sup_Owners CR_Cas_Sup_Owners { get; set; }
        public virtual CR_Mas_Com_Lessor CR_Mas_Com_Lessor { get; set; }
        public virtual CR_Mas_Sup_Brand CR_Mas_Sup_Brand { get; set; }
        public virtual CR_Mas_Sup_Category_Car CR_Mas_Sup_Category_Car { get; set; }
        public virtual CR_Mas_Sup_Color CR_Mas_Sup_Color { get; set; }
        public virtual CR_Mas_Sup_Model CR_Mas_Sup_Model { get; set; }


        /// <summary>
        // Added due to updates in branch rented cars stat
        
        public virtual CR_Cas_Contract_Basic CR_Cas_Contract_Basic { get; set; }
        public virtual CR_Cas_Renter_Lessor CR_Cas_Renter_Lessor { get; set; }
        /// </summary>
        public virtual string PassCarDoc { get; set; }
        public virtual string PassCarMain { get; set; }
        public virtual string CarsMainError { get; set; }
        public virtual string CarsDocError { get; set; }
        public virtual CR_Mas_Sup_Registration_Car CR_Mas_Sup_Registration_Car { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Car_Doc_Mainten> CR_Cas_Sup_Car_Doc_Mainten { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CR_Cas_Sup_Features> CR_Cas_Sup_Features { get; set; }




        /// <summary>
        /// ////////Basic price
        /// </summary>
        public string CR_Cas_Car_Price_Basic_No { get; set; }
        public Nullable<System.DateTime> CR_Cas_Car_Price_Basic_Date { get; set; }
        public Nullable<System.DateTime> CR_Cas_Car_Price_Basic_Start_Date { get; set; }
        public Nullable<System.DateTime> CR_Cas_Car_Price_Basic_End_Date { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Daily_Rent { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Weekly_Rent { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Monthly_Rent { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Rental_Tax_Rate { get; set; }
        public Nullable<int> CR_Cas_Car_Price_Basic_No_Daily_Free_KM { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Additional_KM_Value { get; set; }
        public Nullable<int> CR_Cas_Car_Price_Basic_No_Free_Additional_Hours { get; set; }
        public Nullable<int> CR_Cas_Car_Price_Basic_Hour_Max { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Extra_Hour_Value { get; set; }
        public Nullable<int> CR_Cas_Car_Price_Basic_Min_Age_Renter { get; set; }
        public Nullable<int> CR_Cas_Car_Price_Basic_Max_Age_Renter { get; set; }
        public Nullable<bool> CR_Cas_Car_Price_Basic_Additional_Driver { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Additional_Driver_Value { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Internal_Fees_Tamm { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_International_Fees_Tamm { get; set; }
        public Nullable<bool> CR_Cas_Car_Price_Basic_Does_Require_Financial_Credit { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Carrying_Accident { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Carrying_Fire { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Stealing { get; set; }
        public Nullable<decimal> CR_Cas_Car_Price_Basic_Drowning { get; set; }
        public Nullable<bool> CR_Cas_Car_Price_Basic_Activation { get; set; }
        public Nullable<System.DateTime> CR_Cas_Car_Price_Basic_About_To_Expire { get; set; }
        public string CR_Cas_Car_Price_Basic_Status { get; set; }

        public virtual CR_Mas_Sup_Category CR_Mas_Sup_Category { get; set; }
        public virtual CR_Mas_Sup_Sector CR_Mas_Sup_Sector { get; set; }



        //Car doc maintenance

        public string CR_Cas_Sup_Car_Doc_Mainten_Serail_No { get; set; }
        public string CR_Cas_Sup_Car_Doc_Mainten_Code { get; set; }
        public string CR_Cas_Sup_Car_Doc_Mainten_No { get; set; }
        public Nullable<System.DateTime> CR_Cas_Sup_Car_Doc_Mainten_Date { get; set; }
        public Nullable<System.DateTime> CR_Cas_Sup_Car_Doc_Mainten_Start_Date { get; set; }
        public Nullable<System.DateTime> CR_Cas_Sup_Car_Doc_Mainten_End_Date { get; set; }
        public Nullable<bool> CR_Cas_Sup_Car_Doc_Mainten_Activation { get; set; }
        public Nullable<int> CR_Cas_Sup_Car_Doc_Mainten_Limit_KM { get; set; }
        public Nullable<System.DateTime> CR_Cas_Sup_Car_Doc_Mainten_About_To_Expire { get; set; }
        public Nullable<int> CR_Cas_Sup_Car_Doc_Mainten_Default_KM { get; set; }
        public string CR_Cas_Sup_Car_Doc_Mainten_Status { get; set; }
        public string CR_Cas_Sup_Car_Doc_Mainten_Procedure_Type { get; set; }
        public string CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code { get; set; }
        public string CR_Cas_Sup_Car_Doc_Mainten_Branch_Code { get; set; }
        public string CR_Cas_Sup_Car_Doc_Mainten_Type { get; set; }
        public Nullable<int> CR_Cas_Sup_Car_Doc_Mainten_End_KM { get; set; }
        public virtual CR_Mas_Sup_Procedures CR_Mas_Sup_Procedures { get; set; }
    }
}