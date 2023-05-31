using RentCar.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using RentCar.Models.ApiModel;
using System.Web;

namespace RentCar.Models.CAS
{
    public class LoadAlerts
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        public void GetExpiredDocs(string LessorCode)
        {
            int nbr = 0;
            var CompanydocsN = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
            && d.CR_Cas_Sup_Branch_Documentation_Status == "N").Count() != 0 ? db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
            && d.CR_Cas_Sup_Branch_Documentation_Status == "N").Count() : 0;

            //Session.Remove("CompanyDocsN");
            HttpContext.Current.Session["CompanyDocsN"] = CompanydocsN;


            var CompanydocsX = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
            && d.CR_Cas_Sup_Branch_Documentation_Status == "X").Count() != 0 ? db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
            && d.CR_Cas_Sup_Branch_Documentation_Status == "X").Count() : 0;

            {
                //Session.Remove("CompanydocsX");
                HttpContext.Current.Session["CompanydocsX"] = CompanydocsX;


                var CompanydocsE = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
                && d.CR_Cas_Sup_Branch_Documentation_Status == "E").Count() != 0 ? db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
                && d.CR_Cas_Sup_Branch_Documentation_Status == "E").Count() : 0;



                //Session.Remove("CompanydocsE");
                HttpContext.Current.Session["CompanydocsE"] = CompanydocsE;


                var Companydocs = CompanydocsE + CompanydocsN + CompanydocsX;




                var CompanyContractsN = db.CR_Cas_Company_Contract.Where(d => d.CR_Cas_Company_Contract_Lessor == LessorCode
                && d.CR_Cas_Company_Contract_Status == "N").Count() != 0 ? db.CR_Cas_Company_Contract.Where(d => d.CR_Cas_Company_Contract_Lessor == LessorCode
                && d.CR_Cas_Company_Contract_Status == "N").Count() : 0;

                //Session.Remove("CompanyContractsN");
                HttpContext.Current.Session["CompanyContractsN"] = CompanyContractsN;


                var CompanyContractsX = db.CR_Cas_Company_Contract.Where(d => d.CR_Cas_Company_Contract_Lessor == LessorCode && d.CR_Cas_Company_Contract_Status == "X").Count() != 0 ? db.CR_Cas_Company_Contract.Where(d => d.CR_Cas_Company_Contract_Lessor == LessorCode && d.CR_Cas_Company_Contract_Status == "X").Count() : 0;

                HttpContext.Current.Session["CompanyContractsX"] = CompanyContractsX;


                var CompanyContractsE = db.CR_Cas_Company_Contract.Where(d => d.CR_Cas_Company_Contract_Lessor == LessorCode && d.CR_Cas_Company_Contract_Status == "E").Count() != 0 ? db.CR_Cas_Company_Contract.Where(d => d.CR_Cas_Company_Contract_Lessor == LessorCode && d.CR_Cas_Company_Contract_Status == "E").Count() : 0;

                HttpContext.Current.Session["CompanyContractsE"] = CompanyContractsE;



                var CompanyContracts = CompanyContractsN + CompanyContractsX + CompanyContractsE;




                //var PricesN = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
                //&& d.CR_Cas_Sup_Branch_Documentation_Status == "N").Count();
                //Session["PricesN"] = PricesN;

                var PricesX = db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && d.CR_Cas_Car_Price_Basic_Status == "X").Count() != 0 ? db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && d.CR_Cas_Car_Price_Basic_Status == "X").Count() : 0;

                HttpContext.Current.Session["PricesX"] = PricesX;


                var PricesE = db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && d.CR_Cas_Car_Price_Basic_Status == "E").Count() != 0 ? db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && d.CR_Cas_Car_Price_Basic_Status == "E").Count() : 0;

                HttpContext.Current.Session["PricesE"] = PricesE;


                var Prices = PricesX + PricesE;




                var CarsDocN = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
                && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "N").Count() != 0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
                && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "N").Count() : 0;

                HttpContext.Current.Session["CarsDocN"] = CarsDocN;


                var CarsDocX = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
                && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "X").Count() != 0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
                && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "X").Count() : 0;

                HttpContext.Current.Session["CarsDocX"] = CarsDocX;


                var CarsDocE = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3" && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E").Count() != 0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3" && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E").Count() : 0;

                HttpContext.Current.Session["CarsDocE"] = CarsDocE;


                var CarsDoc = CarsDocN + CarsDocX + CarsDocE;





                var CarsMaintenanceN = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
                && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "N").Count() != 0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
                && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "N").Count() : 0;

                HttpContext.Current.Session["CarsMaintenanceN"] = CarsMaintenanceN;


                var CarsMaintenanceE = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4" && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E").Count() != 0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4" && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E").Count() : 0;

                HttpContext.Current.Session["CarsMaintenanceE"] = CarsMaintenanceE;


                var CarsMaintenanceX = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4" && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E").Count() != 0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4" && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E").Count() : 0;

                HttpContext.Current.Session["CarsMaintenanceX"] = CarsMaintenanceX;


                var CarsMaintenance = CarsMaintenanceN + CarsMaintenanceX + CarsMaintenanceE;


                nbr = nbr + Companydocs + CompanyContracts + Prices + CarsDoc + CarsMaintenance;
                HttpContext.Current.Session["AlertsNbr"] = nbr;

            }
        }
    }
}