using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentCar.Models.ApiModel
{
    public class LoginAPI
    {
        public string UserName { get; set; }
        public string LessorCode { get; set; }
        public string BranchCode { get; set; }
        public string UserImage { get; set; }
        public string Logo { get; set; }
        public string CompanyName { get; set; }   
        public string BranchLogo { get; set; }
        public string BranchName { get; set; }
        public bool AuthBranch { get; set; }
        public bool AuthSystem { get; set; }


        ////////////////Validity contract/////////////////////////
        public bool ContractRegister { get; set; }
        public bool ChamberCommerce { get; set; }
        public bool TransferPermission { get; set; }
        public bool LicenceMunicipale { get; set; }
        public bool CompanyAddress { get; set; }
        public bool TrafficLicense { get; set; }
        public bool ContractInsurance { get; set; }
        public bool ContractOperatingCard { get; set; }
        public bool ContractChkecUp { get; set; }
        public bool ContractId { get; set; }
        public bool ContractDrivingLicense { get; set; }
        public bool ContractRenterAddress { get; set; }
        public bool ContractEmployer { get; set; }
        public bool ContractTires { get; set; }
        public bool ContractOil { get; set; }
        public bool ContractMaintenance { get; set; }
        public bool ContractFBrakeMaintenance { get; set; }
        public bool ContractBBrakeMaintenance { get; set; }
        public bool ContractExtension { get; set; }
        public bool ContractAge { get; set; }
        public decimal ContractOpenAmoutRate { get; set; }
        public decimal ContractDiscountRate { get; set; }
        public int ContractKm { get; set; }
        public int ContractHour { get; set; }
        public bool ContractCancel { get; set; }
        public bool ContractEnd { get; set; }
        public decimal ContractClose { get; set; }
        public decimal ContractCloseAmountRate { get; set; }

        //////////////////////////////////////////////////////////
        ///
        //////////////////////////Get Alerts//////////////////////
        public int CompanydocsN { get; set; }
        public int CompanydocsX { get; set; }
        public int CompanydocsE { get; set; }
        public int Companydocs { get; set; }
        public int CompanyContractsN { get; set; }
        public int CompanyContractsX { get; set; }
        public int CompanyContractsE { get; set; }
        public int CompanyContracts { get; set; }
        public int PricesN { get; set; }
        public int PricesX { get; set; }
        public int PricesE { get; set; }
        public int Prices { get; set; }
        public int CarsDocN { get; set; }
        public int CarsDocX { get; set; }
        public int CarsDocE { get; set; }
        public int CarsDoc { get; set; }

        public int CarsMaintenanceN { get; set; }
        public int CarsMaintenanceE { get; set; }
        public int CarsMaintenanceX { get; set; }
        public int CarsMaintenance { get; set; }
        public int nbr { get; set; }

        //////////////////////////////////////////////////////////



    }
}