using Microsoft.Ajax.Utilities;
using RentCar.Models;
using RentCar.Models.BranchSys;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Web.Mvc;

namespace RentCar.Controllers.BranchSys
{
    public class BranchHomeController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();
        // GET: BranchHome
        public ActionResult BranchHome()
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null|| Session["BranchName"]==null)
                {
                    RedirectToAction("Login", "Account");
                }
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();
                var BranchName = Session["BranchName"].ToString();
                //UserLogin = Session["UserLogin"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            List<CR_Cas_Sup_Branch> branchs = new List<CR_Cas_Sup_Branch>();
            var branchlist = db.CR_Cas_User_Branch_Validity.Where(v => v.CR_Cas_User_Branch_Validity_Id == UserLogin);
            foreach (var item in branchlist)
            {
                CR_Cas_Sup_Branch br = new CR_Cas_Sup_Branch();
                var n = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == item.CR_Cas_User_Branch_Validity_Branch &&
                b.CR_Cas_Sup_Lessor_Code==LessorCode && b.CR_Cas_Sup_Branch_Status!="D");
                if (n != null)
                {
                    br.CR_Cas_Sup_Branch_Code = item.CR_Cas_User_Branch_Validity_Branch;
                    br.CR_Cas_Sup_Branch_Ar_Name = n.CR_Cas_Sup_Branch_Ar_Name;
                    br.CR_Cas_Sup_Branch_Ar_Short_Name = n.CR_Cas_Sup_Branch_Ar_Short_Name;
                    branchs.Add(br);
                }
               
            }
            GetValidityContract(UserLogin,BranchCode,LessorCode);
            GetBranchExpiredDocs(LessorCode, BranchCode);
            ViewBag.BranchList = new SelectList(branchs, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Short_Name", BranchCode);
            return View();
        }
        public JsonResult GetCashDrawerNotification(string LessorCode, string Login)
        {
            var procedure = db.CR_Cas_Administrative_Procedures.Where(a=>a.CR_Cas_Administrative_Procedures_Code=="62" && a.CR_Cas_Administrative_Procedures_Lessor==LessorCode 
            && a.CR_Cas_Administrative_Procedures_Targeted_Action==Login && a.CR_Cas_Administrative_Procedures_Action==false && a.CR_Cas_Administrative_Procedures_Type=="I").Count();
            return Json(procedure, JsonRequestBehavior.AllowGet);
        }
                                                                         
        public JsonResult CheckCashClosing(string LessorCode, string Login,string BranchCode)
        {
            var receipt = db.CR_Cas_Account_Receipt.Where(r=>r.CR_Cas_Account_Receipt_User_Code==Login && r.CR_Cas_Account_Receipt_Branch_Code==BranchCode
            && r.CR_Cas_Account_Receipt_Lessor_Code==LessorCode && r.CR_Cas_Account_Receipt_Is_Passing=="1").Count();
            return Json(receipt, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckRenterLessor(string LessorCode)
        {
            var cR_Cas_Renter_Lessor = db.CR_Cas_Renter_Lessor.Where(r => r.CR_Cas_Renter_Lessor_Code == LessorCode).Count();

            return Json(cR_Cas_Renter_Lessor, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckReceiptList(string LessorCode, string Login, string BranchCode)
        {
            var receipt = db.CR_Cas_Account_Receipt.Where(r => r.CR_Cas_Account_Receipt_User_Code == Login && r.CR_Cas_Account_Receipt_Branch_Code == BranchCode
            && r.CR_Cas_Account_Receipt_Lessor_Code == LessorCode).Count();
            return Json(receipt, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckRentedCars(string LessorCode, string BranchCode)
        {
            var Cars = db.CR_Cas_Sup_Car_Information.Where(c=>c.CR_Cas_Sup_Car_Status=="R" && c.CR_Cas_Sup_Car_Lessor_Code==LessorCode && c.CR_Cas_Sup_Car_Owner_Branch_Code==BranchCode).Count();
            return Json(Cars, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckActiveCars(string LessorCode, string BranchCode)
        {
            var CarsActive = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Status == "A" && c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                  c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode && c.CR_Cas_Sup_Car_Documentation_Status == true && c.CR_Cas_Sup_Car_Price_Status == "1"
                  && c.CR_Cas_Sup_Car_Maintenance_Status == true).Count();
            return Json(CarsActive, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckMaintCars(string LessorCode, string BranchCode)
        {
            var CarsMain = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode && c.CR_Cas_Sup_Car_Status == "A"
                && (c.CR_Cas_Sup_Car_Price_Status != "1" || c.CR_Cas_Sup_Car_Maintenance_Status == false || c.CR_Cas_Sup_Car_Documentation_Status == false)).Count();
            return Json(CarsMain, JsonRequestBehavior.AllowGet);
        }
        private void GetValidityContract(string Login, string BranchCode, string LessorCode)
        {
            var validity = db.CR_Cas_User_Validity_Contract.FirstOrDefault(v => v.CR_Cas_User_Validity_Contract_User_Id == Login);
            if (validity != null)
            {
                if ((bool)(validity.CR_Cas_User_Validity_Contract_Register == true))
                {
                    Session["ContractRegister"] = "True";
                }
                else
                {
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "10" && d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_Status == "A" || doc.CR_Cas_Sup_Branch_Documentation_Status == "X")
                        {
                            Session["ContractRegister"] = "True";
                        }
                        else
                        {
                            Session["ContractRegister"] = "False";
                        }
                    }
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Chamber_Commerce == true))
                {
                    Session["ChamberCommerce"] = "True";
                }
                else
                {
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "11" && d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_Status == "A" || doc.CR_Cas_Sup_Branch_Documentation_Status == "X")
                        {
                            Session["ChamberCommerce"] = "True";
                        }
                        else
                        {
                            Session["ChamberCommerce"] = "False";
                        }
                    }
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Transfer_Permission == true))
                {
                    Session["TransferPermission"] = "True";
                }
                else
                {
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "12" && d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_Status == "A" || doc.CR_Cas_Sup_Branch_Documentation_Status == "X")
                        {
                            Session["TransferPermission"] = "True";
                        }
                        else
                        {
                            Session["TransferPermission"] = "False";
                        }
                    }
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Licence_Municipale == true))
                {
                    Session["LicenceMunicipale"] = "True";
                }
                else
                {
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "13" && d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_Status == "A" || doc.CR_Cas_Sup_Branch_Documentation_Status == "X")
                        {
                            Session["LicenceMunicipale"] = "True";
                        }
                        else
                        {
                            Session["LicenceMunicipale"] = "False";
                        }
                    }
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Company_Address == true))
                {
                    Session["CompanyAddress"] = "True";
                }
                else
                {
                    var doc = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d => d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode
                    && d.CR_Cas_Sup_Branch_Documentation_Code == "14" && d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode);
                    if (doc != null)
                    {
                        if (doc.CR_Cas_Sup_Branch_Documentation_Status == "A" || doc.CR_Cas_Sup_Branch_Documentation_Status == "X")
                        {
                            Session["CompanyAddress"] = "True";
                        }
                        else
                        {
                            Session["CompanyAddress"] = "False";
                        }
                    }
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Traffic_License == true))
                {
                    Session["TrafficLicense"] = "True";
                }
                else
                {
                    Session["TrafficLicense"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Insurance == true))
                {
                    Session["ContractInsurance"] = "True";
                }
                else
                {
                    Session["ContractInsurance"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Operating_Card == true))
                {
                    Session["ContractOperatingCard"] = "True";
                }
                else
                {
                    Session["ContractOperatingCard"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Chkec_Up == true))
                {
                    Session["ContractChkecUp"] = "True";
                }
                else
                {
                    Session["ContractChkecUp"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Id == true))
                {
                    Session["ContractId"] = "True";
                }
                else
                {
                    Session["ContractId"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Driving_License == true))
                {
                    Session["ContractDrivingLicense"] = "True";
                }
                else
                {
                    Session["ContractDrivingLicense"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Renter_Address == true))
                {
                    Session["ContractRenterAddress"] = "True";
                }
                else
                {
                    Session["ContractRenterAddress"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Employer == true))
                {
                    Session["ContractEmployer"] = "True";
                }
                else
                {
                    Session["ContractEmployer"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Tires == true))
                {
                    Session["ContractTires"] = "True";
                }
                else
                {
                    Session["ContractTires"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Oil == true))
                {
                    Session["ContractOil"] = "True";
                }
                else
                {
                    Session["ContractOil"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Maintenance == true))
                {
                    Session["ContractMaintenance"] = "True";
                }
                else
                {
                    Session["ContractMaintenance"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_FBrake_Maintenance == true))
                {
                    Session["ContractFBrakeMaintenance"] = "True";
                }
                else
                {
                    Session["ContractFBrakeMaintenance"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_BBrake_Maintenance == true))
                {
                    Session["ContractBBrakeMaintenance"] = "True";
                }
                else
                {
                    Session["ContractBBrakeMaintenance"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Extension == true))
                {
                    Session["ContractExtension"] = "True";
                }
                else
                {
                    Session["ContractExtension"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Age == true))
                {
                    Session["ContractAge"] = "True";
                }
                else
                {
                    Session["ContractAge"] = "False";
                }

                Session["ContractOpenAmoutRate"] = validity.CR_Cas_User_Validity_Contract_Open_Amout_Rate;
                Session["ContractDiscountRate"] = validity.CR_Cas_User_Validity_Contract_Discount_Rate;
                Session["ContractKm"] = validity.CR_Cas_User_Validity_Contract_Km;
                Session["ContractHour"] = validity.CR_Cas_User_Validity_Contract_Hour;

                if ((bool)(validity.CR_Cas_User_Validity_Contract_Cancel == true))
                {
                    Session["ContractCancel"] = "True";
                }
                else
                {
                    Session["ContractCancel"] = "False";
                }

                if ((bool)(validity.CR_Cas_User_Validity_Contract_End == true))
                {
                    Session["ContractEnd"] = "True";
                }
                else
                {
                    Session["ContractEnd"] = "False";
                }

                Session["ContractClose"] = validity.CR_Cas_User_Validity_Contract_Close;
                Session["ContractCloseAmountRate"] = validity.CR_Cas_User_Validity_Contract_Close_Amount_Rate;
            }
            else
            {
                Session["ContractRegister"] = "False";
                Session["ChamberCommerce"] = "False";
                Session["TransferPermission"] = "False";
                Session["LicenceMunicipale"] = "False";
                Session["CompanyAddress"] = "False";
                Session["TrafficLicense"] = "False";
                Session["ContractInsurance"] = "False";
                Session["ContractOperatingCard"] = "False";
                Session["ContractChkecUp"] = "False";
                Session["ContractId"] = "False";
                Session["ContractDrivingLicense"] = "False";
                Session["ContractRenterAddress"] = "False";
                Session["ContractEmployer"] = "False";
                Session["ContractTires"] = "False";
                Session["ContractOil"] = "False";
                Session["ContractMaintenance"] = "False";
                Session["ContractFBrakeMaintenance"] = "False";
                Session["ContractBBrakeMaintenance"] = "False";
                Session["ContractExtension"] = "False";
                Session["ContractAge"] = "False";
                Session["ContractOpenAmoutRate"] = "0";
                Session["ContractDiscountRate"] = "0";
                Session["ContractKm"] = "0";
                Session["ContractHour"] = "0";
                Session["ContractCancel"] = "False";
                Session["ContractEnd"] = "False";
                Session["ContractClose"] = "0";
                Session["ContractCloseAmountRate"] = "0";
            }

        }
        public JsonResult ChangeBranch(string BranchCode)
        {
            var LessorCode = "";
            var UserLogin = "";
            try
            {
                if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null)
                {
                    RedirectToAction("Login", "Account");
                }
                LessorCode = Session["LessorCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }

            Session.Remove("BranchCode");
            Session["BranchCode"] = BranchCode;
            var str = "";
            var branch = db.CR_Cas_Sup_Branch.FirstOrDefault(br => br.CR_Cas_Sup_Lessor_Code == LessorCode && br.CR_Cas_Sup_Branch_Code == BranchCode);
            if (branch != null)
            {
                var brnChk = "<span style=\"color: #ff5b5b \"> (الفرع موقوف)</span>";
                var brnName = "<span>"+branch.CR_Cas_Sup_Branch_Ar_Name+ "</span>" ;
                if (branch.CR_Cas_Sup_Branch_Status=="H")
                {
                    
                    Session["BranchName"] = brnName + brnChk;
                }
                else
                {
                    Session["BranchName"] = branch.CR_Cas_Sup_Branch_Ar_Name;
                }
                Session["BranchLogo"] = branch.CR_Cas_Sup_Branch_LogoMap;
                str = branch.CR_Cas_Sup_Branch_Ar_Name;
            }
            
            
            //////////str = branch.CR_Cas_Sup_Branch_LogoMap.Substring(1, branch.CR_Cas_Sup_Branch_LogoMap.Length - 1) + "-" + branch.CR_Cas_Sup_Branch_Ar_Name;
           
            var User = db.CR_Cas_User_Information.FirstOrDefault(b=>b.CR_Cas_User_Information_Id==UserLogin);
            User.CR_Cas_User_Information_Branch_Code = BranchCode;
            GetValidityContract(UserLogin, BranchCode, LessorCode);
           
            GetBranchExpiredDocs(LessorCode, BranchCode);
            db.Entry(User).State = EntityState.Modified;
            db.SaveChanges();
            return Json(str, JsonRequestBehavior.AllowGet);

        }

        public JsonResult CheckCarsPriceStatus(string LessorCode,string BranchCode)
        {
            var cars = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode && c.CR_Cas_Sup_Car_Owner_Branch_Code == BranchCode
              && c.CR_Cas_Sup_Car_Status == "A" && c.CR_Cas_Sup_Car_Price_Status == "1").Count();
            return Json(cars, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckBranch(string LessorCode, string BranchCode)
        {
            var branch = db.CR_Cas_Sup_Branch.Where(b=>b.CR_Cas_Sup_Lessor_Code==LessorCode&& b.CR_Cas_Sup_Branch_Code==BranchCode && b.CR_Cas_Sup_Branch_Status=="A").Count();
            return Json(branch, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckLessor(string LessorCode)
        {
            var Lessor = db.CR_Mas_Com_Lessor.Where(b => b.CR_Mas_Com_Lessor_Code == LessorCode && b.CR_Mas_Com_Lessor_Status == "A").Count();
            return Json(Lessor, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckRenewContract(string LessorCode, string BranchCode)
        {
            var Contract = db.CR_Cas_Contract_Basic.Where(c=>c.CR_Cas_Contract_Basic_Lessor==LessorCode && c.CR_Cas_Contract_Basic_Owner_Branch==BranchCode && c.CR_Cas_Contract_Basic_Status=="A").Count();
            return Json(Contract, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckContractSettlement(string LessorCode, string BranchCode)
        {
            //var Contract = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Owner_Branch == BranchCode 
            //&& (c.CR_Cas_Contract_Basic_Status == "A"|| c.CR_Cas_Contract_Basic_Status=="E" || c.CR_Cas_Contract_Basic_is_Receiving_Branch == true)).Count();
            var Contract = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode
            && (c.CR_Cas_Contract_Basic_Status == "A" || c.CR_Cas_Contract_Basic_Status == "E")
            && (c.CR_Cas_Contract_Basic_Owner_Branch == BranchCode || c.CR_Cas_Contract_Basic_is_Receiving_Branch == true)).Count();


            return Json(Contract, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult Carousel()
        {
            IQueryable<CR_Cas_Sup_Car_Information> Cars;
            List<CarInfoPrice> lPrice = new List<CarInfoPrice>();
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null)
                {
                    RedirectToAction("Login", "Account");
                }
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            var Carsinfo = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Status == "A" && c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                  c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode && c.CR_Cas_Sup_Car_Documentation_Status==true && c.CR_Cas_Sup_Car_Price_Status=="1"
                  && c.CR_Cas_Sup_Car_Maintenance_Status==true);
            Cars = Carsinfo;
            foreach (var Car in Cars)
            {
                CarInfoPrice p = new CarInfoPrice();
                var CarPrice = db.CR_Cas_Car_Price_Basic.FirstOrDefault(price => price.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode &&
                 price.CR_Cas_Car_Price_Basic_Model_Code == Car.CR_Cas_Sup_Car_Model_Code && price.CR_Cas_Car_Price_Basic_Car_Year == Car.CR_Cas_Sup_Car_Year
                 && (price.CR_Cas_Car_Price_Basic_Status=="A" || price.CR_Cas_Car_Price_Basic_Status=="X"));
                if (CarPrice != null)
                {
                    ////////////////////////////////////
                    p.CR_Cas_Sup_Car_Lessor_Code = Car.CR_Cas_Sup_Car_Lessor_Code;
                    p.CR_Cas_Sup_Car_Model_Code = Car.CR_Cas_Sup_Car_Model_Code;
                    p.CR_Cas_Sup_Car_Year = Car.CR_Cas_Sup_Car_Year;
                    p.CR_Cas_Sup_Car_Serail_No = Car.CR_Cas_Sup_Car_Serail_No;
                    //////////////////////////////////////////////////////////
                    p.CR_Mas_Sup_Category_Car = db.CR_Mas_Sup_Category_Car.FirstOrDefault(c => c.CR_Mas_Sup_Category_Car_Code == Car.CR_Cas_Sup_Car_Category_Code 
                    && c.CR_Mas_Sup_Category_Car_Year==Car.CR_Cas_Sup_Car_Year && c.CR_Mas_Sup_Category_Model_Code==Car.CR_Cas_Sup_Car_Model_Code);
                    if (p.CR_Mas_Sup_Category_Car != null)
                    {
                        p.CR_Mas_Sup_Color = Car.CR_Mas_Sup_Color;
                        p.CR_Mas_Sup_Model = Car.CR_Mas_Sup_Model;

                        p.CR_Cas_Sup_Car_Owner_Branch_Code = Car.CR_Cas_Sup_Car_Owner_Branch_Code;
                        p.CR_Cas_Sup_Car_Location_Branch_Code = Car.CR_Cas_Sup_Car_Location_Branch_Code;
                        p.CR_Cas_Sup_Car_Owner_Code = Car.CR_Cas_Sup_Car_Owner_Code;
                        p.CR_Cas_Sup_Car_Beneficiary_Code = Car.CR_Cas_Sup_Car_Beneficiary_Code;
                        p.CR_Cas_Sup_Car_Customs_No = Car.CR_Cas_Sup_Car_Customs_No;
                        p.CR_Cas_Sup_Car_Structure_No = Car.CR_Cas_Sup_Car_Structure_No;
                        p.CR_Cas_Sup_Car_Registration_Code = Car.CR_Cas_Sup_Car_Registration_Code;
                        p.CR_Cas_Sup_Car_Brand_Code = Car.CR_Cas_Sup_Car_Brand_Code;
                        p.CR_Cas_Sup_Car_Category_Code = Car.CR_Cas_Sup_Car_Category_Code;
                        p.CR_Cas_Sup_Car_Out_Main_Color_Code = Car.CR_Cas_Sup_Car_Out_Main_Color_Code;
                        p.CR_Cas_Sup_Car_Out_Secondary_Color_Code = Car.CR_Cas_Sup_Car_Out_Secondary_Color_Code;
                        p.CR_Cas_Sup_Car_In_Main_Color_Code = Car.CR_Cas_Sup_Car_In_Main_Color_Code;
                        p.CR_Cas_Sup_Car_In_Secondary_Color_Code = Car.CR_Cas_Sup_Car_In_Secondary_Color_Code;
                        p.CR_Cas_Sup_Car_Plate_Ar_No = Car.CR_Cas_Sup_Car_Plate_Ar_No;
                        p.CR_Cas_Sup_Car_Plate_En_No = Car.CR_Cas_Sup_Car_Plate_En_No;
                        p.CR_Cas_Sup_Car_Collect_Ar_Name = Car.CR_Cas_Sup_Car_Collect_Ar_Name;
                        p.CR_Cas_Sup_Car_Collect_Ar_Short_Name = Car.CR_Cas_Sup_Car_Collect_Ar_Short_Name;
                        //p.CR_Cas_Sup_Car_Collect_En_Name = Car.CR_Cas_Sup_Car_Collect_En_Name;
                        p.CR_Cas_Sup_Car_Collect_En_Short_Name = Car.CR_Cas_Sup_Car_Collect_En_Short_Name;
                        // p.CR_Cas_Sup_Car_Collect_Fr_Name = Car.CR_Cas_Sup_Car_Collect_Fr_Name;
                        p.CR_Cas_Sup_Car_Collect_Fr_Short_Name = Car.CR_Cas_Sup_Car_Collect_Fr_Short_Name;
                        p.CR_Cas_Sup_Car_No_Current_Meter = Car.CR_Cas_Sup_Car_No_Current_Meter;
                        p.CR_Cas_Sup_Car_Traffic_License_Img = Car.CR_Cas_Sup_Car_Traffic_License_Img;
                        p.CR_Cas_Sup_Car_Joined_Fleet_Date = Car.CR_Cas_Sup_Car_Joined_Fleet_Date;
                        //p.CR_Cas_Sup_Car_Left_Fleet_Date = Car.CR_Cas_Sup_Car_Left_Fleet_Date;
                        //p.CR_Cas_Car_Price_Basic_Min_Age_Renter = CarPrice.CR_Cas_Car_Price_Basic_Min_Age_Renter;
                        p.CR_Cas_Car_Price_Basic_Additional_KM_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_KM_Value;
                        p.CR_Cas_Car_Price_Basic_No_Daily_Free_KM = CarPrice.CR_Cas_Car_Price_Basic_No_Daily_Free_KM;
                        p.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours = CarPrice.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours;
                        p.CR_Cas_Car_Price_Basic_Extra_Hour_Value = CarPrice.CR_Cas_Car_Price_Basic_Extra_Hour_Value;
                        if (Car.CR_Cas_Sup_Car_Last_Pictures == null || Car.CR_Cas_Sup_Car_Last_Pictures == "")
                        {
                            p.CR_Cas_Sup_Car_Last_Pictures = "~/images/img.png";
                        }
                        else
                        {
                            p.CR_Cas_Sup_Car_Last_Pictures = Car.CR_Cas_Sup_Car_Last_Pictures;
                        }

                        p.CR_Cas_Sup_Car_Documentation_Status = Car.CR_Cas_Sup_Car_Documentation_Status;
                        p.CR_Cas_Sup_Car_Maintenance_Status = Car.CR_Cas_Sup_Car_Maintenance_Status;
                        p.CR_Cas_Car_Price_Basic_Status = CarPrice.CR_Cas_Car_Price_Basic_Status;
                        p.CR_Cas_Sup_Car_Status = Car.CR_Cas_Sup_Car_Status;
                        p.CR_Cas_Sup_Car_Reasons = Car.CR_Cas_Sup_Car_Reasons;



                        p.CR_Cas_Car_Price_Basic_No = CarPrice.CR_Cas_Car_Price_Basic_No;
                        var featuresSum = db.CR_Cas_Car_Price_Features.Where(f => f.CR_Cas_Car_Price_Features_No == p.CR_Cas_Car_Price_Basic_No).Sum(f => f.CR_Cas_Car_Price_Features_Value);
                        if (featuresSum == null)
                        {
                            featuresSum = 0;
                        }

                        p.CR_Cas_Car_Price_Basic_Daily_Rent = CarPrice.CR_Cas_Car_Price_Basic_Daily_Rent + featuresSum;
                        p.CR_Cas_Car_Price_Basic_Weekly_Rent = CarPrice.CR_Cas_Car_Price_Basic_Weekly_Rent + featuresSum;
                        p.CR_Cas_Car_Price_Basic_Monthly_Rent = CarPrice.CR_Cas_Car_Price_Basic_Monthly_Rent + featuresSum;
                        p.CR_Cas_Car_Price_Basic_Rental_Tax_Rate = CarPrice.CR_Cas_Car_Price_Basic_Rental_Tax_Rate;
                        p.CR_Cas_Car_Price_Basic_No_Daily_Free_KM = CarPrice.CR_Cas_Car_Price_Basic_No_Daily_Free_KM;
                        p.CR_Cas_Car_Price_Basic_Additional_KM_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_KM_Value;
                        p.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours = CarPrice.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours;
                        p.CR_Cas_Car_Price_Basic_Hour_Max = CarPrice.CR_Cas_Car_Price_Basic_Hour_Max;
                        p.CR_Cas_Car_Price_Basic_Extra_Hour_Value = CarPrice.CR_Cas_Car_Price_Basic_Extra_Hour_Value;
                        p.CR_Cas_Car_Price_Basic_Min_Age_Renter = CarPrice.CR_Cas_Car_Price_Basic_Min_Age_Renter;
                        p.CR_Cas_Car_Price_Basic_Max_Age_Renter = CarPrice.CR_Cas_Car_Price_Basic_Max_Age_Renter;
                        p.CR_Cas_Car_Price_Basic_Additional_Driver = CarPrice.CR_Cas_Car_Price_Basic_Additional_Driver;
                        p.CR_Cas_Car_Price_Basic_Additional_Driver_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_Driver_Value;

                        p.CR_Cas_Car_Price_Basic_Internal_Fees_Tamm = CarPrice.CR_Cas_Car_Price_Basic_Internal_Fees_Tamm;
                        p.CR_Cas_Car_Price_Basic_International_Fees_Tamm = CarPrice.CR_Cas_Car_Price_Basic_International_Fees_Tamm;
                        p.CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value = CarPrice.CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value;
                        p.CR_Cas_Car_Price_Basic_Carrying_Accident = CarPrice.CR_Cas_Car_Price_Basic_Carrying_Accident;
                        p.CR_Cas_Car_Price_Basic_Carrying_Fire = CarPrice.CR_Cas_Car_Price_Basic_Carrying_Fire;
                        p.CR_Cas_Car_Price_Basic_Stealing = CarPrice.CR_Cas_Car_Price_Basic_Stealing;
                        p.CR_Cas_Car_Price_Basic_Drowning = CarPrice.CR_Cas_Car_Price_Basic_Drowning;
                        p.CR_Cas_Sup_Car_Model_Code = CarPrice.CR_Cas_Car_Price_Basic_Model_Code;
                        p.CR_Cas_Sup_Car_Year = CarPrice.CR_Cas_Car_Price_Basic_Car_Year;

                       
                        if (Car.CR_Cas_Sup_Car_Documentation_Status != true)
                        {
                            p.CarsDocError = "التأكد من الوثائق";
                           // p.PassCarDoc = "False";
                        }
                        else
                        {
                            p.CarsDocError = "";
                        }
                        if (Car.CR_Cas_Sup_Car_Maintenance_Status != true)
                        {
                            p.CarsMainError = "التأكد من الصيانة";
                            //p.PassCarMain = "False";
                        }
                        else
                        {
                            p.CarsMainError = "";
                        }


                        lPrice.Add(p);
                    }

                }
            }

            List<CR_Cas_Sup_Branch> branchs = new List<CR_Cas_Sup_Branch>();
            var branchlist = db.CR_Cas_User_Branch_Validity.Where(v => v.CR_Cas_User_Branch_Validity_Id == UserLogin);
            foreach (var item in branchlist)
            {
                CR_Cas_Sup_Branch br = new CR_Cas_Sup_Branch();
                //var n = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == item.CR_Cas_User_Branch_Validity_Branch &&
                //b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Status == "A");

                /////////////////////////////////////////// New Code /////////////////////////////////////////////
                ///
                /////////////////////// when the branch is hold should all cars be hold //////////////////////////////////////////////

                var n = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == item.CR_Cas_User_Branch_Validity_Branch &&
                b.CR_Cas_Sup_Lessor_Code == LessorCode);

                if (n != null)
                {
                    br.CR_Cas_Sup_Branch_Code = item.CR_Cas_User_Branch_Validity_Branch;
                    br.CR_Cas_Sup_Branch_Ar_Name = n.CR_Cas_Sup_Branch_Ar_Name;
                    branchs.Add(br);
                }

            }
            ViewBag.BranchList = new SelectList(branchs, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", BranchCode);
            return PartialView(lPrice);
        }


        public PartialViewResult CarouselRented()
        {
            IQueryable<CR_Cas_Sup_Car_Information> Cars;
            List<CarInfoPrice> lPrice = new List<CarInfoPrice>();
            var LessorCode = "";

            var UserLogin = "";
            var BranchCode = "";
            try
            {
                if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null)
                {
                    RedirectToAction("Login", "Account");
                }
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            var Carsinfo = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Status == "R" && c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                  c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode );
            Cars = Carsinfo;
            foreach (var Car in Cars)
            {
                CarInfoPrice p = new CarInfoPrice();
                var CarPrice = db.CR_Cas_Car_Price_Basic.FirstOrDefault(price => price.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode &&
                 price.CR_Cas_Car_Price_Basic_Model_Code == Car.CR_Cas_Sup_Car_Model_Code && price.CR_Cas_Car_Price_Basic_Car_Year == Car.CR_Cas_Sup_Car_Year);
                if (CarPrice != null)
                {
                    var contract = db.CR_Cas_Contract_Basic.FirstOrDefault(c=>c.CR_Cas_Contract_Basic_Car_Serail_No==Car.CR_Cas_Sup_Car_Serail_No);
                    if (contract != null)
                    {
                        p.CR_Cas_Contract_Basic = contract;
                        var RenterInfo = db.CR_Cas_Renter_Lessor.FirstOrDefault(r => r.CR_Cas_Renter_Lessor_Id == contract.CR_Cas_Contract_Basic_Renter_Id);
                        p.CR_Cas_Renter_Lessor = RenterInfo;
                    }
                    
                    ////////////////////////////////////
                    p.CR_Cas_Sup_Car_Lessor_Code = Car.CR_Cas_Sup_Car_Lessor_Code;
                    p.CR_Cas_Sup_Car_Model_Code = Car.CR_Cas_Sup_Car_Model_Code;
                    p.CR_Cas_Sup_Car_Year = Car.CR_Cas_Sup_Car_Year;
                    p.CR_Cas_Sup_Car_Serail_No = Car.CR_Cas_Sup_Car_Serail_No;
                    //////////////////////////////////////////////////////////
                    p.CR_Mas_Sup_Category_Car = db.CR_Mas_Sup_Category_Car.FirstOrDefault(c => c.CR_Mas_Sup_Category_Car_Code == Car.CR_Cas_Sup_Car_Category_Code &&
                    c.CR_Mas_Sup_Category_Car_Year == Car.CR_Cas_Sup_Car_Year && c.CR_Mas_Sup_Category_Model_Code == Car.CR_Cas_Sup_Car_Model_Code);
                    if (p.CR_Mas_Sup_Category_Car != null)
                    {
                        
                        

                        p.CR_Mas_Sup_Color = Car.CR_Mas_Sup_Color;
                        p.CR_Mas_Sup_Model = Car.CR_Mas_Sup_Model;

                        p.CR_Cas_Sup_Car_Owner_Branch_Code = Car.CR_Cas_Sup_Car_Owner_Branch_Code;
                        p.CR_Cas_Sup_Car_Location_Branch_Code = Car.CR_Cas_Sup_Car_Location_Branch_Code;
                        p.CR_Cas_Sup_Car_Owner_Code = Car.CR_Cas_Sup_Car_Owner_Code;
                        p.CR_Cas_Sup_Car_Beneficiary_Code = Car.CR_Cas_Sup_Car_Beneficiary_Code;
                        p.CR_Cas_Sup_Car_Customs_No = Car.CR_Cas_Sup_Car_Customs_No;
                        p.CR_Cas_Sup_Car_Structure_No = Car.CR_Cas_Sup_Car_Structure_No;
                        p.CR_Cas_Sup_Car_Registration_Code = Car.CR_Cas_Sup_Car_Registration_Code;
                        p.CR_Cas_Sup_Car_Brand_Code = Car.CR_Cas_Sup_Car_Brand_Code;
                        p.CR_Cas_Sup_Car_Category_Code = Car.CR_Cas_Sup_Car_Category_Code;
                        p.CR_Cas_Sup_Car_Out_Main_Color_Code = Car.CR_Cas_Sup_Car_Out_Main_Color_Code;
                        p.CR_Cas_Sup_Car_Out_Secondary_Color_Code = Car.CR_Cas_Sup_Car_Out_Secondary_Color_Code;
                        p.CR_Cas_Sup_Car_In_Main_Color_Code = Car.CR_Cas_Sup_Car_In_Main_Color_Code;
                        p.CR_Cas_Sup_Car_In_Secondary_Color_Code = Car.CR_Cas_Sup_Car_In_Secondary_Color_Code;
                        p.CR_Cas_Sup_Car_Plate_Ar_No = Car.CR_Cas_Sup_Car_Plate_Ar_No;
                        p.CR_Cas_Sup_Car_Plate_En_No = Car.CR_Cas_Sup_Car_Plate_En_No;
                        p.CR_Cas_Sup_Car_Collect_Ar_Name = Car.CR_Cas_Sup_Car_Collect_Ar_Name;
                        p.CR_Cas_Sup_Car_Collect_Ar_Short_Name = Car.CR_Cas_Sup_Car_Collect_Ar_Short_Name;
                        //p.CR_Cas_Sup_Car_Collect_En_Name = Car.CR_Cas_Sup_Car_Collect_En_Name;
                        p.CR_Cas_Sup_Car_Collect_En_Short_Name = Car.CR_Cas_Sup_Car_Collect_En_Short_Name;
                        // p.CR_Cas_Sup_Car_Collect_Fr_Name = Car.CR_Cas_Sup_Car_Collect_Fr_Name;
                        p.CR_Cas_Sup_Car_Collect_Fr_Short_Name = Car.CR_Cas_Sup_Car_Collect_Fr_Short_Name;
                        p.CR_Cas_Sup_Car_No_Current_Meter = Car.CR_Cas_Sup_Car_No_Current_Meter;
                        p.CR_Cas_Sup_Car_Traffic_License_Img = Car.CR_Cas_Sup_Car_Traffic_License_Img;
                        p.CR_Cas_Sup_Car_Joined_Fleet_Date = Car.CR_Cas_Sup_Car_Joined_Fleet_Date;
                        //p.CR_Cas_Sup_Car_Left_Fleet_Date = Car.CR_Cas_Sup_Car_Left_Fleet_Date;
                        //p.CR_Cas_Car_Price_Basic_Min_Age_Renter = CarPrice.CR_Cas_Car_Price_Basic_Min_Age_Renter;
                        p.CR_Cas_Car_Price_Basic_Additional_KM_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_KM_Value;
                        p.CR_Cas_Car_Price_Basic_No_Daily_Free_KM = CarPrice.CR_Cas_Car_Price_Basic_No_Daily_Free_KM;
                        p.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours = CarPrice.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours;
                        p.CR_Cas_Car_Price_Basic_Extra_Hour_Value = CarPrice.CR_Cas_Car_Price_Basic_Extra_Hour_Value;
                        if (Car.CR_Cas_Sup_Car_Last_Pictures == null || Car.CR_Cas_Sup_Car_Last_Pictures == "")
                        {
                            p.CR_Cas_Sup_Car_Last_Pictures = "~/images/img.png";
                        }
                        else
                        {
                            p.CR_Cas_Sup_Car_Last_Pictures = Car.CR_Cas_Sup_Car_Last_Pictures;
                        }

                        p.CR_Cas_Sup_Car_Documentation_Status = Car.CR_Cas_Sup_Car_Documentation_Status;
                        p.CR_Cas_Sup_Car_Maintenance_Status = Car.CR_Cas_Sup_Car_Maintenance_Status;
                        p.CR_Cas_Car_Price_Basic_Status = CarPrice.CR_Cas_Car_Price_Basic_Status;
                        p.CR_Cas_Sup_Car_Status = Car.CR_Cas_Sup_Car_Status;
                        p.CR_Cas_Sup_Car_Reasons = Car.CR_Cas_Sup_Car_Reasons;



                        p.CR_Cas_Car_Price_Basic_No = CarPrice.CR_Cas_Car_Price_Basic_No;
                        var featuresSum = db.CR_Cas_Car_Price_Features.Where(f => f.CR_Cas_Car_Price_Features_No == p.CR_Cas_Car_Price_Basic_No).Sum(f => f.CR_Cas_Car_Price_Features_Value);
                        if (featuresSum == null)
                        {
                            featuresSum = 0;
                        }

                        p.CR_Cas_Car_Price_Basic_Daily_Rent = CarPrice.CR_Cas_Car_Price_Basic_Daily_Rent + featuresSum;
                        p.CR_Cas_Car_Price_Basic_Weekly_Rent = CarPrice.CR_Cas_Car_Price_Basic_Weekly_Rent + featuresSum;
                        p.CR_Cas_Car_Price_Basic_Monthly_Rent = CarPrice.CR_Cas_Car_Price_Basic_Monthly_Rent + featuresSum;
                        p.CR_Cas_Car_Price_Basic_Rental_Tax_Rate = CarPrice.CR_Cas_Car_Price_Basic_Rental_Tax_Rate;
                        p.CR_Cas_Car_Price_Basic_No_Daily_Free_KM = CarPrice.CR_Cas_Car_Price_Basic_No_Daily_Free_KM;
                        p.CR_Cas_Car_Price_Basic_Additional_KM_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_KM_Value;
                        p.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours = CarPrice.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours;
                        p.CR_Cas_Car_Price_Basic_Hour_Max = CarPrice.CR_Cas_Car_Price_Basic_Hour_Max;
                        p.CR_Cas_Car_Price_Basic_Extra_Hour_Value = CarPrice.CR_Cas_Car_Price_Basic_Extra_Hour_Value;
                        p.CR_Cas_Car_Price_Basic_Min_Age_Renter = CarPrice.CR_Cas_Car_Price_Basic_Min_Age_Renter;
                        p.CR_Cas_Car_Price_Basic_Max_Age_Renter = CarPrice.CR_Cas_Car_Price_Basic_Max_Age_Renter;
                        p.CR_Cas_Car_Price_Basic_Additional_Driver = CarPrice.CR_Cas_Car_Price_Basic_Additional_Driver;
                        p.CR_Cas_Car_Price_Basic_Additional_Driver_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_Driver_Value;

                        p.CR_Cas_Car_Price_Basic_Internal_Fees_Tamm = CarPrice.CR_Cas_Car_Price_Basic_Internal_Fees_Tamm;
                        p.CR_Cas_Car_Price_Basic_International_Fees_Tamm = CarPrice.CR_Cas_Car_Price_Basic_International_Fees_Tamm;
                        p.CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value = CarPrice.CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value;
                        p.CR_Cas_Car_Price_Basic_Carrying_Accident = CarPrice.CR_Cas_Car_Price_Basic_Carrying_Accident;
                        p.CR_Cas_Car_Price_Basic_Carrying_Fire = CarPrice.CR_Cas_Car_Price_Basic_Carrying_Fire;
                        p.CR_Cas_Car_Price_Basic_Stealing = CarPrice.CR_Cas_Car_Price_Basic_Stealing;
                        p.CR_Cas_Car_Price_Basic_Drowning = CarPrice.CR_Cas_Car_Price_Basic_Drowning;
                        p.CR_Cas_Sup_Car_Model_Code = CarPrice.CR_Cas_Car_Price_Basic_Model_Code;
                        p.CR_Cas_Sup_Car_Year = CarPrice.CR_Cas_Car_Price_Basic_Car_Year;


                        if (Car.CR_Cas_Sup_Car_Documentation_Status != true)
                        {
                            p.CarsDocError = "التأكد من الوثائق";
                            // p.PassCarDoc = "False";
                        }
                        else
                        {
                            p.CarsDocError = "";
                        }
                        if (Car.CR_Cas_Sup_Car_Maintenance_Status != true)
                        {
                            p.CarsMainError = "التأكد من الصيانة";
                            //p.PassCarMain = "False";
                        }
                        else
                        {
                            p.CarsMainError = "";
                        }


                        lPrice.Add(p);
                    }

                }
            }

            List<CR_Cas_Sup_Branch> branchs = new List<CR_Cas_Sup_Branch>();
            var branchlist = db.CR_Cas_User_Branch_Validity.Where(v => v.CR_Cas_User_Branch_Validity_Id == UserLogin);
            foreach (var item in branchlist)
            {
                CR_Cas_Sup_Branch br = new CR_Cas_Sup_Branch();
                //var n = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == item.CR_Cas_User_Branch_Validity_Branch &&
                //b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Status == "A");


                /////////////////////////////////////////// New Code /////////////////////////////////////////////
                ///
                /////////////////////// when the branch is hold should all cars be hold //////////////////////////////////////////////

                var n = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == item.CR_Cas_User_Branch_Validity_Branch &&
                b.CR_Cas_Sup_Lessor_Code == LessorCode);

                if (n != null)
                {
                    br.CR_Cas_Sup_Branch_Code = item.CR_Cas_User_Branch_Validity_Branch;
                    br.CR_Cas_Sup_Branch_Ar_Name = n.CR_Cas_Sup_Branch_Ar_Name;
                    branchs.Add(br);
                }

            }
            ViewBag.BranchList = new SelectList(branchs, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", BranchCode);
            return PartialView(lPrice);
        }

        public PartialViewResult CarouselCarMaint()
        {
            IQueryable<CR_Cas_Sup_Car_Information> Cars;
            List<CarInfoPrice> lPrice = new List<CarInfoPrice>();
            var LessorCode = "";

            var UserLogin = "";
            var BranchCode = "";
            try
            {
                if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null)
                {
                    RedirectToAction("Login", "Account");
                }
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
               
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }



            //var Carsinfo = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
            //    c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode &&  c.CR_Cas_Sup_Car_Status != "A" && c.CR_Cas_Sup_Car_Price_Status == "0");

            //var Carsinfo = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
            //   c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode && c.CR_Cas_Sup_Car_Status != "A" );
            //var Carsinfo = db.CR_Cas_Sup_Car_Information.Where(n => n.CR_Cas_Sup_Car_Status == "A" && n.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&n.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode
            //    && (n.CR_Cas_Sup_Car_Price_Status != "1" || n.CR_Cas_Sup_Car_Maintenance_Status == false || n.CR_Cas_Sup_Car_Documentation_Status == false));

            //var Carsinfo = db.CR_Cas_Sup_Car_Information.Where(n => n.CR_Cas_Sup_Car_Status == "A" && n.CR_Cas_Sup_Car_Lessor_Code == LessorCode && n.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode
            //    && (n.CR_Cas_Sup_Car_Price_Status != "1" || n.CR_Cas_Sup_Car_Maintenance_Status == false || n.CR_Cas_Sup_Car_Documentation_Status == false));

            // Not Active Cars
            var NbrAllCars = db.CR_Cas_Sup_Car_Information.Where(a => a.CR_Cas_Sup_Car_Lessor_Code == LessorCode && a.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode);
            var NbrActive = db.CR_Cas_Sup_Car_Information.Where(a => a.CR_Cas_Sup_Car_Status == "A" && a.CR_Cas_Sup_Car_Lessor_Code == LessorCode && a.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode && a.CR_Cas_Sup_Car_Price_Status == "1" && a.CR_Cas_Sup_Car_Branch_Status == "A" && a.CR_Cas_Sup_Car_Owner_Status == "A");
            var NbrNotActive = db.CR_Cas_Sup_Car_Information.Where(n => n.CR_Cas_Sup_Car_Status == "A"&& n.CR_Cas_Sup_Car_Lessor_Code == LessorCode && n.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode && (n.CR_Cas_Sup_Car_Price_Status != "1" || n.CR_Cas_Sup_Car_Branch_Status == "H" && n.CR_Cas_Sup_Car_Owner_Status == "H"));
            var NbrHolded = db.CR_Cas_Sup_Car_Information.Where(h => h.CR_Cas_Sup_Car_Status == "H" && h.CR_Cas_Sup_Car_Lessor_Code == LessorCode && h.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode);
            var NbrBuy = db.CR_Cas_Sup_Car_Information.Where(h => h.CR_Cas_Sup_Car_Status == "O" && h.CR_Cas_Sup_Car_Lessor_Code == LessorCode && h.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode);
            var NbrRented = db.CR_Cas_Sup_Car_Information.Where(h => h.CR_Cas_Sup_Car_Status == "R" && h.CR_Cas_Sup_Car_Lessor_Code == LessorCode && h.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode);

            var branchh = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == BranchCode && b.CR_Cas_Sup_Lessor_Code==LessorCode);
            if (branchh.CR_Cas_Sup_Branch_Status=="H")
            {
                Cars = NbrActive.Concat(NbrNotActive).Concat(NbrHolded);
            }
            else
            {
                Cars = NbrNotActive.Concat(NbrHolded).Concat(NbrBuy);
            }
            foreach (var Car in Cars)
            {
                CarInfoPrice p = new CarInfoPrice();
                var CarPrice = db.CR_Cas_Car_Price_Basic.FirstOrDefault(price => price.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode &&
                 /*price.CR_Cas_Car_Price_Basic_Model_Code == Car.CR_Cas_Sup_Car_Model_Code &&*/ price.CR_Cas_Car_Price_Basic_Car_Year == Car.CR_Cas_Sup_Car_Year);
                if (CarPrice != null)
                {
                    var branch = db.CR_Cas_Sup_Branch.FirstOrDefault(b=>b.CR_Cas_Sup_Branch_Code==BranchCode&&b.CR_Cas_Sup_Lessor_Code==LessorCode);
                    if (branch != null)
                    {
                        p.CR_Cas_Sup_Branch = branch;
                    }
                    var Owner = db.CR_Cas_Sup_Owners.FirstOrDefault(b => b.CR_Cas_Sup_Owners_Code == Car.CR_Cas_Sup_Car_Owner_Code&&b.CR_Cas_Sup_Owners_Lessor_Code == LessorCode);
                    if (Owner != null)
                    {
                        p.CR_Cas_Sup_Owners = Owner;
                    }
                    ////////////////////////////////////
                    p.CR_Cas_Sup_Car_Lessor_Code = Car.CR_Cas_Sup_Car_Lessor_Code;
                    p.CR_Cas_Sup_Car_Model_Code = Car.CR_Cas_Sup_Car_Model_Code;
                    p.CR_Cas_Sup_Car_Year = Car.CR_Cas_Sup_Car_Year;
                    p.CR_Cas_Sup_Car_Serail_No = Car.CR_Cas_Sup_Car_Serail_No;
                    //////////////////////////////////////////////////////////
                    p.CR_Mas_Sup_Category_Car = db.CR_Mas_Sup_Category_Car.FirstOrDefault(c => c.CR_Mas_Sup_Category_Car_Code == Car.CR_Cas_Sup_Car_Category_Code &&
                    c.CR_Mas_Sup_Category_Car_Year == Car.CR_Cas_Sup_Car_Year /*&& c.CR_Mas_Sup_Category_Model_Code == Car.CR_Cas_Sup_Car_Model_Code*/);
                    if (p.CR_Mas_Sup_Category_Car != null)
                    {
                        p.CR_Mas_Sup_Color = Car.CR_Mas_Sup_Color;
                        p.CR_Mas_Sup_Model = Car.CR_Mas_Sup_Model;

                        p.CR_Cas_Sup_Car_Owner_Branch_Code = Car.CR_Cas_Sup_Car_Owner_Branch_Code;
                        p.CR_Cas_Sup_Car_Location_Branch_Code = Car.CR_Cas_Sup_Car_Location_Branch_Code;
                        p.CR_Cas_Sup_Car_Owner_Code = Car.CR_Cas_Sup_Car_Owner_Code;
                        p.CR_Cas_Sup_Car_Beneficiary_Code = Car.CR_Cas_Sup_Car_Beneficiary_Code;
                        p.CR_Cas_Sup_Car_Customs_No = Car.CR_Cas_Sup_Car_Customs_No;
                        p.CR_Cas_Sup_Car_Structure_No = Car.CR_Cas_Sup_Car_Structure_No;
                        p.CR_Cas_Sup_Car_Registration_Code = Car.CR_Cas_Sup_Car_Registration_Code;
                        p.CR_Cas_Sup_Car_Brand_Code = Car.CR_Cas_Sup_Car_Brand_Code;
                        p.CR_Cas_Sup_Car_Category_Code = Car.CR_Cas_Sup_Car_Category_Code;
                        p.CR_Cas_Sup_Car_Out_Main_Color_Code = Car.CR_Cas_Sup_Car_Out_Main_Color_Code;
                        p.CR_Cas_Sup_Car_Out_Secondary_Color_Code = Car.CR_Cas_Sup_Car_Out_Secondary_Color_Code;
                        p.CR_Cas_Sup_Car_In_Main_Color_Code = Car.CR_Cas_Sup_Car_In_Main_Color_Code;
                        p.CR_Cas_Sup_Car_In_Secondary_Color_Code = Car.CR_Cas_Sup_Car_In_Secondary_Color_Code;
                        p.CR_Cas_Sup_Car_Plate_Ar_No = Car.CR_Cas_Sup_Car_Plate_Ar_No;
                        p.CR_Cas_Sup_Car_Plate_En_No = Car.CR_Cas_Sup_Car_Plate_En_No;
                        p.CR_Cas_Sup_Car_Collect_Ar_Name = Car.CR_Cas_Sup_Car_Collect_Ar_Name;
                        p.CR_Cas_Sup_Car_Collect_Ar_Short_Name = Car.CR_Cas_Sup_Car_Collect_Ar_Short_Name;
                        //p.CR_Cas_Sup_Car_Collect_En_Name = Car.CR_Cas_Sup_Car_Collect_En_Name;
                        p.CR_Cas_Sup_Car_Collect_En_Short_Name = Car.CR_Cas_Sup_Car_Collect_En_Short_Name;
                        // p.CR_Cas_Sup_Car_Collect_Fr_Name = Car.CR_Cas_Sup_Car_Collect_Fr_Name;
                        p.CR_Cas_Sup_Car_Collect_Fr_Short_Name = Car.CR_Cas_Sup_Car_Collect_Fr_Short_Name;
                        p.CR_Cas_Sup_Car_No_Current_Meter = Car.CR_Cas_Sup_Car_No_Current_Meter;
                        p.CR_Cas_Sup_Car_Traffic_License_Img = Car.CR_Cas_Sup_Car_Traffic_License_Img;
                        p.CR_Cas_Sup_Car_Joined_Fleet_Date = Car.CR_Cas_Sup_Car_Joined_Fleet_Date;
                        //p.CR_Cas_Sup_Car_Left_Fleet_Date = Car.CR_Cas_Sup_Car_Left_Fleet_Date;
                        //p.CR_Cas_Car_Price_Basic_Min_Age_Renter = CarPrice.CR_Cas_Car_Price_Basic_Min_Age_Renter;
                        p.CR_Cas_Car_Price_Basic_Additional_KM_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_KM_Value;
                        p.CR_Cas_Car_Price_Basic_No_Daily_Free_KM = CarPrice.CR_Cas_Car_Price_Basic_No_Daily_Free_KM;
                        p.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours = CarPrice.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours;
                        p.CR_Cas_Car_Price_Basic_Extra_Hour_Value = CarPrice.CR_Cas_Car_Price_Basic_Extra_Hour_Value;

                        if (Car.CR_Cas_Sup_Car_Last_Pictures == null || Car.CR_Cas_Sup_Car_Last_Pictures == "")
                        {
                            p.CR_Cas_Sup_Car_Last_Pictures = "~/images/img.png";
                        }
                        else
                        {
                            p.CR_Cas_Sup_Car_Last_Pictures = Car.CR_Cas_Sup_Car_Last_Pictures;
                        }

                        p.CR_Cas_Sup_Car_Documentation_Status = Car.CR_Cas_Sup_Car_Documentation_Status;
                        p.CR_Cas_Sup_Car_Maintenance_Status = Car.CR_Cas_Sup_Car_Maintenance_Status;
                        p.CR_Cas_Car_Price_Basic_Status = CarPrice.CR_Cas_Car_Price_Basic_Status;
                        p.CR_Cas_Sup_Car_Status = Car.CR_Cas_Sup_Car_Status;
                        p.CR_Cas_Sup_Car_Reasons = Car.CR_Cas_Sup_Car_Reasons;



                        p.CR_Cas_Car_Price_Basic_No = CarPrice.CR_Cas_Car_Price_Basic_No;
                        var featuresSum = db.CR_Cas_Car_Price_Features.Where(f => f.CR_Cas_Car_Price_Features_No == p.CR_Cas_Car_Price_Basic_No).Sum(f => f.CR_Cas_Car_Price_Features_Value);
                        if (featuresSum == null)
                        {
                            featuresSum = 0;
                        }

                        p.CR_Cas_Car_Price_Basic_Daily_Rent = CarPrice.CR_Cas_Car_Price_Basic_Daily_Rent + featuresSum;
                        p.CR_Cas_Car_Price_Basic_Weekly_Rent = CarPrice.CR_Cas_Car_Price_Basic_Weekly_Rent + featuresSum;
                        p.CR_Cas_Car_Price_Basic_Monthly_Rent = CarPrice.CR_Cas_Car_Price_Basic_Monthly_Rent + featuresSum;
                        p.CR_Cas_Car_Price_Basic_Rental_Tax_Rate = CarPrice.CR_Cas_Car_Price_Basic_Rental_Tax_Rate;
                        p.CR_Cas_Car_Price_Basic_No_Daily_Free_KM = CarPrice.CR_Cas_Car_Price_Basic_No_Daily_Free_KM;
                        p.CR_Cas_Car_Price_Basic_Additional_KM_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_KM_Value;
                        p.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours = CarPrice.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours;
                        p.CR_Cas_Car_Price_Basic_Hour_Max = CarPrice.CR_Cas_Car_Price_Basic_Hour_Max;
                        p.CR_Cas_Car_Price_Basic_Extra_Hour_Value = CarPrice.CR_Cas_Car_Price_Basic_Extra_Hour_Value;
                        p.CR_Cas_Car_Price_Basic_Min_Age_Renter = CarPrice.CR_Cas_Car_Price_Basic_Min_Age_Renter;
                        p.CR_Cas_Car_Price_Basic_Max_Age_Renter = CarPrice.CR_Cas_Car_Price_Basic_Max_Age_Renter;
                        p.CR_Cas_Car_Price_Basic_Additional_Driver = CarPrice.CR_Cas_Car_Price_Basic_Additional_Driver;
                        p.CR_Cas_Car_Price_Basic_Additional_Driver_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_Driver_Value;

                        p.CR_Cas_Car_Price_Basic_Internal_Fees_Tamm = CarPrice.CR_Cas_Car_Price_Basic_Internal_Fees_Tamm;
                        p.CR_Cas_Car_Price_Basic_International_Fees_Tamm = CarPrice.CR_Cas_Car_Price_Basic_International_Fees_Tamm;
                        p.CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value = CarPrice.CR_Cas_Car_Price_Basic_Min_Percentage_Contract_Value;
                        p.CR_Cas_Car_Price_Basic_Carrying_Accident = CarPrice.CR_Cas_Car_Price_Basic_Carrying_Accident;
                        p.CR_Cas_Car_Price_Basic_Carrying_Fire = CarPrice.CR_Cas_Car_Price_Basic_Carrying_Fire;
                        p.CR_Cas_Car_Price_Basic_Stealing = CarPrice.CR_Cas_Car_Price_Basic_Stealing;
                        p.CR_Cas_Car_Price_Basic_Drowning = CarPrice.CR_Cas_Car_Price_Basic_Drowning;
                        p.CR_Cas_Sup_Car_Model_Code = CarPrice.CR_Cas_Car_Price_Basic_Model_Code;
                        p.CR_Cas_Sup_Car_Year = CarPrice.CR_Cas_Car_Price_Basic_Car_Year;

                        lPrice.Add(p);
                    }

                }
            }

            List<CR_Cas_Sup_Branch> branchs = new List<CR_Cas_Sup_Branch>();
            var branchlist = db.CR_Cas_User_Branch_Validity.Where(v => v.CR_Cas_User_Branch_Validity_Id == UserLogin);
            foreach (var item in branchlist)
            {
                CR_Cas_Sup_Branch br = new CR_Cas_Sup_Branch();

                //var n = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == item.CR_Cas_User_Branch_Validity_Branch &&
                //b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Status == "A");


                /////////////////////////////////////////// New Code /////////////////////////////////////////////
                ///
                /////////////////////// when the branch is hold should all cars be hold //////////////////////////////////////////////
                var n = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == item.CR_Cas_User_Branch_Validity_Branch &&
                b.CR_Cas_Sup_Lessor_Code == LessorCode);
                if (n != null)
                {
                    br.CR_Cas_Sup_Branch_Code = item.CR_Cas_User_Branch_Validity_Branch;
                    br.CR_Cas_Sup_Branch_Ar_Name = n.CR_Cas_Sup_Branch_Ar_Name;
                    branchs.Add(br);
                }

            }
            ViewBag.BranchList = new SelectList(branchs, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", BranchCode);
            return PartialView(lPrice);
        }


        public ActionResult BranchStat()
        {
            
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                if (Session["LessorCode"] == null || Session["UserLogin"] == null || Session["BranchCode"] == null)
                {
                    RedirectToAction("Login", "Account");
                }
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();
                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                
            }
            catch
            {
              return  RedirectToAction("Login", "Account");
            }


            var Cars = db.CR_Cas_Sup_Car_Information.Where(c=>c.CR_Cas_Sup_Car_Lessor_Code==LessorCode && c.CR_Cas_Sup_Car_Location_Branch_Code==BranchCode && c.CR_Cas_Sup_Car_Status!="D" 
            && c.CR_Cas_Sup_Car_Status!="O");

            if (Cars != null)
            {
                ViewBag.NbrTot = Cars.Count();

                //ViewBag.NbrActive = Cars.Where(a=>a.CR_Cas_Sup_Car_Status == "A" && a.CR_Cas_Sup_Car_Price_Status == "1" && a.CR_Cas_Sup_Car_Documentation_Status==true
                //&& a.CR_Cas_Sup_Car_Maintenance_Status==true).Count() ;
                
                // Rented Cars

                var branch = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == BranchCode && b.CR_Cas_Sup_Lessor_Code == LessorCode);
                var Owner = db.CR_Cas_Sup_Owners.FirstOrDefault(b => b.CR_Cas_Sup_Owners_Lessor_Code == LessorCode);

                // Active Cars
                var NbrActive = Cars.Where(a => a.CR_Cas_Sup_Car_Status == "A" && a.CR_Cas_Sup_Car_Price_Status == "1").Count();

                // Not Active Cars
                var NbrNotActive = Cars.Where(n => n.CR_Cas_Sup_Car_Status == "A" && n.CR_Cas_Sup_Car_Price_Status != "1").Count();
                //var NbrNotActive2 = Cars.Where(n => n.CR_Cas_Sup_Car_Status != "A" && n.CR_Cas_Sup_Car_Status!="H"&&n.CR_Cas_Sup_Car_Status!="R" &&n.CR_Cas_Sup_Car_Status!="O").Count();
                var NbrHolded = Cars.Where(h => h.CR_Cas_Sup_Car_Status == "H").Count();
                var NbrTobuy = Cars.Where(h => h.CR_Cas_Sup_Car_Status == "O").Count();

              
                    // Active Cars
                    ViewBag.NbrActive = NbrActive;
                    // Not Active Cars                  
                    ViewBag.NbrNotActive = NbrNotActive + NbrHolded + NbrTobuy;
                    // Ranted Cats
                    ViewBag.NbrRented = Cars.Where(r => r.CR_Cas_Sup_Car_Status == "R").Count();
            }

            var Contracts = db.CR_Cas_Contract_Basic.Where(c=>c.CR_Cas_Contract_Basic_Lessor==LessorCode && c.CR_Cas_Contract_Basic_Owner_Branch==BranchCode);
            if (Contracts != null)
            {
                var date = DateTime.Now.Date;
                var afterDay =  date.AddDays(1);
                ViewBag.NbrContracts = Contracts.Where(c => c.CR_Cas_Contract_Basic_Status != "U" && c.CR_Cas_Contract_Basic_Status != "y").Count();
                ViewBag.NbrActiveContracts = Contracts.Where(a=>a.CR_Cas_Contract_Basic_Status=="A").Count();
                ViewBag.NbrexpiredContracts = Contracts.Where(a => a.CR_Cas_Contract_Basic_Status == "E").Count();
                // this day
                ViewBag.NbrDexpiredContracts = Contracts.Where(a => a.CR_Cas_Contract_Basic_Expected_End_Date== date && a.CR_Cas_Contract_Basic_Status != "C").Count();
                // after day
                ViewBag.NbrTexpiredContracts = Contracts.Where(a => a.CR_Cas_Contract_Basic_Expected_End_Date == afterDay && a.CR_Cas_Contract_Basic_Status != "C"&& a.CR_Cas_Contract_Basic_Status=="A").Count();
                ViewBag.NbrClosedContracts = Contracts.Where(a=>a.CR_Cas_Contract_Basic_Status=="C").Count();
            }

            var Sp = db.CR_Cas_Sup_SalesPoint.Where(r=>r.CR_Cas_Sup_SalesPoint_Com_Code==LessorCode && r.CR_Cas_Sup_SalesPoint_Brn_Code==BranchCode);
            if (Sp != null)
            {
                var bank = LessorCode + "0000";
                var TotReceipt = Sp.Select(b=>b.CR_Cas_Sup_SalesPoint_Balance).Sum();
                var TotCash = Sp.Where(c=>c.CR_Cas_Sup_SalesPoint_Bank_No==bank).Select(s=>s.CR_Cas_Sup_SalesPoint_Balance).Sum();
                var SalesPoint = TotReceipt - TotCash;


                ViewBag.TotReceipt = TotReceipt;
                ViewBag.TotCash = TotCash;
                ViewBag.TotSalesPoint = SalesPoint;
            }

            List<CR_Cas_Sup_Branch> branchs = new List<CR_Cas_Sup_Branch>();
            var branchlist = db.CR_Cas_User_Branch_Validity.Where(v => v.CR_Cas_User_Branch_Validity_Id == UserLogin);
            foreach (var item in branchlist)
            {
                CR_Cas_Sup_Branch br = new CR_Cas_Sup_Branch();

                // Old Code 
                //var nActice = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == item.CR_Cas_User_Branch_Validity_Branch &&
                //b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Status == "A");
                //if (nActice != null)
                //{
                //    br.CR_Cas_Sup_Branch_Code = item.CR_Cas_User_Branch_Validity_Branch;
                //    br.CR_Cas_Sup_Branch_Ar_Name = nActice.CR_Cas_Sup_Branch_Ar_Name;
                //    branchs.Add(br);
                //}

                /////////////////////////////////////////// New Code /////////////////////////////////////////////
                ///
                /////////////////////// when the branch is hold should all cars be hold //////////////////////////////////////////////

                var n = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == item.CR_Cas_User_Branch_Validity_Branch &&
                b.CR_Cas_Sup_Lessor_Code == LessorCode);              


                if (n != null)
                {                   
                    br.CR_Cas_Sup_Branch_Code = item.CR_Cas_User_Branch_Validity_Branch;
                    br.CR_Cas_Sup_Branch_Ar_Name = n.CR_Cas_Sup_Branch_Ar_Name;
                    branchs.Add(br);
                }
            }
            GetValidityContract(UserLogin, BranchCode, LessorCode);
            GetBranchExpiredDocs(LessorCode, BranchCode);
            ViewBag.BranchList = new SelectList(branchs, "CR_Cas_Sup_Branch_Code", "CR_Cas_Sup_Branch_Ar_Name", BranchCode);

            return View();
        }


        public void GetBranchExpiredDocs(string LessorCode,string BranchCode)
        {
            int nbr = 0;
            var BranchdocsN = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
            && d.CR_Cas_Sup_Branch_Documentation_Status == "N" && d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode).Count() != 0 ? db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
            && d.CR_Cas_Sup_Branch_Documentation_Status == "N" && d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode).Count() : 0;


            Session["BranchdocsN"] = BranchdocsN;

            var BranchdocsX = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
            && d.CR_Cas_Sup_Branch_Documentation_Status == "X" && d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode).Count() != 0 ? db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
            && d.CR_Cas_Sup_Branch_Documentation_Status == "X" && d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode).Count() : 0;

            Session["BranchdocsX"] = BranchdocsX;

            var BranchdocsE = db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
            && d.CR_Cas_Sup_Branch_Documentation_Status == "E" && d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode).Count() != 0 ? db.CR_Cas_Sup_Branch_Documentation.Where(d => d.CR_Cas_Sup_Branch_Documentation_Lessor_Code == LessorCode
            && d.CR_Cas_Sup_Branch_Documentation_Status == "E" && d.CR_Cas_Sup_Branch_Documentation_Branch_Code == BranchCode).Count() : 0;
            Session["BranchdocsE"] = BranchdocsE;

            var Companydocs = BranchdocsN + BranchdocsX + BranchdocsE;


            var BranchPricesN = db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode
            && d.CR_Cas_Car_Price_Basic_Status == "N" && d.CR_Cas_Car_Price_Basic_Lessor_Code == BranchCode).Count() !=0 ? db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode
            && d.CR_Cas_Car_Price_Basic_Status == "N" && d.CR_Cas_Car_Price_Basic_Lessor_Code == BranchCode).Count() : 0;

            Session["BranchPricesN"] = BranchPricesN;

            var BranchPricesX = db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode
            && d.CR_Cas_Car_Price_Basic_Status == "X" && d.CR_Cas_Car_Price_Basic_Lessor_Code == BranchCode).Count() != 0 ? db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode
            && d.CR_Cas_Car_Price_Basic_Status == "X" && d.CR_Cas_Car_Price_Basic_Lessor_Code == BranchCode).Count() : 0;
            Session["BranchPricesX"] = BranchPricesX;

            var BranchPricesE = db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode
            && d.CR_Cas_Car_Price_Basic_Status == "E" && d.CR_Cas_Car_Price_Basic_Lessor_Code == BranchCode).Count() != 0 ? db.CR_Cas_Car_Price_Basic.Where(d => d.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode
            && d.CR_Cas_Car_Price_Basic_Status == "E" && d.CR_Cas_Car_Price_Basic_Lessor_Code == BranchCode).Count() :0;
            Session["PricesE"] = BranchPricesE;

            var Prices = BranchPricesX + BranchPricesE;




            var BranchCarsDocN = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "N" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code==BranchCode).Count();
            Session["BranchCarsDocN"] = BranchCarsDocN != 0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "N" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count() :0 ;

            var BranchCarsDocX = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "X" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count() != 0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "X" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count():0;
            Session["BranchCarsDocX"] = BranchCarsDocX;

            var BranchCarsDocE = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count() !=0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count() :0;
            Session["BranchCarsDocE"] = BranchCarsDocE;

            var CarsDoc = BranchCarsDocN + BranchCarsDocX + BranchCarsDocE;




            var BranchCarsMaintenanceN = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "N" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count() != 0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "N" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count() :0;
            Session["BranchCarsMaintenanceN"] = BranchCarsMaintenanceN;

            var BranchCarsMaintenanceE = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "X" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count() !=0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "X" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count() : 0;
            Session["BranchCarsMaintenanceE"] = BranchCarsMaintenanceE;

            var BranchCarsMaintenanceX = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count() != 0 ? db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Lessor_Code == LessorCode && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4"
            && d.CR_Cas_Sup_Car_Doc_Mainten_Status == "E" && d.CR_Cas_Sup_Car_Doc_Mainten_Branch_Code == BranchCode).Count() : 0;
            Session["BranchCarsMaintenanceX"] = BranchCarsMaintenanceX;

            var CarsMaintenance = BranchCarsMaintenanceN + BranchCarsMaintenanceX + BranchCarsMaintenanceE;


            nbr = nbr + Companydocs  + Prices + CarsDoc + CarsMaintenance;
            Session["BranchAlertsNbr"] = nbr;

        }

    }
}