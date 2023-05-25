using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentCar.Models.CAS
{
    public class CarDocsModel
    {
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
        public bool PassCar { get; set; }
        public string TrafficLicenseError { get; set; }
        public string ContractInsuranceError { get; set; }
        public string ContractOperatingCardError { get; set; }
        public string ContractChkecUpError { get; set; }
        public string ContractTires { get; set; }
        public string ContractOil { get; set; }
        public string ContractMaintenance { get; set; }
        public string ContractFBrakeMaintenance { get; set; }
        public string ContractBBrakeMaintenance { get; set; }
        public virtual CR_Mas_Sup_Procedures CR_Mas_Sup_Procedures { get; set; }
        public virtual CR_Cas_Sup_Car_Information CR_Cas_Sup_Car_Information { get; set; }
    }
}