using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using RentCar.Models;
using RentCar.Models.BranchSys;
using RentCar.Models.CAS;
using RentCar.Models.RptModels;
using RentCar.Reports.ContractBasicReports.ContractCr;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading;
using System.Web.Mvc;
using System.Drawing;
using System.Net.Mail;
using System.Net.Mime;

namespace RentCar.Controllers.BranchSys
{
    public class BasicContractController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: BasicContract
        public ActionResult Index()
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    return RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            var cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Where(c=>c.CR_Cas_Contract_Basic_Lessor==LessorCode && c.CR_Cas_Contract_Basic_Owner_Branch==BranchCode && c.CR_Cas_Contract_Basic_Status!="U")
                .Include(c => c.CR_Mas_Com_Lessor);
            return View(cR_Cas_Contract_Basic.ToList());
        }

        public JsonResult getsalespoint(string paycode)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                    RedirectToAction("Login", "Account");
            }
            var Code = LessorCode + "0000";
            db.Configuration.ProxyCreationEnabled = false;
            List<CR_Cas_Sup_SalesPoint> SalesPoint = null;
            if (paycode=="10")
            {
                SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(x => x.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && x.CR_Cas_Sup_SalesPoint_Brn_Code==BranchCode &&
                x.CR_Cas_Sup_SalesPoint_Bank_Code==Code && x.CR_Cas_Sup_SalesPoint_Status=="A" && x.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status=="A").ToList();
            }
            else if(paycode!="" && paycode!=null)
            {
                SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(x => x.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && x.CR_Cas_Sup_SalesPoint_Brn_Code == BranchCode &&
               x.CR_Cas_Sup_SalesPoint_Bank_Code != Code && x.CR_Cas_Sup_SalesPoint_Status == "A" && x.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "A").ToList();
            }
            else
            {
                
            }
            
            
            return Json(SalesPoint, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCarsDocsAuthority(string SerialNo)
        {
            CarDocsModel DocsModel = new CarDocsModel();
            DocsModel.PassCar = true;
            var l = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Serail_No == SerialNo && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3").Include(p => p.CR_Mas_Sup_Procedures);
            if (l != null)
            {
                foreach (var d in l)
                {
                    if (d.CR_Cas_Sup_Car_Doc_Mainten_Code == "26" && (d.CR_Cas_Sup_Car_Doc_Mainten_End_Date < DateTime.Now || d.CR_Cas_Sup_Car_Doc_Mainten_End_Date == null))
                    {
                        if (Session["TrafficLicense"].ToString() != "True")
                        {
                            DocsModel.TrafficLicenseError = "رخصة السير منتهية";
                            DocsModel.PassCar = false;
                        }
                        else
                        {
                            DocsModel.TrafficLicenseError = "";
                        }
                    }
                    else
                    {
                        DocsModel.TrafficLicenseError = "";
                    }

                    if (d.CR_Cas_Sup_Car_Doc_Mainten_Code == "27" && (d.CR_Cas_Sup_Car_Doc_Mainten_End_Date < DateTime.Now || d.CR_Cas_Sup_Car_Doc_Mainten_End_Date == null))
                    {

                        if (Session["ContractInsurance"].ToString() != "True")
                        {
                            DocsModel.ContractInsuranceError = "التأمين منتهي";
                            DocsModel.PassCar = false;
                        }
                        else
                        {
                            DocsModel.ContractInsuranceError = "";
                        }

                    }
                    else
                    {
                        DocsModel.ContractInsuranceError = "";
                    }

                    if (d.CR_Cas_Sup_Car_Doc_Mainten_Code == "28" && (d.CR_Cas_Sup_Car_Doc_Mainten_End_Date < DateTime.Now || d.CR_Cas_Sup_Car_Doc_Mainten_End_Date == null))
                    {

                        if (Session["ContractOperatingCard"].ToString() != "True")
                        {
                            DocsModel.ContractOperatingCardError = "بطاقة التشغيل منتهية";
                            DocsModel.PassCar = false;
                        }
                        else
                        {
                            DocsModel.ContractOperatingCardError = "";
                        }

                    }
                    else
                    {
                        DocsModel.ContractOperatingCardError = "";
                    }


                    if (d.CR_Cas_Sup_Car_Doc_Mainten_Code == "29" && (d.CR_Cas_Sup_Car_Doc_Mainten_End_Date < DateTime.Now || d.CR_Cas_Sup_Car_Doc_Mainten_End_Date == null))
                    {

                        if (Session["ContractChkecUp"].ToString() != "True")
                        {
                            DocsModel.ContractChkecUpError = "الفحص الدوري منتهي";
                            DocsModel.PassCar = false;
                        }
                        else
                        {
                            DocsModel.ContractChkecUpError = "";
                        }

                    }
                    else
                    {
                        DocsModel.ContractChkecUpError = "";
                    }
                }
            }
            return Json(DocsModel, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetCarsMainAuthority(string SerialNo)
        {
            CarDocsModel DocsModel = new CarDocsModel();
            DocsModel.PassCar = true;
            var l = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Serail_No == SerialNo && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4").Include(p => p.CR_Mas_Sup_Procedures);
            if (l != null)
            {
                foreach (var d in l)
                {
                    if (d.CR_Cas_Sup_Car_Doc_Mainten_Code == "36" && (d.CR_Cas_Sup_Car_Doc_Mainten_End_Date < DateTime.Now || d.CR_Cas_Sup_Car_Doc_Mainten_End_Date == null))
                    {
                        if (Session["ContractTires"].ToString() != "True")
                        {
                            DocsModel.ContractTires = "صيانة الإطارات منتهية";
                            DocsModel.PassCar = false;
                        }
                        else
                        {
                            DocsModel.ContractTires = "";
                        }
                    }
                    else
                    {
                        DocsModel.ContractTires = "";
                    }

                    if (d.CR_Cas_Sup_Car_Doc_Mainten_Code == "37" && (d.CR_Cas_Sup_Car_Doc_Mainten_End_Date < DateTime.Now || d.CR_Cas_Sup_Car_Doc_Mainten_End_Date == null))
                    {

                        if (Session["ContractOil"].ToString() != "True")
                        {
                            DocsModel.ContractOil = "صيانة زيت المكينة منتهية";
                            DocsModel.PassCar = false;
                        }
                        else
                        {
                            DocsModel.ContractOil = "";
                        }

                    }
                    else
                    {
                        DocsModel.ContractOil = "";
                    }

                    if (d.CR_Cas_Sup_Car_Doc_Mainten_Code == "38" && (d.CR_Cas_Sup_Car_Doc_Mainten_End_Date < DateTime.Now || d.CR_Cas_Sup_Car_Doc_Mainten_End_Date == null))
                    {

                        if (Session["ContractMaintenance"].ToString() != "True")
                        {
                            DocsModel.ContractMaintenance = "الصيانة الدورية منتهية";
                            DocsModel.PassCar = false;
                        }
                        else
                        {
                            DocsModel.ContractMaintenance = "";
                        }

                    }
                    else
                    {
                        DocsModel.ContractMaintenance = "";
                    }


                    if (d.CR_Cas_Sup_Car_Doc_Mainten_Code == "39" && (d.CR_Cas_Sup_Car_Doc_Mainten_End_Date < DateTime.Now || d.CR_Cas_Sup_Car_Doc_Mainten_End_Date == null))
                    {

                        if (Session["ContractFBrakeMaintenance"].ToString() != "True")
                        {
                            DocsModel.ContractFBrakeMaintenance = "صيانة الفرامل الأمامية منتهية";
                            DocsModel.PassCar = false;
                        }
                        else
                        {
                            DocsModel.ContractFBrakeMaintenance = "";
                        }

                    }
                    else
                    {
                        DocsModel.ContractFBrakeMaintenance = "";
                    }

                    if (d.CR_Cas_Sup_Car_Doc_Mainten_Code == "40" && (d.CR_Cas_Sup_Car_Doc_Mainten_End_Date < DateTime.Now || d.CR_Cas_Sup_Car_Doc_Mainten_End_Date == null))
                    {

                        if (Session["ContractBBrakeMaintenance"].ToString() != "True")
                        {
                            DocsModel.ContractBBrakeMaintenance = "صيانة الفرامل الخلفية منتهية";
                            DocsModel.PassCar = false;
                        }
                        else
                        {
                            DocsModel.ContractBBrakeMaintenance = "";
                        }

                    }
                    else
                    {
                        DocsModel.ContractBBrakeMaintenance = "";
                    }
                }
            }
            return Json(DocsModel, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AllContracts()
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    return RedirectToAction("Account", "Login"); 
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            var cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Owner_Branch == BranchCode && c.CR_Cas_Contract_Basic_Status!="U")
                .OrderByDescending(d=>d.CR_Cas_Contract_Basic_Date)
                .Include(c => c.CR_Mas_Com_Lessor).Include(car=>car.CR_Cas_Sup_Car_Information);
            return View(cR_Cas_Contract_Basic.ToList());
        }

        public ActionResult RenterContracts(string id)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    return RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }

            var Renter = db.CR_Cas_Renter_Lessor.FirstOrDefault(r=>r.CR_Cas_Renter_Lessor_Id==id && r.CR_Cas_Renter_Lessor_Code==LessorCode);
            ViewBag.RenterID = id;
            ViewBag.RenterName = Renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Ar_Name;
            ViewBag.RenterJobs = Renter.CR_Mas_Renter_Information.CR_Mas_Sup_Jobs.CR_Mas_Sup_Jobs_Ar_Name;
            ViewBag.ContractsNo = Renter.CR_Cas_Renter_Lessor_Contract_Number;
            ViewBag.RentalDays = Renter.CR_Cas_Renter_Lessor_Days;
            ViewBag.RenterNationality =db.CR_Mas_Sup_Nationalities.FirstOrDefault(nat=>nat.CR_Mas_Sup_Nationalities_Code==Renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Nationality).CR_Mas_Sup_Nationalities_Ar_Name;
            var RenterMembership = Renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Membership;
            var Membership = db.CR_Mas_Sup_Membership.FirstOrDefault(m=>m.CR_Mas_Sup_Membership_Code==RenterMembership);
            if (Membership != null)
            {
                ViewBag.RenterMembership =Membership.CR_Mas_Sup_Membership_Ar_Name ;
            }
            
            ViewBag.RenterFirstInteraction = string.Format("{0:yyyy/MM/dd}", Renter.CR_Cas_Renter_Lessor_Date_First_Interaction);
            ViewBag.RenterLastInteraction = string.Format("{0:yyyy/MM/dd}", Renter.CR_Cas_Renter_Lessor_Date_Last_Interaction);
            ViewBag.RenterRating = Renter.CR_Cas_Renter_Rating;
            ViewBag.Km = Renter.CR_Cas_Renter_Lessor_KM;
            ViewBag.InteractionValue = Renter.CR_Cas_Renter_Lessor_Interaction_Amount_Value;
            ViewBag.RenterBalance = Renter.CR_Cas_Renter_Lessor_Balance;

            var cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode
                && (c.CR_Cas_Contract_Basic_Status != "U") &&
                (c.CR_Cas_Contract_Basic_Renter_Id == id || c.CR_Cas_Contract_Basic_Driver_Id == id || c.CR_Cas_Contract_Basic_Additional_Driver_Id == id))
                .Include(c => c.CR_Mas_Com_Lessor).Include(car => car.CR_Cas_Sup_Car_Information);

            
            return View(cR_Cas_Contract_Basic.ToList());
        }

        public JsonResult GetCurrentMeter(string No)
        {
            var car = db.CR_Cas_Sup_Car_Information.FirstOrDefault(c => c.CR_Cas_Sup_Car_Serail_No == No);
            var currentMeter = car.CR_Cas_Sup_Car_No_Current_Meter;
            return Json(currentMeter, JsonRequestBehavior.AllowGet);
        }

        ////public JsonResult GetRenterInfoAPI(string ID)
        ////{
        ////    var renters = db.CR_Mas_Renter_Information.FirstOrDefault(c => c.CR_Mas_Renter_Information_Id == ID);
        ////    CR_Mas_Renter_Information r = new CR_Mas_Renter_Information();
        ////    r.CR_Mas_Renter_Information_Id = renters.CR_Mas_Renter_Information_Id;
        ////    r.CR_Mas_Renter_Information_Ar_Name = renters.CR_Mas_Renter_Information_Ar_Name;
        ////    r.CR_Mas_Renter_Information_BirthDate = renters.CR_Mas_Renter_Information_BirthDate;
        ////    r.CR_Mas_Renter_Information_Educational_Qualification = renters.CR_Mas_Renter_Information_Educational_Qualification;
        ////    r.CR_Mas_Renter_Information_Email = renters.CR_Mas_Renter_Information_Email;
        ////    return Json(r, JsonRequestBehavior.AllowGet);
        ////}

        public PartialViewResult VirtuelInspection()
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            var l = db.CR_Mas_Sup_Virtual_Inspection.Where(c => c.CR_Mas_Sup_Virtual_Inspection_Status == "A");

            return PartialView(l);
        }

        public PartialViewResult ChoicesList(string No)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            var l = db.CR_Cas_Car_Price_Choices.Where(c => c.CR_Cas_Car_Price_Choices_No == No);

            return PartialView(l);
        }

        public PartialViewResult CategoriesList(string Sector)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            List<CR_Mas_Sup_Category> Lcategories = new List<CR_Mas_Sup_Category>();

            if (Sector != null && Sector != "")
            {
                var l = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode && c.CR_Cas_Sup_Car_Status == "A");



                CR_Mas_Sup_Category ct = new CR_Mas_Sup_Category();
                ct.CR_Mas_Sup_Category_Ar_Name = "الكل";
                ct.CR_Mas_Sup_Category_En_Name = "All";
                ct.CR_Mas_Sup_Category_Code = "0";
                Lcategories.Add(ct);
                foreach (var item in l)
                {
                    var ex = Lcategories.Any(x => x.CR_Mas_Sup_Category_Code == item.CR_Cas_Sup_Car_Category_Code);
                    var CarPrice = db.CR_Cas_Car_Price_Basic.Any(p => p.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode
                    && p.CR_Cas_Car_Price_Basic_Car_Year == item.CR_Cas_Sup_Car_Year && p.CR_Cas_Car_Price_Basic_Model_Code == item.CR_Cas_Sup_Car_Model_Code
                    && p.CR_Cas_Car_Price_Basic_Sector == Sector && p.CR_Cas_Car_Price_Basic_Status == "A");


                    if (!ex && CarPrice)
                    {
                        CR_Mas_Sup_Category categorie = new CR_Mas_Sup_Category();
                        categorie.CR_Mas_Sup_Category_Code = item.CR_Cas_Sup_Car_Category_Code;
                        var cat = db.CR_Mas_Sup_Category.FirstOrDefault(c => c.CR_Mas_Sup_Category_Code == item.CR_Cas_Sup_Car_Category_Code);
                        categorie.CR_Mas_Sup_Category_Ar_Name = cat.CR_Mas_Sup_Category_Ar_Name;
                        categorie.CR_Mas_Sup_Category_En_Name = cat.CR_Mas_Sup_Category_En_Name;
                        Lcategories.Add(categorie);
                    }

                }
            }


            return PartialView(Lcategories);
        }

        public PartialViewResult AdditionalList(string No)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            var l = db.CR_Cas_Car_Price_Additional.Where(c => c.CR_Cas_Car_Price_Additional_No == No);

            return PartialView(l);
        }

        public PartialViewResult CarsList(string Sector, string Category)
            {
            IQueryable<CR_Cas_Sup_Car_Information> Cars;
            List<CarInfoPrice> lPrice = new List<CarInfoPrice>();

            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch (Exception ex)
            {
                RedirectToAction("Login", "Account");
            }


            string CategoryCode = "";

            if (Category != null && Category != "")
            {

                string catname = Category.ToLower().Replace(" ", "");
                var cat = db.CR_Mas_Sup_Category.FirstOrDefault(x => x.CR_Mas_Sup_Category_Ar_Name.ToLower().Replace(" ", "").Contains(catname));
                if (cat != null)
                {
                    CategoryCode = cat.CR_Mas_Sup_Category_Code;
                }


                if (Category != "الكل")
                {
                    var Carsinfo = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Status == "A" && c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                   c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode && c.CR_Cas_Sup_Car_Category_Code == CategoryCode && c.CR_Cas_Sup_Car_Price_Status=="1");
                    Cars = Carsinfo;
                }
                else
                {
                    var Carsinfo = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Status == "A" && c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                    c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode && c.CR_Cas_Sup_Car_Price_Status == "1");
                    Cars = Carsinfo;
                }




                foreach (var Car in Cars)
                {
                    CarInfoPrice p = new CarInfoPrice();
                    var CarPrice = db.CR_Cas_Car_Price_Basic.FirstOrDefault(price => price.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode 
                    && price.CR_Cas_Car_Price_Basic_Sector == Sector &&
                     price.CR_Cas_Car_Price_Basic_Model_Code == Car.CR_Cas_Sup_Car_Model_Code && price.CR_Cas_Car_Price_Basic_Car_Year == Car.CR_Cas_Sup_Car_Year 
                     && price.CR_Cas_Car_Price_Basic_Status == "A");
                    if (CarPrice != null)
                    {
                        ////////////////////////////////////
                        p.CR_Cas_Sup_Car_Lessor_Code = Car.CR_Cas_Sup_Car_Lessor_Code;
                        p.CR_Cas_Sup_Car_Model_Code = Car.CR_Cas_Sup_Car_Model_Code;
                        p.CR_Cas_Sup_Car_Year = Car.CR_Cas_Sup_Car_Year;
                        p.CR_Cas_Sup_Car_Serail_No = Car.CR_Cas_Sup_Car_Serail_No;
                        //////////////////////////////////////////////////////////
                        p.CR_Mas_Sup_Category_Car = db.CR_Mas_Sup_Category_Car.FirstOrDefault(c=>c.CR_Mas_Sup_Category_Car_Code==Car.CR_Cas_Sup_Car_Category_Code &&
                        c.CR_Mas_Sup_Category_Car_Year==Car.CR_Cas_Sup_Car_Year && c.CR_Mas_Sup_Category_Model_Code == Car.CR_Cas_Sup_Car_Model_Code);
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
                            //p.CR_Cas_Sup_Car_Price_Status = Car.CR_Cas_Sup_Car_Price_Status;
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

                            p.PassCarDoc = "True";
                            p.PassCarMain = "True";

                            if (Car.CR_Cas_Sup_Car_Documentation_Status != true)
                            {
                                p.CarsDocError = "التأكد من الوثائق";
                                p.PassCarDoc = "False";
                            }
                            else
                            {
                                p.CarsDocError = "";
                            }
                            if (Car.CR_Cas_Sup_Car_Maintenance_Status != true)
                            {
                                p.CarsMainError = "التأكد من الصيانة";
                                p.PassCarMain = "False";
                            }
                            else
                            {
                                p.CarsMainError = "";
                            }


                            lPrice.Add(p);
                        }
                        
                    }
                }
            }
            else
            {
                if (Sector != null )
                {
                    var Carsinfo = db.CR_Cas_Sup_Car_Information.Where(c => c.CR_Cas_Sup_Car_Status == "A" && c.CR_Cas_Sup_Car_Lessor_Code == LessorCode &&
                    c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode);
                    Cars = Carsinfo;
                    //var ca = Cars.Count();            

                    foreach (var Car in Cars)
                    {
                        CarInfoPrice p = new CarInfoPrice();
                        var CarPrice = db.CR_Cas_Car_Price_Basic.FirstOrDefault(price => price.CR_Cas_Car_Price_Basic_Lessor_Code == LessorCode && price.CR_Cas_Car_Price_Basic_Sector == Sector &&
                         price.CR_Cas_Car_Price_Basic_Model_Code == Car.CR_Cas_Sup_Car_Model_Code && price.CR_Cas_Car_Price_Basic_Car_Year == Car.CR_Cas_Sup_Car_Year && price.CR_Cas_Car_Price_Basic_Status == "A");
                        if (CarPrice != null)
                        {
                            ////////////////////////////////////
                            p.CR_Cas_Sup_Car_Lessor_Code = Car.CR_Cas_Sup_Car_Lessor_Code;
                            p.CR_Cas_Sup_Car_Model_Code = Car.CR_Cas_Sup_Car_Model_Code;
                            p.CR_Cas_Sup_Car_Year = Car.CR_Cas_Sup_Car_Year;
                            p.CR_Cas_Sup_Car_Serail_No = Car.CR_Cas_Sup_Car_Serail_No;
                            //////////////////////////////////////////////////////////
                            p.CR_Mas_Sup_Category_Car = db.CR_Mas_Sup_Category_Car.FirstOrDefault(c => c.CR_Mas_Sup_Category_Car_Code == Car.CR_Cas_Sup_Car_Category_Code &&
                            c.CR_Mas_Sup_Category_Car_Year==Car.CR_Cas_Sup_Car_Year && c.CR_Mas_Sup_Category_Model_Code == Car.CR_Cas_Sup_Car_Model_Code);
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
                                //p.CR_Cas_Sup_Car_Price_Status = Car.CR_Cas_Sup_Car_Price_Status;
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



                                p.PassCarDoc = "True";
                                p.PassCarMain = "True";

                                if (Car.CR_Cas_Sup_Car_Documentation_Status != true)
                                {
                                    p.CarsDocError = "التأكد من الوثائق";
                                    p.PassCarDoc = "False";
                                }
                                else
                                {
                                    p.CarsDocError = "";
                                }
                                if (Car.CR_Cas_Sup_Car_Maintenance_Status != true)
                                {
                                    p.CarsMainError = "التأكد من الصيانة";
                                    p.PassCarMain = "False";
                                }
                                else
                                {
                                    p.CarsMainError = "";
                                }

                                lPrice.Add(p);
                            }
                            
                        }
                    }
                }
            }

                return PartialView(lPrice);
        }


        public PartialViewResult CarsDocs(string SerialNo)
        { 
            var l = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Serail_No == SerialNo && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "3");
            return PartialView(l);
        }


        public PartialViewResult CarMaint(string SerialNo)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }

            var l = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Serail_No == SerialNo && d.CR_Cas_Sup_Car_Doc_Mainten_Type == "4");
            return PartialView(l);

        }


        // GET: BasicContract/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
            if (cR_Cas_Contract_Basic == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Contract_Basic);
        }


        public int get_age(DateTime dob)
        {
            int age = 0;
            age = DateTime.Now.Subtract(dob).Days;
            age = age / 365;
            return age;
        }

        public JsonResult getCustomerInfo(string id, string operation,string RenterMobile/*,string RenterEmail*/,string Driver_Mobile,/*string Driver_Email,*/string Additional_Driver_Mobile
            /*,string Additional_Driver_Email*/,string LessorCode,string BranchCode,string UserLogin,string ContractNo)
        {
            var renterinfo = db.CR_Mas_Renter_Information.FirstOrDefault(x => x.CR_Mas_Renter_Information_Id == id);
            if (renterinfo != null)
            {
                Models.BranchSys.RenterInfoModel r = new Models.BranchSys.RenterInfoModel();
                r.PassRenter = true;
                r.CR_Mas_Renter_Information_Accidents = renterinfo.CR_Mas_Renter_Information_Accidents;
                r.CR_Mas_Renter_Information_Ar_Name = renterinfo.CR_Mas_Renter_Information_Ar_Name;
                r.CR_Mas_Renter_Information_Bank = renterinfo.CR_Mas_Renter_Information_Bank;
                r.CR_Mas_Renter_Information_BirthDate = renterinfo.CR_Mas_Renter_Information_BirthDate;
                r.CR_Mas_Renter_Information_Contract_Number = renterinfo.CR_Mas_Renter_Information_Contract_Number;
                r.CR_Mas_Renter_Information_Date_First_Interaction = renterinfo.CR_Mas_Renter_Information_Date_First_Interaction;
                r.CR_Mas_Renter_Information_Date_Last_Interaction = renterinfo.CR_Mas_Renter_Information_Date_Last_Interaction;
                r.CR_Mas_Renter_Information_Days = renterinfo.CR_Mas_Renter_Information_Days;
                r.CR_Mas_Renter_Information_Educational_Qualification = renterinfo.CR_Mas_Renter_Information_Educational_Qualification;
                r.CR_Mas_Renter_Information_Email = renterinfo.CR_Mas_Renter_Information_Email;
                r.CR_Mas_Renter_Information_En_Name = renterinfo.CR_Mas_Renter_Information_En_Name;
                r.CR_Mas_Renter_Information_Evaluation_Count = renterinfo.CR_Mas_Renter_Information_Evaluation_Count;
                r.CR_Mas_Renter_Information_Evaluation_Value = renterinfo.CR_Mas_Renter_Information_Evaluation_Value;
                r.CR_Mas_Renter_Information_Expiry_Driving_License_Date = renterinfo.CR_Mas_Renter_Information_Expiry_Driving_License_Date;
                r.CR_Mas_Renter_Information_Expiry_Id_Date = renterinfo.CR_Mas_Renter_Information_Expiry_Id_Date;
                r.CR_Mas_Renter_Information_Gender = renterinfo.CR_Mas_Renter_Information_Gender;
                r.CR_Mas_Renter_Information_Iban = renterinfo.CR_Mas_Renter_Information_Iban;
                r.CR_Mas_Renter_Information_Id = renterinfo.CR_Mas_Renter_Information_Id;
                r.CR_Mas_Renter_Information_Issue_Id_Date = renterinfo.CR_Mas_Renter_Information_Issue_Id_Date;
                r.CR_Mas_Renter_Information_Jobs = renterinfo.CR_Mas_Renter_Information_Jobs;
                r.CR_Mas_Renter_Information_Membership = renterinfo.CR_Mas_Renter_Information_Membership;
                r.CR_Mas_Renter_Information_Mobile = RenterMobile;
                r.CR_Mas_Renter_Information_Nationality = renterinfo.CR_Mas_Renter_Information_Nationality;
                r.CR_Mas_Renter_Information_Pasport_Date = renterinfo.CR_Mas_Renter_Information_Pasport_Date;
                r.CR_Mas_Renter_Information_Pasport_Expiry_Date = renterinfo.CR_Mas_Renter_Information_Pasport_Expiry_Date;
                r.CR_Mas_Renter_Information_Passport_No = renterinfo.CR_Mas_Renter_Information_Passport_No;
                r.CR_Mas_Renter_Information_Reasons = renterinfo.CR_Mas_Renter_Information_Reasons;
                r.CR_Mas_Renter_Information_Requests_Number = renterinfo.CR_Mas_Renter_Information_Requests_Number;
                r.CR_Mas_Renter_Information_Reservations_Executed_Number = renterinfo.CR_Mas_Renter_Information_Reservations_Executed_Number;
                r.CR_Mas_Renter_Information_Reservations_Number = renterinfo.CR_Mas_Renter_Information_Reservations_Number;
                r.CR_Mas_Renter_Information_Sector = renterinfo.CR_Mas_Renter_Information_Sector;
                r.CR_Mas_Renter_Information_Signature = renterinfo.CR_Mas_Renter_Information_Signature;
                r.CR_Mas_Renter_Information_Social = renterinfo.CR_Mas_Renter_Information_Social;
                r.CR_Mas_Renter_Information_Status = renterinfo.CR_Mas_Renter_Information_Status;
                r.CR_Mas_Renter_Information_Traveled_Distance = renterinfo.CR_Mas_Renter_Information_Traveled_Distance;
                r.CR_Mas_Renter_Information_UpDate_Personal = renterinfo.CR_Mas_Renter_Information_UpDate_Personal;
                r.CR_Mas_Renter_Information_UpDate_Post = renterinfo.CR_Mas_Renter_Information_UpDate_Workplace;
                r.CR_Mas_Renter_Information_Value = renterinfo.CR_Mas_Renter_Information_Value;
                r.CR_Mas_Renter_Information_Violations = renterinfo.CR_Mas_Renter_Information_Violations;
                r.CR_Mas_Renter_Information_Visits_Number = renterinfo.CR_Mas_Renter_Information_Visits_Number;
                r.CR_Mas_Renter_Information_Workplace_Subscription = renterinfo.CR_Mas_Renter_Information_Workplace_Subscription;



                renterinfo.CR_Mas_Renter_Information_Mobile = RenterMobile;
                
                //if(RenterEmail!=null && RenterEmail != "")
                //{
                //    renterinfo.CR_Mas_Renter_Information_Email = RenterEmail;
                //}
                //else if(Driver_Email!=null && Driver_Email != "")
                //{
                //    renterinfo.CR_Mas_Renter_Information_Email = Driver_Email;
                //}
                //else
                //{
                //    renterinfo.CR_Mas_Renter_Information_Email = Additional_Driver_Email;
                //}

                db.Entry(renterinfo).State = EntityState.Modified;

                var CasRenter = db.CR_Cas_Renter_Lessor.FirstOrDefault(rt=>rt.CR_Cas_Renter_Lessor_Id==id && rt.CR_Cas_Renter_Lessor_Code == LessorCode);
                if (CasRenter != null)
                {
                    if (CasRenter.CR_Cas_Renter_Lessor_Status == "R")
                    {
                        r.HaveContract = true;
                    }
                    else
                    {
                        r.HaveContract= false;
                    }
                }

                var job = db.CR_Mas_Sup_Jobs.FirstOrDefault(j => j.CR_Mas_Sup_Jobs_Code == r.CR_Mas_Renter_Information_Jobs);
                if (job != null)
                {
                    r.Job = job.CR_Mas_Sup_Jobs_Ar_Name;
                }

                r.EmployerErrorMessage = "";
                var employer = db.CR_Mas_Sup_Employer.FirstOrDefault(e => e.CR_Mas_Sup_Employer_Code == r.CR_Mas_Renter_Information_Workplace_Subscription);
                if (employer != null)
                {
                    r.Employer = employer.CR_Mas_Sup_Employer_Ar_Name;
                }
                else
                {
                    if (Session["ContractEmployer"].ToString() != "True")
                    {
                        r.EmployerErrorMessage = "جهة العمل غير متوفرة";
                        r.PassRenter = false;
                    }
                    else
                    {
                        r.EmployerErrorMessage = "";
                    }
                }


                var gender = db.CR_Mas_Sup_Gender.FirstOrDefault(g => g.CR_Mas_Sup_Gender_Code == r.CR_Mas_Renter_Information_Gender);
                if (gender != null)
                {
                    r.Gender = gender.CR_Mas_Sup_Gender_Ar_Name;
                }


                var Nationality = db.CR_Mas_Sup_Nationalities.FirstOrDefault(n => n.CR_Mas_Sup_Nationalities_Code == r.CR_Mas_Renter_Information_Nationality);
                if (Nationality != null)
                {
                    r.Nationality = Nationality.CR_Mas_Sup_Nationalities_Ar_Name;
                }


                r.Age = get_age((DateTime)renterinfo.CR_Mas_Renter_Information_BirthDate);



                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                CR_Cas_Contract_Basic contract = new CR_Cas_Contract_Basic();
                if (operation == "")
                {

                    //////////////////////////////////////Create Contract///////////////////////////////////
                    DateTime year = DateTime.Now;
                    var y = year.ToString("yy");

                    var Sector = r.CR_Mas_Renter_Information_Sector;
                    var autoinc = GetContractLastRecord(Sector, LessorCode, BranchCode).CR_Cas_Contract_Basic_No;
                    contract.CR_Cas_Contract_Basic_No = y + "-" + Sector + "-" + "90" + "-" + LessorCode + "-" + BranchCode + autoinc;
                    contract.CR_Cas_Contract_Basic_Owner_Branch = BranchCode;
                    contract.CR_Cas_Contract_Basic_Year = int.Parse(y);
                    contract.CR_Cas_Contract_Basic_Type = 90;
                    contract.CR_Cas_Contract_Basic_Lessor = LessorCode;
                    contract.CR_Cas_Contract_Basic_Sector = Sector;
                    contract.CR_Cas_Contract_Basic_Date = DateTime.Now;
                    string currentTime = DateTime.Now.ToString("HH:mm:ss");
                    contract.CR_Cas_Contract_Basic_Time = TimeSpan.Parse(currentTime);
                    contract.CR_Cas_Contract_Basic_Status = "U";
                    contract.CR_Cas_Contract_Basic_User_Insert = UserLogin;
                    contract.CR_Cas_Contract_Basic_Copy = 1;
                    if (id != null)
                    {
                        //contract.CR_Cas_Contract_Basic_Renter_Id = int.Parse(id);
                        contract.CR_Cas_Contract_Basic_Renter_Id = id;
                    }

                    db.CR_Cas_Contract_Basic.Add(contract);


                    ////////////////////////////////Add Message account/////////////////////////
                    CR_Mas_Account_Msg_Owed MSG = new CR_Mas_Account_Msg_Owed();
                    MSG.CR_Cas_Account_Msg_Owed_Contract_No = contract.CR_Cas_Contract_Basic_No;
                    MSG.CR_Cas_Account_Msg_Owed_Counter = MSGOwedLastRecord(contract.CR_Cas_Contract_Basic_No);
                    MSG.CR_Cas_Account_Msg_Owed_Value = (decimal?)0.5;
                    MSG.CR_Cas_Account_Msg_Owed_Due_Date = DateTime.Now;
                    MSG.CR_Cas_Account_Msg_Owed_Is_Paid = false;
                    MSG.CR_Cas_Account_Msg_Owed_Pay_Date = null;
                    MSG.CR_Cas_Account_Msg_Owed_Pay_No = "";
                    MSG.CR_Cas_Account_Msg_Owed_Contract_Code = "1";
                    db.CR_Mas_Account_Msg_Owed.Add(MSG);

                    

                    r.TracingNo = contract.CR_Cas_Contract_Basic_No;

                }
                else
                {
                    contract.CR_Cas_Contract_Basic_No = ContractNo;
                }

                ////////////////////////////////////////////////////// Check driving licence and update if necessary//////////////////////////////////////////////////
                ///
                CR_Elm_Personal elminfo = null;
                
                if (r.CR_Mas_Renter_Information_Expiry_Driving_License_Date <= DateTime.Now || r.CR_Mas_Renter_Information_Expiry_Id_Date <= DateTime.Now)
                {
                    elminfo = db.CR_Elm_Personal.FirstOrDefault(f => f.CR_Elm_ID == id);
                    if (elminfo.CR_Elm_Expiry_Driver_Date > renterinfo.CR_Mas_Renter_Information_Expiry_Id_Date|| elminfo.CR_Elm_Expiry_ID_Date >renterinfo.CR_Mas_Renter_Information_Expiry_Driving_License_Date)
                    {
                        r.CR_Mas_Renter_Information_Expiry_Driving_License_Date = elminfo.CR_Elm_Expiry_Driver_Date;
                        r.CR_Mas_Renter_Information_Expiry_Id_Date = elminfo.CR_Elm_Expiry_ID_Date;

                        renterinfo.CR_Mas_Renter_Information_Expiry_Driving_License_Date = elminfo.CR_Elm_Expiry_Driver_Date;
                        renterinfo.CR_Mas_Renter_Information_Expiry_Id_Date = elminfo.CR_Elm_Expiry_ID_Date;
                        renterinfo.CR_Mas_Renter_Information_UpDate_License = DateTime.Now;

                        ////////////////////////////////Add Licence Owed////////////////////////////////////

                        CR_Mas_Account_License_Owed LOwed = new CR_Mas_Account_License_Owed();
                        LOwed.CR_Mas_Account_License_Owed_Contract_No = contract.CR_Cas_Contract_Basic_No;
                        LOwed.CR_Mas_Account_License_Owed_Counter = LicenseOwedLastRecord(contract.CR_Cas_Contract_Basic_No);
                        LOwed.CR_Mas_Account_License_Owed_Value = 4;
                        LOwed.CR_Mas_Account_License_Owed_Due_Date = DateTime.Now;
                        LOwed.CR_Mas_Account_License_Owed_Is_Paid = false;
                        LOwed.CR_Mas_Account_License_Owed_Pay_Date = null;
                        LOwed.CR_Mas_Account_License_Owed_Pay_No = "";
                        LOwed.CR_Mas_Account_License_Owed_Contract_Code = "5";

                        db.CR_Mas_Account_License_Owed.Add(LOwed);
                        ////////////////////////////////////////////////////////////////////////////////////////

                        db.Entry(renterinfo).State = EntityState.Modified;

                        
                        if (elminfo.CR_Elm_Expiry_Driver_Date <= DateTime.Now)
                        {
                            if (Session["ContractDrivingLicense"].ToString() != "True")
                            {
                                r.DrivingLicenceErrorMessage = "رخصة القيادة منتهية";
                                r.PassRenter = false;
                            }
                            else
                            {
                                r.DrivingLicenceErrorMessage = "";
                            }

                        }
                        else
                        {
                            r.DrivingLicenceErrorMessage = "";
                        }


                        if (elminfo.CR_Elm_Expiry_ID_Date <= DateTime.Now)
                        {
                            if (Session["ContractId"].ToString() != "True")
                            {
                                r.IdErrorMessage = "الهوية منتهية";
                                r.PassRenter = false;
                            }
                            else
                            {
                                r.IdErrorMessage = "";
                            }

                        }
                        else
                        {
                            r.IdErrorMessage = "";
                        }
                    }
                }


                

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                r.AddressErrorMessage = "";
                var renterAdr = db.CR_Mas_Address.FirstOrDefault(x => x.CR_Mas_Address_Id_Code == id);
                if (renterAdr != null)
                {
                    var RegionName = db.CR_Mas_Sup_Regions.FirstOrDefault(p => p.CR_Mas_Sup_Regions_Code == renterAdr.CR_Mas_Address_Regions);
                    var CityName = db.CR_Mas_Sup_City.FirstOrDefault(e => e.CR_Mas_Sup_City_Code == renterAdr.CR_Mas_Address_City);

                    r.address = RegionName.CR_Mas_Sup_Regions_Ar_Name + " - " + CityName.CR_Mas_Sup_City_Ar_Name + " - " + renterAdr.CR_Mas_Address_Ar_District + " - "
                        + renterAdr.CR_Mas_Address_Ar_Street + " - " + renterAdr.CR_Mas_Address_Building + " - " + renterAdr.CR_Mas_Address_Ar_District + " - " +"وحدة رقم " 
                        + renterAdr.CR_Mas_Address_Unit_No  + " - " + renterAdr.CR_Mas_Address_Zip_Code + " - " + renterAdr.CR_Mas_Address_Additional_Numbers;
                }
                else
                {
                  
                    var elmAddress = db.CR_ELM_Address.FirstOrDefault(a=>a.CR_ELM_Ad_Code==id);
                    if (elmAddress != null)
                    {
                        var OurRegionCode = "";
                        var OurCityCode = "";
                        CR_Mas_Address MasAddress = new CR_Mas_Address();
                        var area = db.CR_Mas_Sup_Regions.FirstOrDefault(a => a.CR_Mas_Sup_Regions_Ar_Name.ToLower().Replace(" ", "").Contains(elmAddress.CR_ELM_Ad_Ar_Area.ToLower().Replace(" ", "")));
                        if (area != null)
                        {
                            OurRegionCode = area.CR_Mas_Sup_Regions_Code;
                        }
                        var City = db.CR_Mas_Sup_City.FirstOrDefault(c => c.CR_Mas_Sup_City_Ar_Name.ToLower().Replace(" ", "").Contains(elmAddress.CR_ELM_Ad_Ar_City.ToLower().Replace(" ", "")));
                        if (City != null)
                        {
                            OurCityCode = City.CR_Mas_Sup_City_Code;
                        }
                        MasAddress.CR_Mas_Address_Id_Code = id;
                        MasAddress.CR_Mas_Address_Regions = OurRegionCode;
                        MasAddress.CR_Mas_Address_City = OurCityCode;


                        MasAddress.CR_Mas_Address_Ar_District = elmAddress.CR_ELM_Ad_Ar_District;
                        MasAddress.CR_Mas_Address_En_District = elmAddress.CR_ELM_Ad_EN_District;
                        MasAddress.CR_Mas_Address_Ar_Street = elmAddress.CR_ELM_Ad_Ar_Street;
                        MasAddress.CR_Mas_Address_En_Street = elmAddress.CR_ELM_Ad_En_Street;
                        MasAddress.CR_Mas_Address_Building = elmAddress.CR_ELM_Ad_Bld_No;
                        MasAddress.CR_Mas_Address_Unit_No = elmAddress.CR_ELM_Ad_Unit_No;
                        MasAddress.CR_Mas_Address_Zip_Code = int.Parse(elmAddress.CR_ELM_Ad_Zip_Code);
                        MasAddress.CR_Mas_Address_Additional_Numbers = int.Parse(elmAddress.CR_ELM_Ad_Extra_No);

                        MasAddress.CR_Mas_Address_UpDate_Post = DateTime.Now;


                        ////////////////////////////////Add Post Owed////////////////////////////////////
                        CR_Mas_Account_Post_Owed PostOwed = new CR_Mas_Account_Post_Owed();
                        PostOwed.CR_Mas_Account_Post_Owed_Contract_No = contract.CR_Cas_Contract_Basic_No;
                        PostOwed.CR_Mas_Account_Post_Owed_Counter = PostOwedLastRecord(contract.CR_Cas_Contract_Basic_No);
                        PostOwed.CR_Mas_Account_Post_Owed_Value = 3;
                        PostOwed.CR_Mas_Account_Post_Owed_Due_Date = DateTime.Now;
                        PostOwed.CR_Mas_Account_Post_Owed_Is_Paid = false;
                        PostOwed.CR_Mas_Account_Post_Owed_Pay_Date = null;
                        PostOwed.CR_Mas_Account_Post_Owed_Pay_No = "";
                        PostOwed.CR_Mas_Account_Post_Owed_Contract_Code = "3";

                        db.CR_Mas_Account_Post_Owed.Add(PostOwed);
                        /////////////////////////////
                    }
                    else
                    {
                        if (Session["ContractRenterAddress"].ToString() != "True")
                        {
                            r.AddressErrorMessage = "العنوان الوطني غير متوفر";
                            r.PassRenter = false;
                        }
                        else
                        {
                            r.AddressErrorMessage = "";
                        }
                    }

                }

                var casrenter = db.CR_Cas_Renter_Lessor.FirstOrDefault(casr=> casr.CR_Cas_Renter_Lessor_Id==id && casr.CR_Cas_Renter_Lessor_Code==LessorCode);
                if(casrenter==null)
                {
                    CR_Cas_Renter_Lessor NewCasRenter = new CR_Cas_Renter_Lessor();
                    NewCasRenter.CR_Cas_Renter_Lessor_Id = id;
                    NewCasRenter.CR_Cas_Renter_Lessor_Code = LessorCode;
                    NewCasRenter.CR_Cas_Renter_Lessor_Date_First_Interaction = DateTime.Now;
                    NewCasRenter.CR_Cas_Renter_Lessor_Balance = 0;
                    NewCasRenter.CR_Cas_Renter_Lessor_Interaction_Amount_Value = 0;
                    NewCasRenter.CR_Cas_Renter_Lessor_Contract_Number = 0;
                    NewCasRenter.CR_Cas_Renter_Lessor_KM = 0;
                    NewCasRenter.CR_Cas_Renter_Lessor_Days = 0;
                    NewCasRenter.CR_Cas_Renter_Rating = 0;
                    NewCasRenter.CR_Cas_Renter_Lessor_Status = "A";
                    db.CR_Cas_Renter_Lessor.Add(NewCasRenter);
                    r.PreviousBalance = 0;
                }
                else
                {
                    if (casrenter.CR_Cas_Renter_Lessor_Balance != null)
                    {
                        r.PreviousBalance = (decimal)casrenter.CR_Cas_Renter_Lessor_Balance;
                    }
                    else
                    {
                        r.PreviousBalance =0;
                    }
                    
                }


                
                
               
                r.RenterMessageCode = 123;

                ///
                db.SaveChanges();


                return Json(r, JsonRequestBehavior.AllowGet);
            }
            else if (renterinfo == null)/////////////////////////////Data from ELM/////////////////////////////////////////////
            {
                //  targeted action must be renter id
                /////////////////////////////////////// Retrive data Tracing/////////////////////////////
                /// // اذا كان اول رقم واحد او اثنين : قطاع أفراد
                //elm.CR_Elm_Sector = ElmInfo.CR_Elm_Sector;
                var SubSector = id.Substring(0, 1);
                var OurSectorCode = "";
                if (SubSector == "1" || SubSector == "2")
                {
                     OurSectorCode = "1";
                }
                else if (SubSector == "7")
                {
                     OurSectorCode = "2";
                }
                
                ///////////////////////////////////////////////////////////////////////////////

                Models.BranchSys.ElmInfoModel elm = new Models.BranchSys.ElmInfoModel();
                var ElmInfo = db.CR_Elm_Personal.FirstOrDefault(e => e.CR_Elm_ID == id);
                if (ElmInfo != null)
                {
                    elm.CR_Elm_Ar_Educational_Qualification = ElmInfo.CR_Elm_Ar_Educational_Qualification;
                    elm.CR_Elm_Ar_Gender = ElmInfo.CR_Elm_Ar_Gender;
                    elm.CR_Elm_Ar_Jobs = ElmInfo.CR_Elm_Ar_Jobs;
                    elm.CR_Elm_Ar_Name = ElmInfo.CR_Elm_Ar_Name;
                    elm.CR_Elm_Ar_Nationality = ElmInfo.CR_Elm_Ar_Nationality;
                    elm.CR_Elm_Email = ElmInfo.CR_Elm_Email;
                    elm.CR_Elm_Ar_Social = ElmInfo.CR_Elm_Ar_Social;
                    elm.CR_Elm_Ar_Workplace_Subscription = ElmInfo.CR_Elm_Ar_Workplace_Subscription;
                    elm.CR_Elm_BirthDate = ElmInfo.CR_Elm_BirthDate;
                    if(RenterMobile!="")
                    {
                        elm.CR_Elm_Mobile = RenterMobile;
                    }
                    if (Driver_Mobile != "" )
                    {
                        elm.CR_Elm_Mobile = Driver_Mobile;
                    }
                    if (Additional_Driver_Mobile != "")
                    {
                        elm.CR_Elm_Mobile = Additional_Driver_Mobile;
                    }

                    elm.CR_Elm_En_Name = ElmInfo.CR_Elm_En_Name;
                    elm.CR_Elm_En_Nationality = elm.CR_Elm_En_Nationality;
                    elm.CR_Elm_En_Social = elm.CR_Elm_En_Social;

                    elm.CR_Elm_Expiry_Driver_Date = ElmInfo.CR_Elm_Expiry_Driver_Date;
                    elm.CR_Elm_Expiry_ID_Date = ElmInfo.CR_Elm_Expiry_ID_Date;


                    elm.PassRenter = true;
                    if (DateTime.Now >= ElmInfo.CR_Elm_Expiry_Driver_Date)
                    {

                        if (Session["ContractDrivingLicense"].ToString() != "True")
                        {
                            elm.IdErrorMessage = "صلاحية رخصة القيادة منتهية";
                            elm.PassRenter = false;
                        }
                        else
                        {
                            elm.IdErrorMessage = "";
                        }


                    }
                    else
                    {
                        elm.IdErrorMessage = "";
                    }

                   
                    if (DateTime.Now >= ElmInfo.CR_Elm_Expiry_ID_Date)
                    {
                       
                        if (Session["ContractId"].ToString() != "True")
                        {
                            elm.IdErrorMessage = "صلاحية الهوية منتهية";
                            elm.PassRenter = false;
                        }
                        else
                        {
                            elm.IdErrorMessage = "";
                        }
                    }
                    else
                    {
                        elm.IdErrorMessage = "";
                    }



                    elm.CR_Elm_ID = ElmInfo.CR_Elm_ID;
                    elm.CR_Elm_Issue_ID_Date = ElmInfo.CR_Elm_Issue_ID_Date;
                    
                    
                    elm.OurSectorCode = OurSectorCode;
                    elm.CR_Elm_BirthDate = ElmInfo.CR_Elm_BirthDate;//string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                    elm.Age = get_age((DateTime)elm.CR_Elm_BirthDate);
                    //var s = db.CR_Mas_Sup_Sector.FirstOrDefault(x=>x.CR_Mas_Sup_Sector_Ar_Name.ToLower().Replace(" ","").Contains(elm.CR_Elm_Sector.ToLower().Replace(" ","")));
                    //if (s != null)
                    //{
                    //    elm.OurSectorCode = s.CR_Mas_Sup_Sector_Code;
                    //}


                    var Nat = db.CR_Mas_Sup_Nationalities.FirstOrDefault(n => n.CR_Mas_Sup_Nationalities_Ar_Name.ToLower().Replace(" ", "").Contains(elm.CR_Elm_Ar_Nationality.ToLower().Replace(" ", "")));
                    if (Nat != null)
                    {
                        elm.OurNationalityCode = Nat.CR_Mas_Sup_Nationalities_Code;
                    }

                    var gender = db.CR_Mas_Sup_Gender.FirstOrDefault(g => g.CR_Mas_Sup_Gender_Ar_Name.ToLower().Replace(" ", "").Contains(elm.CR_Elm_Ar_Gender.ToLower().Replace(" ", "")));
                    if (gender != null)
                    {
                        elm.OurGenderCode = gender.CR_Mas_Sup_Gender_Code;
                    }

                    var j = db.CR_Mas_Sup_Jobs.FirstOrDefault(g => g.CR_Mas_Sup_Jobs_Ar_Name.ToLower().Replace(" ", "").Contains(elm.CR_Elm_Ar_Jobs.ToLower().Replace(" ", "")));
                    if (gender != null)
                    {
                        elm.OurJobsCode = j.CR_Mas_Sup_Jobs_Code;
                    }


                    var social = db.CR_Mas_Sup_Social.FirstOrDefault(so => so.CR_Mas_Sup_Social_Ar_Name.ToLower().Replace(" ", "").Contains(elm.CR_Elm_Ar_Social.ToLower().Replace(" ", "")));
                    if (social != null)
                    {
                        elm.OurSocialCode = social.CR_Mas_Sup_Social_Code;
                    }

                    var qualification = db.CR_Mas_Sup_Educational_Qualification.FirstOrDefault(q => q.CR_Mas_Sup_Educational_Qualification_Ar_Name.ToLower().Replace(" ", "").Contains(elm.CR_Elm_Ar_Educational_Qualification.ToLower().Replace(" ", "")));
                    if (qualification != null)
                    {
                        elm.OurQualificationCode = qualification.CR_Mas_Sup_Educational_Qualification_Code;
                    }

                    elm.EmployerErrorMessage = "";
                    var work = db.CR_Mas_Sup_Employer.FirstOrDefault(w => w.CR_Mas_Sup_Employer_Ar_Name.ToLower().Replace(" ", "").Contains(elm.CR_Elm_Ar_Workplace_Subscription.ToLower().Replace(" ", "")));
                    if (work != null)
                    {
                        elm.OurEmployerCode = work.CR_Mas_Sup_Employer_Code;
                    }
                    else
                    {
                        if (Session["ContractEmployer"].ToString() != "True")
                        {
                            elm.EmployerErrorMessage = "جهة العمل غير متوفرة";
                            elm.PassRenter = false;
                        }
                        else
                        {
                            elm.EmployerErrorMessage = "";
                        }
                    }

                    Random r = new Random();
                    elm.Code = r.Next(1000, 9999);
                    //////////////////////////////Save renter in our base////////////////////////////////
                    CR_Mas_Renter_Information cust = new CR_Mas_Renter_Information();
                    cust.CR_Mas_Renter_Information_Id = ElmInfo.CR_Elm_ID;
                    cust.CR_Mas_Renter_Information_Sector = elm.OurSectorCode;

                    cust.CR_Mas_Renter_Information_Ar_Name = ElmInfo.CR_Elm_Ar_Name;
                    cust.CR_Mas_Renter_Information_En_Name = ElmInfo.CR_Elm_En_Name;
                    cust.CR_Mas_Renter_Information_BirthDate = ElmInfo.CR_Elm_BirthDate;
                    cust.CR_Mas_Renter_Information_Issue_Id_Date = ElmInfo.CR_Elm_Issue_ID_Date;
                    cust.CR_Mas_Renter_Information_Expiry_Id_Date = ElmInfo.CR_Elm_Expiry_ID_Date;
                    cust.CR_Mas_Renter_Information_Expiry_Driving_License_Date = ElmInfo.CR_Elm_Expiry_Driver_Date;
                    cust.CR_Mas_Renter_Information_Workplace_Subscription = elm.OurEmployerCode;
                    cust.CR_Mas_Renter_Information_Nationality = elm.OurNationalityCode;// we need also arabe and english nationality in renter information table
                    cust.CR_Mas_Renter_Information_Gender = elm.OurGenderCode;// we need also arabe and english gender in renter information table
                    cust.CR_Mas_Renter_Information_Jobs = elm.OurJobsCode;// we need also arabe and english jobs in renter information table
                    cust.CR_Mas_Renter_Information_Educational_Qualification = elm.OurQualificationCode;//we need both languages
                    cust.CR_Mas_Renter_Information_Social = elm.OurSocialCode;// we need both languages
                    cust.CR_Mas_Renter_Information_Membership = "1600000006";
                    cust.CR_Mas_Renter_Information_Signature = "";
                    cust.CR_Mas_Renter_Information_Mobile = elm.CR_Elm_Mobile;
                    cust.CR_Mas_Renter_Information_Email = elm.CR_Elm_Email;
                    cust.CR_Mas_Renter_Information_Passport_No = null;
                    cust.CR_Mas_Renter_Information_Pasport_Date = null;
                    cust.CR_Mas_Renter_Information_Pasport_Expiry_Date = null;
                    cust.CR_Mas_Renter_Information_Iban = null;
                    cust.CR_Mas_Renter_Information_Bank = null;
                    cust.CR_Mas_Renter_Information_UpDate_Personal = DateTime.Now;
                    cust.CR_Mas_Renter_Information_UpDate_Post = DateTime.Now;
                    cust.CR_Mas_Renter_Information_UpDate_License = DateTime.Now;
                    cust.CR_Mas_Renter_Information_Date_First_Interaction = DateTime.Now;
                    cust.CR_Mas_Renter_Information_Date_Last_Interaction = DateTime.Now;
                    cust.CR_Mas_Renter_Information_Status = "A";
                    cust.CR_Mas_Renter_Information_Evaluation_Count = 0;
                    cust.CR_Mas_Renter_Information_Evaluation_Value = 0;
                    cust.CR_Mas_Renter_Information_Days= 0;
                    cust.CR_Mas_Renter_Information_Value= 0;
                    cust.CR_Mas_Renter_Information_Contract_Number= 0;
                    cust.CR_Mas_Renter_Information_UpDate_Workplace = DateTime.Now.Date;

                    db.CR_Mas_Renter_Information.Add(cust);
                    ////////////////////////////////////////////////////////////////////////////////////
                    /////////////////// add renter to cas renter lessor/////////////////////
                    CR_Cas_Renter_Lessor NewCasRenter = new CR_Cas_Renter_Lessor();
                    NewCasRenter.CR_Cas_Renter_Lessor_Id = id;
                    NewCasRenter.CR_Cas_Renter_Lessor_Code = LessorCode;
                    NewCasRenter.CR_Cas_Renter_Lessor_Date_First_Interaction = DateTime.Now;
                    NewCasRenter.CR_Cas_Renter_Lessor_Date_Last_Interaction = DateTime.Now;
                    NewCasRenter.CR_Cas_Renter_Lessor_Balance = 0;
                    NewCasRenter.CR_Cas_Renter_Lessor_Interaction_Amount_Value = 0;
                    NewCasRenter.CR_Cas_Renter_Lessor_Contract_Number = 0;
                    NewCasRenter.CR_Cas_Renter_Lessor_KM = 0;
                    NewCasRenter.CR_Cas_Renter_Lessor_Days = 0;
                    NewCasRenter.CR_Cas_Renter_Rating = 0;
                    NewCasRenter.CR_Cas_Renter_Lessor_Status = "A";
                    NewCasRenter.CR_Cas_Renter_Membership_Code = "1600000006";
                    NewCasRenter.CR_Cas_Renter_Admin_Membership_Code = "مشترك";
                    elm.PreviousBalance = 0;
                    db.CR_Cas_Renter_Lessor.Add(NewCasRenter);
                    //////////////////////////////////////////////////////////////////
                   
                    //////////////////////////////Save Address into our base/////////////////////////////////////////
                    elm.AddressErrorMessage = "";
                    var adr = db.CR_ELM_Address.FirstOrDefault(a => a.CR_ELM_Ad_Code == id);
                    if (adr != null)
                    {
                       
                        var AdrArea = "";
                        var AdrCity = "";
                        var AdrDistrict = "";
                        var Adrtreet = "";
                        

                        var area = db.CR_Mas_Sup_Regions.FirstOrDefault(a => a.CR_Mas_Sup_Regions_Ar_Name.ToLower().Replace(" ", "").Contains(adr.CR_ELM_Ad_Ar_Area.ToLower().Replace(" ", "")));
                        if (area != null)
                        {
                            elm.OurRegionCode = area.CR_Mas_Sup_Regions_Code;
                        }
                        var City = db.CR_Mas_Sup_City.FirstOrDefault(c => c.CR_Mas_Sup_City_Ar_Name.ToLower().Replace(" ", "").Contains(adr.CR_ELM_Ad_Ar_City.ToLower().Replace(" ", "")));
                        if (City != null)
                        {
                            elm.OurCityCode = City.CR_Mas_Sup_City_Code;
                        }

                        if(adr.CR_ELM_Ad_Ar_Area!=null && adr.CR_ELM_Ad_Ar_Area != "")
                        {
                            AdrArea = adr.CR_ELM_Ad_Ar_Area.Trim();
                        }

                        if (adr.CR_ELM_Ad_Ar_City != null && adr.CR_ELM_Ad_Ar_City != "")
                        {
                            AdrCity = adr.CR_ELM_Ad_Ar_City.Trim();
                        }

                        if (adr.CR_ELM_Ad_Ar_District != null && adr.CR_ELM_Ad_Ar_District != "")
                        {
                            AdrDistrict = adr.CR_ELM_Ad_Ar_District.Trim();
                        }

                        if (adr.CR_ELM_Ad_Ar_Street != null && adr.CR_ELM_Ad_Ar_Street != "")
                        {
                            Adrtreet = adr.CR_ELM_Ad_Ar_Street.Trim();
                        }

                       



                        elm.Address = AdrArea + " - " + AdrCity + " - " + AdrDistrict + " - " + Adrtreet + " - " + adr.CR_ELM_Ad_Bld_No + " - " + "وحدة رقم " + adr.CR_ELM_Ad_Unit_No
                            + " - " + adr.CR_ELM_Ad_Zip_Code + " - " + adr.CR_ELM_Ad_Extra_No;


                        CR_Mas_Address address = new CR_Mas_Address();

                        if (elm != null)
                        {
                            address.CR_Mas_Address_Id_Code = elm.CR_Elm_ID;
                            address.CR_Mas_Address_Regions = elm.OurRegionCode;
                            address.CR_Mas_Address_City = elm.OurCityCode;
                        }

                        address.CR_Mas_Address_Ar_District = adr.CR_ELM_Ad_Ar_District;
                        address.CR_Mas_Address_En_District = adr.CR_ELM_Ad_EN_District;
                        address.CR_Mas_Address_Ar_Street = adr.CR_ELM_Ad_Ar_Street;
                        address.CR_Mas_Address_En_Street = adr.CR_ELM_Ad_En_Street;
                        address.CR_Mas_Address_Building = adr.CR_ELM_Ad_Bld_No;
                        address.CR_Mas_Address_Unit_No = adr.CR_ELM_Ad_Unit_No;
                        address.CR_Mas_Address_Zip_Code = int.Parse(adr.CR_ELM_Ad_Zip_Code);
                        address.CR_Mas_Address_Additional_Numbers = int.Parse(adr.CR_ELM_Ad_Extra_No);

                        address.CR_Mas_Address_UpDate_Post = DateTime.Now;


                        db.CR_Mas_Address.Add(address);
                    }
                    else
                    {
                        if (Session["ContractRenterAddress"].ToString() != "True")
                        {
                            elm.AddressErrorMessage = "العنوان الوطني غير متوفر";
                            elm.PassRenter = false;
                        }
                        else
                        {
                            elm.AddressErrorMessage = "";
                        }
                    }


                    //////////////////////////////////////////////////////////////////////////////////
                    if (operation == "")
                    {

                        //////////////////////////////////////Create Contract///////////////////////////////////
                        DateTime Currentyear = DateTime.Now;
                        var yy = Currentyear.ToString("yy");
                        CR_Cas_Contract_Basic contract = new CR_Cas_Contract_Basic();
                        var Sector = elm.OurSectorCode;
                        var autoinc = GetContractLastRecord(Sector, LessorCode, BranchCode).CR_Cas_Contract_Basic_No;
                        contract.CR_Cas_Contract_Basic_Copy = 1;
                        contract.CR_Cas_Contract_Basic_No = yy + "-" + Sector + "-" + "90" + "-" + LessorCode + "-" + BranchCode + autoinc;
                        contract.CR_Cas_Contract_Basic_Owner_Branch = BranchCode;
                        contract.CR_Cas_Contract_Basic_Year = int.Parse(yy);
                        contract.CR_Cas_Contract_Basic_Type = 90;
                        contract.CR_Cas_Contract_Basic_Lessor = LessorCode;
                        contract.CR_Cas_Contract_Basic_Sector = Sector;
                        contract.CR_Cas_Contract_Basic_Date = DateTime.Now;
                        string currentTime = DateTime.Now.ToString("HH:mm:ss");
                        contract.CR_Cas_Contract_Basic_Time = TimeSpan.Parse(currentTime);
                        contract.CR_Cas_Contract_Basic_Status = "U";
                        if (id != null)
                        {
                            contract.CR_Cas_Contract_Basic_Renter_Id = id;
                        }
                        

                        db.CR_Cas_Contract_Basic.Add(contract);



                        ////////////////////////////////Add Message account/////////////////////////
                        CR_Mas_Account_Msg_Owed MSG = new CR_Mas_Account_Msg_Owed();
                        MSG.CR_Cas_Account_Msg_Owed_Contract_No = contract.CR_Cas_Contract_Basic_No;
                        MSG.CR_Cas_Account_Msg_Owed_Counter = MSGOwedLastRecord(contract.CR_Cas_Contract_Basic_No);
                        MSG.CR_Cas_Account_Msg_Owed_Value = (decimal?)0.5;
                        MSG.CR_Cas_Account_Msg_Owed_Due_Date = DateTime.Now;
                        MSG.CR_Cas_Account_Msg_Owed_Is_Paid = false;
                        MSG.CR_Cas_Account_Msg_Owed_Pay_Date = null;
                        MSG.CR_Cas_Account_Msg_Owed_Pay_No = "";
                        MSG.CR_Cas_Account_Msg_Owed_Contract_Code = "1";

                        db.CR_Mas_Account_Msg_Owed.Add(MSG);


                        /////////////////////////////////Add Data Account///////////////////////////
                        CR_Mas_Account_Data_Owed DOwed = new CR_Mas_Account_Data_Owed();
                        DOwed.CR_Mas_Account_Data_Owed_Contract_No = contract.CR_Cas_Contract_Basic_No;
                        DOwed.CR_Mas_Account_Data_Owed_Counter = DataOwedLastRecord(contract.CR_Cas_Contract_Basic_No);
                        DOwed.CR_Mas_Account_Data_Owed_Value = 3;
                        DOwed.CR_Mas_Account_Data_Owed_Due_Date = DateTime.Now;
                        DOwed.CR_Mas_Account_Data_Owed_Is_Paid = false;
                        DOwed.CR_Mas_Account_Data_Owed_Pay_Date = null;
                        DOwed.CR_Mas_Account_Data_Owed_Pay_No = "";
                        DOwed.CR_Mas_Account_Data_Owed_Contract_Code = "2";

                        db.CR_Mas_Account_Data_Owed.Add(DOwed);




                        ////////////////////////////////Add Post Owed////////////////////////////////////

                        CR_Mas_Account_Post_Owed PostOwed = new CR_Mas_Account_Post_Owed();
                        PostOwed.CR_Mas_Account_Post_Owed_Contract_No = contract.CR_Cas_Contract_Basic_No;
                        PostOwed.CR_Mas_Account_Post_Owed_Counter = PostOwedLastRecord(contract.CR_Cas_Contract_Basic_No);
                        PostOwed.CR_Mas_Account_Post_Owed_Value = 3;
                        PostOwed.CR_Mas_Account_Post_Owed_Due_Date = DateTime.Now;
                        PostOwed.CR_Mas_Account_Post_Owed_Is_Paid = false;
                        PostOwed.CR_Mas_Account_Post_Owed_Pay_Date = null;
                        PostOwed.CR_Mas_Account_Post_Owed_Pay_No = "";
                        PostOwed.CR_Mas_Account_Post_Owed_Contract_Code = "3";

                        db.CR_Mas_Account_Post_Owed.Add(PostOwed);


                        ////////////////////////////////Add Post Owed////////////////////////////////////

                        CR_Mas_Account_License_Owed LOwed = new CR_Mas_Account_License_Owed();
                        LOwed.CR_Mas_Account_License_Owed_Contract_No = contract.CR_Cas_Contract_Basic_No;
                        LOwed.CR_Mas_Account_License_Owed_Counter = LicenseOwedLastRecord(contract.CR_Cas_Contract_Basic_No);
                        LOwed.CR_Mas_Account_License_Owed_Value = 4;
                        LOwed.CR_Mas_Account_License_Owed_Due_Date = DateTime.Now;
                        LOwed.CR_Mas_Account_License_Owed_Is_Paid = false;
                        LOwed.CR_Mas_Account_License_Owed_Pay_Date = null;
                        LOwed.CR_Mas_Account_License_Owed_Pay_No = "";
                        LOwed.CR_Mas_Account_License_Owed_Contract_Code = "5";

                        db.CR_Mas_Account_License_Owed.Add(LOwed);
                        ////////////////////////////////////////////////////////////////////////////////////////

                       
                        elm.TracingNo = contract.CR_Cas_Contract_Basic_No;
                    }

                
                }

                db.SaveChanges();
                return Json(elm, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(null, JsonRequestBehavior.AllowGet);

            }


        }

        public CR_Cas_Contract_Basic GetContractLastRecord(string Sector,string LessorCode,string BranchCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var yy = int.Parse(y);
            var SectorCode = Sector;
            var Lrecord = db.CR_Cas_Contract_Basic.Where(x => x.CR_Cas_Contract_Basic_Lessor == LessorCode &&
                x.CR_Cas_Contract_Basic_Sector == SectorCode
                && x.CR_Cas_Contract_Basic_Year == yy && x.CR_Cas_Contract_Basic_Owner_Branch == BranchCode)
                .Max(x => x.CR_Cas_Contract_Basic_No.Substring(x.CR_Cas_Contract_Basic_No.Length - 4, 4));

            CR_Cas_Contract_Basic c = new CR_Cas_Contract_Basic();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                c.CR_Cas_Contract_Basic_No = val.ToString("0000");
            }
            else
            {
                c.CR_Cas_Contract_Basic_No = "0001";
            }

            return c;
        }

        public CR_Cas_Account_Receipt GetReceiptLastRecord(string LessorCode, string BranchCode)
        {
            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var Lrecord = db.CR_Cas_Account_Receipt.Where(x => x.CR_Cas_Account_Receipt_Lessor_Code == LessorCode
                && x.CR_Cas_Account_Receipt_Year == y && x.CR_Cas_Account_Receipt_Branch_Code == BranchCode)
                .Max(x => x.CR_Cas_Account_Receipt_No.Substring(x.CR_Cas_Account_Receipt_No.Length - 4, 4));

            CR_Cas_Account_Receipt c = new CR_Cas_Account_Receipt();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                c.CR_Cas_Account_Receipt_No = val.ToString("0000");
            }
            else
            {
                c.CR_Cas_Account_Receipt_No = "0001";
            }

            return c;
        }

        public int MSGOwedLastRecord(string No)
        {

            int Max = db.CR_Mas_Account_Msg_Owed.Where(x=>x.CR_Cas_Account_Msg_Owed_Contract_No==No)
                          .Select(x => x.CR_Cas_Account_Msg_Owed_Counter)
                          .DefaultIfEmpty(0)
                          .Max();
            Max = Max + 1;
            return Max;
        }


        public int DataOwedLastRecord(string No)
        {
            int Max = db.CR_Mas_Account_Data_Owed.Where(x => x.CR_Mas_Account_Data_Owed_Contract_No == No)
                          .Select(x => x.CR_Mas_Account_Data_Owed_Counter)
                          .DefaultIfEmpty(0)
                          .Max();
            Max = Max + 1;
            return Max;
        }


        public int PostOwedLastRecord(string No)
        {
            int Max = db.CR_Mas_Account_Post_Owed.Where(x => x.CR_Mas_Account_Post_Owed_Contract_No == No)
                          .Select(x => x.CR_Mas_Account_Post_Owed_Counter)
                          .DefaultIfEmpty(0)
                          .Max();
            Max = Max + 1;
            return Max;
        }


        public int LicenseOwedLastRecord(string No)
        {
            int Max = db.CR_Mas_Account_License_Owed.Where(x => x.CR_Mas_Account_License_Owed_Contract_No == No)
                          .Select(x => x.CR_Mas_Account_License_Owed_Counter)
                          .DefaultIfEmpty(0)
                          .Max();
            Max = Max + 1;
            return Max;
        }
        //public CR_Cas_Contract_Basic GetLastRecord(int ProcedureCode, string sector,string LessorCode)
        //{

        //    DateTime year = DateTime.Now;
        //    var y = int.Parse(year.ToString("yy"));
        //    //var LessorCode = Session["LessorCode"].ToString();

        //    var Lrecord = db.CR_Cas_Contract_Basic.Where(x => x.CR_Cas_Contract_Basic_Lessor == LessorCode &&
        //        x.CR_Cas_Contract_Basic_Type == ProcedureCode
        //        && x.CR_Cas_Contract_Basic_Sector == sector
        //        && x.CR_Cas_Contract_Basic_Year == y)
        //        .Max(x => x.CR_Cas_Contract_Basic_No.Substring(x.CR_Cas_Contract_Basic_No.Length - 4, 4));

        //    CR_Cas_Contract_Basic T = new CR_Cas_Contract_Basic();
        //    if (Lrecord != null)
        //    {
        //        Int64 val = Int64.Parse(Lrecord) + 1;
        //        T.CR_Cas_Contract_Basic_No = val.ToString("0000");
        //    }
        //    else
        //    {
        //        T.CR_Cas_Contract_Basic_No = "0001";
        //    }

        //    return T;
        //}

        public ActionResult Create()
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            ViewBag.ContractDate = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewBag.PayType= new SelectList(db.CR_Mas_Sup_Payment_Method.Where(p=>p.CR_Mas_Sup_Payment_Method_Type=="1" && p.CR_Mas_Sup_Payment_Method_Status=="A")
                , "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Ar_Name");

            ViewBag.CasherName = "";
            //ViewBag.CasherName = new SelectList(db.CR_Cas_Sup_SalesPoint.Where(s=>s.CR_Cas_Sup_SalesPoint_Com_Code==LessorCode &&
            //    s.CR_Cas_Sup_SalesPoint_Brn_Code==BranchCode && s.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status=="A")
            //    , "CR_Cas_Sup_SalesPoint_Code", "CR_Cas_Sup_SalesPoint_Ar_Name");

            return View();
        }

        // POST: BasicContract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "CR_Cas_Contract_Basic_No,CR_Cas_Contract_Basic_Year," +
            "CR_Cas_Contract_Basic_Type,CR_Cas_Contract_Basic_Lessor,CR_Cas_Contract_Basic_Owner_Branch," +
            "CR_Cas_Contract_Basic_Rented_Branch,CR_Cas_Contract_Basic_Date,CR_Cas_Contract_Basic_Time," +
            "CR_Cas_Contract_Basic_Start_Date,CR_Cas_Contract_Basic_Start_Time,CR_Cas_Contract_Basic_Expected_End_Date," +
            "CR_Cas_Contract_Basic_Expected_End_Time,CR_Cas_Contract_Basic_Expected_Rental_Days," +
            "CR_Cas_Contract_Basic_Car_Price_Basic_No,CR_Cas_Contract_Basic_Data_Contract," +
            "CR_Cas_Contract_Basic_Bennan_Contract,CR_Cas_Contract_Basic_Tamm_Contract," +
            "CR_Cas_Contract_Basic_Msg_Contract,CR_Cas_Contract_Basic_Public_Discount," +
            "CR_Cas_Contract_Basic_Special_Discount,CR_Cas_Contract_Basic_Car_Serail_No," +
            "CR_Cas_Contract_Basic_Renter_Id,CR_Cas_Contract_Basic_Sector," +
            "CR_Cas_Contract_Basic_is_Renter_Driver,CR_Cas_Contract_Basic_Driver_Id," +
            "CR_Cas_Contract_Basic_is_Additional_Driver,CR_Cas_Contract_Basic_Additional_Driver_Id," +
            "CR_Cas_Contract_Basic_is_Special_Driver,CR_Cas_Contract_Basic_Special_Driver_Id," +
            "CR_Cas_Contract_Basic_Counter,CR_Cas_Contract_Basic_is_Data_From_Elm," +
            "CR_Cas_Contract_Basic_Confessions_Data,CR_Cas_Contract_Basic_Verification_Code," +
            "CR_Cas_Contract_Basic_Use_Within_Country,CR_Cas_Contract_Basic_Authorization_Code," +
            "CR_Cas_Contract_Basic_Daily_Rent,CR_Cas_Contract_Basic_Weekly_Rent,CR_Cas_Contract_Basic_Monthly_Rent," +
            "CR_Cas_Contract_Basic_Tax_Rate,CR_Cas_Contract_Basic_Daily_Free_KM,CR_Cas_Contract_Basic_Additional_KM_Value," +
            "CR_Cas_Contract_Basic_Free_Additional_Hours,CR_Cas_Contract_Basic_Hour_Max,CR_Cas_Contract_Basic_Extra_Hour_Value," +
            "CR_Cas_Contract_Basic_Receiving_Branch,CR_Cas_Contract_Basic_Actual_End_Date,CR_Cas_Contract_Basic_Actual_End_Time," +
            "CR_Cas_Contract_Basic_Actual_Rental_Days,CR_Cas_Contract_Basic_Total_KM,CR_Cas_Contract_Basic_Delivery_Reading_KM," +
            "CR_Cas_Contract_Basic_Additional_Free_KM,CR_Cas_Contract_Basic_Additional_Hours,CR_Cas_Contract_Basic_Previous_Balance")]
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic , FormCollection collection,
            string CR_Cas_Contract_Basic_Discount_Value,string TotalContractET, string TotalContractTax,string TotalContractIT, string TotalPayed,string TotalContract,
            HttpPostedFileBase img1, HttpPostedFileBase img2, HttpPostedFileBase img3, HttpPostedFileBase img4, HttpPostedFileBase img5, HttpPostedFileBase img6, HttpPostedFileBase img7,
            HttpPostedFileBase img8, HttpPostedFileBase img9,string CurrentMeter, HttpPostedFileBase RenterSignatureImg, string PayType,string CasherName,
            string Reasons,string AuthNo,string CR_Cas_Contract_Basic_Previous_Balance, string RenterReason,string DriverReason,string AdditionalDriverReason,string Price,
            string TotalToPay,string ValueAdditionalDriver,string RenterEmail,string Driver_Email,string Additional_Driver_Email,int CR_Cas_Contract_Basic_Daily_Free_KM,int CR_Cas_Contract_Basic_Free_Additional_Hours)
        {
            if (ModelState.IsValid)
            {
                var LessorCode = "";
                var UserLogin = "";
                var BranchCode = "";
                CR_Cas_Account_Receipt Receipt = new CR_Cas_Account_Receipt();
                CR_Cas_Renter_Lessor RenterLessor = null;
                decimal RenterBalance = 0;                                                           
                try
                {
                    LessorCode = Session["LessorCode"].ToString();
                    BranchCode = Session["BranchCode"].ToString();

                    UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                    if (UserLogin == null || LessorCode == null || BranchCode == null)
                    {
                        RedirectToAction("Account", "Login");
                    }
                }
                catch
                {
                    RedirectToAction("Login", "Account");
                }
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ContractSerialNo = collection["CR_Cas_Contract_Basic_No"];
                        var CarSerialNo = collection["serialNo"];
                        var PriceNo = collection["PriceBasicNo_" + CarSerialNo];

                        //var IsRenterDriver = collection["CR_Cas_Contract_Basic_is_Renter_Driver"];
                        var DriverID = collection["CR_Cas_Contract_Basic_Driver_Id"];                   
                        var RenterID = collection["CR_Cas_Contract_Basic_Renter_Id"];

                        bool IsRenterDriver = false;
                        if (DriverID == RenterID)
                        {
                            IsRenterDriver = true;
                        }
                        var AdditionalDriverId = collection["CR_Cas_Contract_Basic_Additional_Driver_Id"];

                        DateTime StartDate = DateTime.Now;
                        string CurrentTime = DateTime.Now.ToString("HH:mm:ss");
                        var StartTime = TimeSpan.Parse(CurrentTime);

                       

                        var RentalDays = collection["CR_Cas_Contract_Basic_Expected_Rental_Days"];


                        var ExpectedEndDate = collection["CR_Cas_Contract_Basic_Expected_End_Date"];
                        var ExpectedEndDate1 = StartDate.AddDays(Convert.ToDouble(RentalDays));
                        var ExpectedEndTime = TimeSpan.Parse(CurrentTime);


                        bool IsAdditionalDriver = false;
                        if (AdditionalDriverId != "" && AdditionalDriverId != null)
                        {
                            IsAdditionalDriver = true;
                        }


                        var ExAuth = collection["ExAuth"];
                        var ContractDate = collection["CR_Cas_Contract_Basic_Date"];

                        var CarPrice = db.CR_Cas_Car_Price_Basic.FirstOrDefault(price => price.CR_Cas_Car_Price_Basic_No == PriceNo);
                        if (CarPrice != null)
                        {
                            var Contract = db.CR_Cas_Contract_Basic.FirstOrDefault(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode &&
                            c.CR_Cas_Contract_Basic_No == ContractSerialNo && c.CR_Cas_Contract_Basic_Copy == 1);
                            if (Contract != null)
                            {
                                //Contract.CR_Cas_Contract_Basic_No = ContractSerialNo;
                                Contract.CR_Cas_Contract_Basic_Copy = 1;
                                Contract.CR_Cas_Contract_Basic_Driver_Id = DriverID;
                                Contract.CR_Cas_Contract_Basic_Additional_Driver_Id = AdditionalDriverId;
                                Contract.CR_Cas_Contract_Basic_Start_Date = StartDate;
                                Contract.CR_Cas_Contract_Basic_Expected_End_Date = ExpectedEndDate1;
                                Contract.CR_Cas_Contract_Basic_Expected_Rental_Days = int.Parse(RentalDays);
                                Contract.CR_Cas_Contract_Basic_is_Renter_Driver = IsRenterDriver;
                                Contract.CR_Cas_Contract_Basic_is_Additional_Driver = IsAdditionalDriver;
                                Contract.CR_Cas_Contract_Basic_Type = 90;
                                Contract.CR_Cas_Contract_Basic_Lessor = LessorCode;
                                Contract.CR_Cas_Contract_Basic_Owner_Branch = BranchCode;
                                Contract.CR_Cas_Contract_Basic_Car_Serail_No = CarSerialNo;
                                Contract.CR_Cas_Contract_Basic_Additional_Driver_Id = AdditionalDriverId;
                                Contract.CR_Cas_Contract_Basic_Date = DateTime.Parse(ContractDate);
                                Contract.CR_Cas_Contract_Basic_Start_Time = TimeSpan.Parse(CurrentTime);
                                Contract.CR_Cas_Contract_Basic_Expected_End_Time = TimeSpan.Parse(CurrentTime);
                                Contract.CR_Cas_Contract_Basic_Additional_Driver_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_Driver_Value;
                                if (ExAuth == "on")
                                {
                                    Contract.CR_Cas_Contract_Basic_Authorization_Value = CarPrice.CR_Cas_Car_Price_Basic_International_Fees_Tamm;
                                    Contract.CR_Cas_Contract_Basic_Use_Within_Country = true;
                                }
                                else
                                {
                                    Contract.CR_Cas_Contract_Basic_Authorization_Value = CarPrice.CR_Cas_Car_Price_Basic_Internal_Fees_Tamm;
                                    Contract.CR_Cas_Contract_Basic_Use_Within_Country = false;
                                }
                                Contract.CR_Cas_Contract_Basic_Alert_Status = "0";
                                var features = CarPrice.CR_Cas_Car_Price_Features;
                                decimal? feat = 0;
                                if (features!=null)
                                {
                                    foreach (var item in features)
                                    {
                                        feat += item.CR_Cas_Car_Price_Features_Value;
                                    }
                                }
                                Contract.CR_Cas_Contract_Basic_Daily_Rent = CarPrice.CR_Cas_Car_Price_Basic_Daily_Rent + feat;
                                Contract.CR_Cas_Contract_Basic_Weekly_Rent = CarPrice.CR_Cas_Car_Price_Basic_Weekly_Rent + feat;
                                Contract.CR_Cas_Contract_Basic_Monthly_Rent = CarPrice.CR_Cas_Car_Price_Basic_Monthly_Rent + feat;
                                Contract.CR_Cas_Contract_BasicAuthorization_No = AuthNo;

                                decimal TotalAdditional = 0;
                                Boolean IsOtherBranchReceiving = false;
                                var IsReceivingRenter = "0";
                                //var CR_Cas_Contract_Additional = collection["item.CR_Cas_Car_Price_Additional_Code"].Split(',');
                                List<CR_Cas_Contract_Additional> LAdditional = new List<CR_Cas_Contract_Additional>();
                                foreach (string item in collection.AllKeys)
                                {
                                    if (item.StartsWith("chkAdditional_"))
                                    {
                                        CR_Cas_Contract_Additional PriceAdditional = new CR_Cas_Contract_Additional();
                                        PriceAdditional.CR_Cas_Contract_Additional_No = ContractSerialNo;
                                        //PriceAdditional.CR_Cas_Contract_Basic_Copy = 1;

                                        var AdditionalCode = item.Replace("chkAdditional_", "");
                                        if (AdditionalCode != "" && AdditionalCode != null)
                                        {
                                            PriceAdditional.CR_Cas_Contract_Additional_Code = AdditionalCode;
                                        }
                                        if (AdditionalCode == "3500000001")
                                        {
                                            IsOtherBranchReceiving = true;
                                        }

                                        if (AdditionalCode == "3500000003")
                                        {
                                            IsReceivingRenter = "0";
                                        }

                                        var additionalval = collection["ValAdditional_" + AdditionalCode];
                                        if (additionalval != "" && additionalval != null)
                                        {
                                            PriceAdditional.CR_Cas_Contract_Additional_Value = Convert.ToDecimal(additionalval);
                                        }

                                        TotalAdditional = TotalAdditional + Convert.ToDecimal(additionalval);
                                        LAdditional.Add(PriceAdditional);
                                    }

                                }
                                LAdditional.ForEach(add => db.CR_Cas_Contract_Additional.Add(add));
                                Contract.CR_Cas_Contract_Basic_Additional_Value = TotalAdditional;
                                Contract.CR_Cas_Contract_Basic_is_Receiving_Branch = IsOtherBranchReceiving;
                                Contract.CR_Cas_Contract_Basic_is_Receiving_Renter = IsReceivingRenter;
                                Boolean IsKmOpen = false;
                                decimal TotalChoices = 0;



                                List<CR_Cas_Contract_Choices> LChoices = new List<CR_Cas_Contract_Choices>();
                                foreach (string item in collection.AllKeys)
                                {
                                    if (item.StartsWith("chkChoice_"))
                                    {
                                        CR_Cas_Contract_Choices PriceChoices = new CR_Cas_Contract_Choices();
                                        PriceChoices.CR_Cas_Contract_Choices_No = ContractSerialNo;
                                        //PriceChoices.CR_Cas_Contract_Basic_Copy = 1;

                                        PriceChoices.CR_Cas_Contract_Choices_Code = item.Replace("chkChoice_", "");
                                        if (PriceChoices.CR_Cas_Contract_Choices_Code == "3600000001")
                                        {
                                            IsKmOpen = true;
                                        }

                                        var ChoicesVal = collection["ValChoices_" + PriceChoices.CR_Cas_Contract_Choices_Code];
                                        if (ChoicesVal != "" && ChoicesVal != null)
                                        {
                                            PriceChoices.CR_Cas_Contract_Choices_Value = Convert.ToDecimal(ChoicesVal);
                                        }

                                        TotalChoices = TotalChoices + Convert.ToDecimal(ChoicesVal);
                                        LChoices.Add(PriceChoices);
                                    }
                                }
                                LChoices.ForEach(ch => db.CR_Cas_Contract_Choices.Add(ch));
                                Contract.CR_Cas_Contract_Basic_Choices_Value = TotalChoices;
                                ////////////////////////////////////InsertInspection/////////////////////////////////////////
                                List<CR_Cas_Contract_Virtual_Inspection> Linspection = new List<CR_Cas_Contract_Virtual_Inspection>();
                                foreach (string item in collection.AllKeys)
                                {
                                    if (item.StartsWith("ChkInspection_"))
                                    {
                                        CR_Cas_Contract_Virtual_Inspection inspection = new CR_Cas_Contract_Virtual_Inspection();
                                        inspection.CR_Cas_Contract_Virtual_Inspection_No = Contract.CR_Cas_Contract_Basic_No;
                                        inspection.CR_Cas_Contract_Virtual_Inspection_In_Out = 1;

                                        inspection.CR_Cas_Contract_Virtual_Inspection_Code = int.Parse(item.Replace("ChkInspection_", ""));

                                        var InspectionRemark = collection["Remark_" + inspection.CR_Cas_Contract_Virtual_Inspection_Code];
                                        if (InspectionRemark != "" && InspectionRemark != null)
                                        {
                                            inspection.CR_Cas_Contract_Virtual_Inspection_Remarks = InspectionRemark;
                                        }
                                        inspection.CR_Cas_Contract_Virtual_Inspection_Action = true;

                                        Linspection.Add(inspection);
                                    }
                                }
                                Linspection.ForEach(ins => db.CR_Cas_Contract_Virtual_Inspection.Add(ins));
                                //////////////////////////////////////////////////////////////////////////////////////////////



                                Contract.CR_Cas_Contract_Basic_is_Km_Open = IsKmOpen;
                                Contract.CR_Cas_Contract_Basic_Tax_Rate = CarPrice.CR_Cas_Car_Price_Basic_Rental_Tax_Rate;
                                var discount = collection["ContractDiscount"];
                                if (discount != null && discount != "")
                                {
                                    Contract.CR_Cas_Contract_Basic_User_Discount = Convert.ToDecimal(discount);
                                }
                                if (CR_Cas_Contract_Basic_Daily_Free_KM == null)
                                {
                                    CR_Cas_Contract_Basic_Daily_Free_KM = 0;
                                }
                                Contract.CR_Cas_Contract_Basic_Daily_Free_KM = CarPrice.CR_Cas_Car_Price_Basic_No_Daily_Free_KM + CR_Cas_Contract_Basic_Daily_Free_KM;
                                Contract.CR_Cas_Contract_Basic_Additional_KM_Value = CarPrice.CR_Cas_Car_Price_Basic_Additional_KM_Value;
                                if (CR_Cas_Contract_Basic_Free_Additional_Hours == null)
                                {
                                    CR_Cas_Contract_Basic_Free_Additional_Hours = 0;
                                }
                                Contract.CR_Cas_Contract_Basic_Free_Additional_Hours = CarPrice.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours + CR_Cas_Contract_Basic_Free_Additional_Hours;
                                Contract.CR_Cas_Contract_Basic_Hour_Max = CarPrice.CR_Cas_Car_Price_Basic_Hour_Max;
                                Contract.CR_Cas_Contract_Basic_Extra_Hour_Value = CarPrice.CR_Cas_Car_Price_Basic_Extra_Hour_Value;

                                var TotContract = collection["TotalContract"];
                                if(TotalContract!="" && TotalContract != null)
                                {
                                    Contract.CR_Cas_Contract_Basic_Value =Convert.ToDecimal(TotContract);
                                }

                                var DiscountVal = CR_Cas_Contract_Basic_Discount_Value;
                                 if(DiscountVal!="" && DiscountVal != null)
                                {
                                    Contract.CR_Cas_Contract_Basic_Discount_Value = Convert.ToDecimal(DiscountVal);
                                }
                                var TotHT = TotalContractET;
                                if (TotHT != "" && TotHT != null)
                                {
                                    Contract.CR_Cas_Contract_Basic_After_Discount_Value = Convert.ToDecimal(TotHT);
                                }

                                var taxval = TotalContractTax;
                                if (taxval != "")
                                {
                                    Contract.CR_Cas_Contract_Basic_Tax_Value = Convert.ToDecimal(taxval);
                                }

                                var net = TotalContractIT;
                                if (net != "")
                                {
                                    Contract.CR_Cas_Contract_Basic_Net_Value = Convert.ToDecimal(net);
                                }
                                var payed = TotalPayed;
                                if (payed!=""&& payed!=null)
                                {
                                    Contract.CR_Cas_Contract_Basic_Payed_Value = Convert.ToDecimal(payed);

                                }

                                //var payed = TotalPayed;
                                //if (payed != "")
                                //{
                                //    Contract.CR_Cas_Contract_Basic_Payed_Value = Convert.ToDecimal(payed);
                                //    var CasUser = db.CR_Cas_User_Information.FirstOrDefault(u=>u.CR_Cas_User_Information_Id==UserLogin && u.CR_Cas_User_Information_Lessor_Code==LessorCode);
                                //    if (CasUser != null)
                                //    {
                                //        CasUser.CR_Cas_User_Information_Balance += Convert.ToDecimal(payed);
                                //    }
                                //}
                                Contract.CR_Cas_Contract_Basic_Car_Price_Basic_No = PriceNo;
                                Contract.CR_Cas_Contract_Basic_User_Insert = UserLogin;
                                if(CR_Cas_Contract_Basic_Previous_Balance!=null && CR_Cas_Contract_Basic_Previous_Balance != "")
                                {
                                    Contract.CR_Cas_Contract_Basic_Previous_Balance = Convert.ToDecimal(CR_Cas_Contract_Basic_Previous_Balance);
                                }
                                else
                                {
                                    Contract.CR_Cas_Contract_Basic_Previous_Balance = 0;
                                }
                                
                                Contract.CR_Cas_Contract_Basic_Status = "A";

                                ////////////////////////////////alerts/////////////////////////////
                                var currentdate = DateTime.Now;
                                var AuthEndDate = currentdate.AddDays(30);
                                Contract.CR_Cas_Contract_Basic_End_Authorization = AuthEndDate;
                                Contract.CR_Cas_Contract_BasicAuthorization_Staus = "T";


                                DateTime HourDateAlert = (DateTime)ExpectedEndDate1.AddHours(-4);
                                Contract.CR_Cas_Contract_Basic_Hour_DateTime_Alert = HourDateAlert;

                                if (Contract.CR_Cas_Contract_Basic_Expected_Rental_Days > 1)
                                {
                                    var d = ExpectedEndDate1;
                                    Contract.CR_Cas_Contract_Basic_Day_DateTime_Alert = d.AddDays(-1);
                                    
                                }

                                ///////////////////////////////////////////////////////////////////
                                if (TotalPayed != null && TotalPayed != "")
                                {
                                    var PayedValue = decimal.Parse(TotalPayed);
                                    if (PayedValue > 0)
                                    {
                                        /////////////////////////////Sub-main-com-account///////////////////
                                        var MainAccount1 = db.CR_Cas_Sup_Main_Com_Account.FirstOrDefault(ac => ac.CR_Cas_Sup_Main_Com_Account_Com_Code == LessorCode
                                        && ac.CR_Cas_Sup_Main_Com_Account_Main_Code == "1" && ac.CR_Cas_Sup_Main_Com_Account_Status == "A");

                                        MainAccount1.CR_Cas_Sup_Main_Com_Account_Balance += PayedValue;
                                        db.Entry(MainAccount1).State = EntityState.Modified;
                                        

                                        var MainAccount4 = db.CR_Cas_Sup_Main_Com_Account.FirstOrDefault(ac => ac.CR_Cas_Sup_Main_Com_Account_Com_Code == LessorCode
                                        && ac.CR_Cas_Sup_Main_Com_Account_Main_Code == "3" && ac.CR_Cas_Sup_Main_Com_Account_Status == "A");

                                        MainAccount4.CR_Cas_Sup_Main_Com_Account_Balance += PayedValue;
                                        db.Entry(MainAccount4).State = EntityState.Modified;
                                        ///////////////////////////////////////////////////////////////////
                                        ///////////////////////////////CR_Cas_Sup_Main_Brn_Account ///////////////////////
                                        var BrnAccount1 = db.CR_Cas_Sup_Main_Brn_Account.FirstOrDefault(bc => bc.CR_Cas_Sup_Main_Brn_Account_Com_Code == LessorCode 
                                        && bc.CR_Cas_Sup_Main_Brn_Account_Brn_Code == BranchCode
                                        && bc.CR_Cas_Sup_Main_Brn_Account_Main_Code == "1" && bc.CR_Cas_Sup_Main_Brn_Account_Status == "A");

                                        BrnAccount1.CR_Cas_Sup_Main_Brn_Account_Balance += decimal.Parse(TotalPayed);
                                        db.Entry(BrnAccount1).State = EntityState.Modified;

                                        var BrnAccount4 = db.CR_Cas_Sup_Main_Brn_Account.FirstOrDefault(bc => bc.CR_Cas_Sup_Main_Brn_Account_Com_Code == LessorCode
                                        && bc.CR_Cas_Sup_Main_Brn_Account_Brn_Code == BranchCode
                                        && bc.CR_Cas_Sup_Main_Brn_Account_Main_Code == "3" && bc.CR_Cas_Sup_Main_Brn_Account_Status == "A");

                                        BrnAccount4.CR_Cas_Sup_Main_Brn_Account_Balance += decimal.Parse(TotalPayed);
                                        db.Entry(BrnAccount4).State = EntityState.Modified;

                                        //////////////////////////////////////////////////////////////////////////////////
                                        ///////////////////////////////CR_Cas_Sup_Sub_Com_Account//////////////////////////
                                        if (PayType == "10")
                                        {
                                            var SubAccount = db.CR_Cas_Sup_Sub_Com_Account.FirstOrDefault(Sc => Sc.CR_Cas_Sup_Sub_Com_Account_Com_Code == LessorCode
                                            && Sc.CR_Cas_Sup_Sub_Com_Account_Sub_Code == "110"
                                            && Sc.CR_Cas_Sup_Sub_Com_Account_Status == "A");
                                            
                                            SubAccount.CR_Cas_Sup_Sub_Com_Account_Balance += decimal.Parse(TotalPayed);
                                            db.Entry(SubAccount).State = EntityState.Modified;
                                            
                                        }
                                        else
                                        {
                                            var SubAccount = db.CR_Cas_Sup_Sub_Com_Account.FirstOrDefault(Sc => Sc.CR_Cas_Sup_Sub_Com_Account_Com_Code == LessorCode
                                            && Sc.CR_Cas_Sup_Sub_Com_Account_Sub_Code == "111"
                                            && Sc.CR_Cas_Sup_Sub_Com_Account_Status == "A");
                                           
                                            SubAccount.CR_Cas_Sup_Sub_Com_Account_Balance += decimal.Parse(TotalPayed);
                                            db.Entry(SubAccount).State = EntityState.Modified;
                                            
                                        }
                                        var SubAccountObligations = db.CR_Cas_Sup_Sub_Com_Account.FirstOrDefault(Sc => Sc.CR_Cas_Sup_Sub_Com_Account_Com_Code == LessorCode
                                           && Sc.CR_Cas_Sup_Sub_Com_Account_Sub_Code == "330"
                                           && Sc.CR_Cas_Sup_Sub_Com_Account_Status == "A");

                                        SubAccountObligations.CR_Cas_Sup_Sub_Com_Account_Balance += decimal.Parse(TotalPayed);
                                        db.Entry(SubAccountObligations).State = EntityState.Modified;
                                        //////////////////////////////////////////////////////////////////////////////////
                                        ///////////////////////////////CR_Cas_Sup_Sub_Brn_Account//////////////////////////
                                        if (PayType == "10")
                                        {
                                            var SubBrnAccount = db.CR_Cas_Sup_Sub_Brn_Account.FirstOrDefault(Sc => Sc.CR_Cas_Sup_Sub_Brn_Account_Com_Code == LessorCode 
                                            && Sc.CR_Cas_Sup_Sub_Brn_Account_Sub_Code == "110"
                                            && Sc.CR_Cas_Sup_Sub_Brn_Account_Status == "A" && Sc.CR_Cas_Sup_Sub_Brn_Account_Brn_Code == BranchCode);
                                           
                                            SubBrnAccount.CR_Cas_Sup_Sub_Brn_Account_Balance += decimal.Parse(TotalPayed);
                                            db.Entry(SubBrnAccount).State = EntityState.Modified;
                                            
                                        }
                                        else
                                        {
                                            var SubBrnAccount = db.CR_Cas_Sup_Sub_Brn_Account.FirstOrDefault(Sc => Sc.CR_Cas_Sup_Sub_Brn_Account_Com_Code == LessorCode 
                                            && Sc.CR_Cas_Sup_Sub_Brn_Account_Sub_Code == "111"
                                            && Sc.CR_Cas_Sup_Sub_Brn_Account_Status == "A" && Sc.CR_Cas_Sup_Sub_Brn_Account_Brn_Code == BranchCode);
                                            
                                            SubBrnAccount.CR_Cas_Sup_Sub_Brn_Account_Balance += decimal.Parse(TotalPayed);
                                            db.Entry(SubBrnAccount).State = EntityState.Modified;
                                            
                                        }
                                        var SubBrnAccountObligation = db.CR_Cas_Sup_Sub_Brn_Account.FirstOrDefault(Sc => Sc.CR_Cas_Sup_Sub_Brn_Account_Com_Code == LessorCode
                                           && Sc.CR_Cas_Sup_Sub_Brn_Account_Sub_Code == "330"
                                           && Sc.CR_Cas_Sup_Sub_Brn_Account_Status == "A" && Sc.CR_Cas_Sup_Sub_Brn_Account_Brn_Code == BranchCode);

                                        SubBrnAccountObligation.CR_Cas_Sup_Sub_Brn_Account_Balance += decimal.Parse(TotalPayed);
                                        db.Entry(SubBrnAccountObligation).State = EntityState.Modified;
                                        ////////////////////////////////////////////////////////////////////////////////// \
                                        ///\
                                       
                                        /////////////////////////////////////Update Cas Renter Lessor///////////////////////
                                        RenterLessor = db.CR_Cas_Renter_Lessor.FirstOrDefault(r => r.CR_Cas_Renter_Lessor_Id == RenterID && r.CR_Cas_Renter_Lessor_Code==LessorCode);
                                        if (RenterLessor != null)
                                        {
                                            RenterLessor.CR_Cas_Renter_Lessor_Contract_Number += 1;
                                            RenterLessor.CR_Cas_Renter_Lessor_Interaction_Amount_Value += PayedValue;
                                            RenterLessor.CR_Cas_Renter_Lessor_Date_Last_Interaction = DateTime.Now;
                                            if (RenterLessor.CR_Cas_Renter_Lessor_Balance != null)
                                            {
                                                RenterBalance = (decimal)RenterLessor.CR_Cas_Renter_Lessor_Balance;
                                            }
                                            else
                                            {
                                                RenterBalance = 0;
                                            }
                                            RenterLessor.CR_Cas_Renter_Lessor_Balance += PayedValue * (-1);
                                            RenterLessor.CR_Cas_Renter_Lessor_Status = "R";
                                            db.Entry(RenterLessor).State = EntityState.Modified;


                                            ///////////////////////////////Car Renter Statistics/////////////////////////////
                                            Contract.CR_Cas_Contract_Basic_Statistics_Membership_Code = RenterLessor.CR_Cas_Renter_Membership_Code;
                                            /////////////////////////////////////////////////////////////////////////////////
                                        }
                                        //////////////////////////////////////////////////////////////////////////////////
                                        /// 
                                        ////////////////////////////////////CR_Cas_Account_Receipt//////////////////////////
                                        if(PayedValue>0)
                                        {
                                            
                                            DateTime year = DateTime.Now;
                                            var y = year.ToString("yy");

                                            var Sector = "1";
                                            var autoinc = GetReceiptLastRecord(LessorCode, BranchCode).CR_Cas_Account_Receipt_No;
                                            Receipt.CR_Cas_Account_Receipt_No = y + "-" + Sector + "-" + "60" + "-" + LessorCode + "-" + BranchCode + autoinc;
                                            Receipt.CR_Cas_Account_Receipt_Year = y;
                                            Receipt.CR_Cas_Account_Receipt_Type = "60";
                                            Receipt.CR_Cas_Account_Receipt_Lessor_Code = LessorCode;
                                            Receipt.CR_Cas_Account_Receipt_Branch_Code = BranchCode;
                                            Receipt.CR_Cas_Account_Receipt_Date = DateTime.Now;
                                            Receipt.CR_Cas_Account_Receipt_Contract_Operation = Contract.CR_Cas_Contract_Basic_No;
                                            Receipt.CR_Cas_Account_Receipt_Payment = PayedValue;
                                            Receipt.CR_Cas_Account_Receipt_Receipt = 0;
                                            Receipt.CR_Cas_Account_Receipt_Payment_Method = PayType;
                                            /////////////////////////////////Update Sales Point//////////////////////
                                            var salesPoint = db.CR_Cas_Sup_SalesPoint.Single(s => s.CR_Cas_Sup_SalesPoint_Code == CasherName);
                                            if (salesPoint != null)
                                            {
                                                Receipt.CR_Cas_Account_Receipt_SalesPoint_No = CasherName;
                                                Receipt.CR_Cas_Account_Receipt_Bank_Code = salesPoint.CR_Cas_Sup_SalesPoint_Bank_Code;
                                                Receipt.CR_Cas_Account_Receipt_SalesPoint_Previous_Balance = salesPoint.CR_Cas_Sup_SalesPoint_Balance;
                                                salesPoint.CR_Cas_Sup_SalesPoint_Balance += PayedValue;
                                                db.Entry(salesPoint).State= EntityState.Modified;
                                            }
                                            /////////////////////////////////Update Cas User Information//////////////////////
                                            var userinfo = db.CR_Cas_User_Information.Single(u => u.CR_Cas_User_Information_Id == UserLogin);
                                            if(userinfo!=null)
                                            {
                                                if (userinfo.CR_Cas_User_Information_Balance == null)
                                                {
                                                    Receipt.CR_Cas_Account_Receipt_User_Previous_Balance = 0;
                                                }
                                                else
                                                {
                                                    Receipt.CR_Cas_Account_Receipt_User_Previous_Balance = userinfo.CR_Cas_User_Information_Balance;
                                                }
                                                if (userinfo.CR_Cas_User_Information_Balance == null)
                                                {
                                                    userinfo.CR_Cas_User_Information_Balance = PayedValue;
                                                }
                                                else
                                                {
                                                    userinfo.CR_Cas_User_Information_Balance += PayedValue;
                                                }
                                                
                                                db.Entry(userinfo).State = EntityState.Modified;
                                            }
                                            //////////////////////////////////////////////////////////////////////////////////
                                            ///
                                            Receipt.CR_Cas_Account_Receipt_Renter_Code = RenterID;
                                            Receipt.CR_Cas_Account_Receipt_Renter_Previous_Balance = RenterBalance;
                                            Receipt.CR_Cas_Account_Receipt_User_Code = UserLogin;
                                            
                                            Receipt.CR_Cas_Account_Receipt_Is_Passing = "1";
                                            Receipt.CR_Cas_Account_Receipt_Reference_Type = "عقد";
                                            
                                            Receipt.CR_Cas_Account_Receipt_Car_Code = CarSerialNo;
                                            Receipt.CR_Cas_Account_Receipt_Status = "A";
                                            Receipt.CR_Cas_Account_Receipt_Reasons = Reasons;
                                            db.CR_Cas_Account_Receipt.Add(Receipt);
                                        }
                                        
                                        /////////////////////////////////////////////////////////////////////////////////////
                                        ///
                                        /// 

                                        /////////////////////////////////////Update Mas Renter Information///////////////////////
                                        var RenterInfo = db.CR_Mas_Renter_Information.FirstOrDefault(r => r.CR_Mas_Renter_Information_Id == RenterID);
                                        if (RenterInfo != null)
                                        {
                                            RenterInfo.CR_Mas_Renter_Information_Contract_Number += 1;
                                            RenterInfo.CR_Mas_Renter_Information_Value += PayedValue;
                                            RenterInfo.CR_Mas_Renter_Information_Date_Last_Interaction = DateTime.Now;
                                            db.Entry(RenterInfo).State = EntityState.Modified;


                                            ///////////////////////////////////Renter Statistics////////////////////////
                                            Contract.CR_Cas_Contract_Basic_Statistics_Nationalities = RenterInfo.CR_Mas_Renter_Information_Nationality;
                                            var Nationality = db.CR_Mas_Sup_Nationalities.FirstOrDefault(c=>c.CR_Mas_Sup_Nationalities_Code== RenterInfo.CR_Mas_Renter_Information_Nationality);
                                            if(Nationality != null)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Country = Nationality.CR_Mas_Sup_Nationalities_Country_Code;
                                            }
                                            
                                            Contract.CR_Cas_Contract_Basic_Statistics_Gender = RenterInfo.CR_Mas_Renter_Information_Gender;
                                            Contract.CR_Cas_Contract_Basic_Statistics_Jobs = RenterInfo.CR_Mas_Renter_Information_Jobs;
                                            var RenterAddress = db.CR_Mas_Address.FirstOrDefault(a => a.CR_Mas_Address_Id_Code == RenterID);
                                            if (RenterAddress != null)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_City_Renter = RenterAddress.CR_Mas_Address_City;
                                                Contract.CR_Cas_Contract_Basic_Statistics_Regions_Renter = RenterAddress.CR_Mas_Address_Regions;
                                            }
                                            var CurrentDate = DateTime.Today;
                                            var BirthDate = DateTime.Parse(RenterInfo.CR_Mas_Renter_Information_BirthDate.ToString());
                                            var age = (currentdate.Year - BirthDate.Year);
                                            if (age < 25)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Age_No = 1;
                                            }
                                            else if (age >= 25 && age <= 29)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Age_No = 2;
                                            }
                                            else if (age >= 30 && age <= 34)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Age_No = 3;
                                            }
                                            else if (age >= 35 && age <= 39)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Age_No = 4;
                                            }
                                            else if (age >= 40 && age <= 44)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Age_No = 5;
                                            }
                                            else if (age >= 45 && age <= 49)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Age_No = 6;
                                            }
                                            else if (age >= 50 && age <= 54)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Age_No = 7;
                                            }
                                            else if (age >= 55 && age <= 59)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Age_No = 8;
                                            }
                                            else if (age >= 60)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Age_No = 9;
                                            }
                                        }
                                        //////////////////////////////////////////////////////////////////////////////////
                                        /////////////////////////////////////Branch Info Statistics//////////////////////////////////
                                        var branch = db.CR_Cas_Sup_Branch.FirstOrDefault(b=>b.CR_Cas_Sup_Lessor_Code==LessorCode && b.CR_Cas_Sup_Branch_Code==BranchCode);
                                        if (branch != null)
                                        {
                                            var BranchAddress = db.CR_Mas_Address.FirstOrDefault(a=>a.CR_Mas_Address_Id_Code == branch.CR_Cas_Sup_Branch_Government_No);
                                            if (BranchAddress != null)
                                            {
                                                Contract.CR_Cas_Contract_Basic_Statistics_Regions_Branch = BranchAddress.CR_Mas_Address_Regions;
                                                Contract.CR_Cas_Contract_Basic_Statistics_City_Branch = BranchAddress.CR_Mas_Address_City;
                                            }
                                        }
                                        //////////////////////////////////////////////////////////////////////////////////
                                        ////////////////////////////////////////CarInfo Statistics//////////////////////////////////
                                        var Carinfo = db.CR_Cas_Sup_Car_Information.FirstOrDefault(c=>c.CR_Cas_Sup_Car_Serail_No==CarSerialNo);
                                        if(Carinfo!=null)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Brand = Carinfo.CR_Cas_Sup_Car_Brand_Code;
                                            Contract.CR_Cas_Contract_Basic_Statistics_Model = Carinfo.CR_Cas_Sup_Car_Model_Code;
                                            Contract.CR_Cas_Contract_Basic_Statistics_Year = Carinfo.CR_Cas_Sup_Car_Year;
                                        }
                                        /////////////////////////////////////////////////////////////////////////////////////////////
                                        var name = currentdate.DayOfWeek.ToString();
                                        if (name == "Monday")
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_No = 3;
                                        }
                                        if (name == "Tuesday")
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_No = 4;
                                        }
                                        if (name == "Wednesday")
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_No = 5;
                                        }
                                        if (name == "Thursday")
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_No = 6;
                                        }
                                        if (name == "Friday")
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_No = 7;
                                        }
                                        if (name == "Saturday")
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_No = 1;
                                        }
                                        if (name == "Sunday")
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_No = 2;
                                        }

                                        var CH = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));

                                        if (CH >= TimeSpan.Parse("00:00:00") && CH <= TimeSpan.Parse("02:59:00"))
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Time_No = 1;
                                        }
                                        else if (CH >= TimeSpan.Parse("03:00:00") && CH <= TimeSpan.Parse("05:59:00"))
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Time_No = 2;
                                        }
                                        else if (CH >= TimeSpan.Parse("06:00:00") && CH <= TimeSpan.Parse("08:59:00"))
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Time_No = 3;
                                        }
                                        else if (CH >= TimeSpan.Parse("09:00:00") && CH <= TimeSpan.Parse("11:59:00"))
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Time_No = 4;
                                        }
                                        else if (CH >= TimeSpan.Parse("12:00:00") && CH <= TimeSpan.Parse("14:59:00"))
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Time_No = 5;
                                        }
                                        else if (CH >= TimeSpan.Parse("15:00:00") && CH <= TimeSpan.Parse("17:59:00"))
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Time_No = 6;
                                        }
                                        else if (CH >= TimeSpan.Parse("18:00:00") && CH <= TimeSpan.Parse("20:59:00"))
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Time_No = 7;
                                        }
                                        else if (CH >= TimeSpan.Parse("21:00:00") && CH <= TimeSpan.Parse("23:59:00"))
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Time_No = 8;
                                        }

                                    }

                                    if (RentalDays != "")
                                    {
                                        if (Convert.ToInt32(RentalDays) <= 3)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 1;
                                        }
                                        else if (Convert.ToInt32(RentalDays) > 3 && Convert.ToInt32(RentalDays) <= 7)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 2;
                                        }
                                        else if (Convert.ToInt32(RentalDays) >= 8 && Convert.ToInt32(RentalDays) <= 10)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 3;
                                        }
                                        else if (Convert.ToInt32(RentalDays) >= 11 && Convert.ToInt32(RentalDays) <= 15)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 4;
                                        }
                                        else if (Convert.ToInt32(RentalDays) >= 16 && Convert.ToInt32(RentalDays) <= 25)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 5;
                                        }
                                        else if (Convert.ToInt32(RentalDays) >= 26 && Convert.ToInt32(RentalDays) <= 30)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 6;
                                        }
                                        else if (Convert.ToInt32(RentalDays) > 30)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 7;
                                        }
                                    }
                                    if (TotalContractIT != "")
                                    {
                                        if (Convert.ToDecimal(TotalContractIT) < 300)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Value_No = 1;
                                        }
                                        else if (Convert.ToDecimal(TotalContractIT) <= 300 && Convert.ToDecimal(TotalContractIT) <= 500)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Value_No = 2;
                                        }
                                        else if (Convert.ToDecimal(TotalContractIT) <= 701 && Convert.ToDecimal(TotalContractIT) <= 1000)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Value_No = 3;
                                        }
                                        else if (Convert.ToDecimal(TotalContractIT) <= 1201 && Convert.ToDecimal(TotalContractIT) <= 2000)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Value_No = 4;
                                        }
                                        else if (Convert.ToDecimal(TotalContractIT) <= 2001 && Convert.ToDecimal(TotalContractIT) <= 3000)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Value_No = 5;
                                        }
                                        else if (Convert.ToDecimal(TotalContractIT) <= 3001 && Convert.ToDecimal(TotalContractIT) <= 4000)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Value_No = 6;

                                        }
                                        else if (Convert.ToDecimal(TotalContractIT) > 4000)
                                        {
                                            Contract.CR_Cas_Contract_Basic_Statistics_Value_No = 7;
                                        }
                                    }

                                    if (Contract.CR_Cas_Contract_Basic_Daily_Free_KM <= 100)
                                    {
                                        Contract.CR_Cas_Contract_Basic_Statistics_KM = 1;
                                    }
                                    else if (Contract.CR_Cas_Contract_Basic_Daily_Free_KM >= 101 && Contract.CR_Cas_Contract_Basic_Daily_Free_KM <= 200)
                                    {
                                        Contract.CR_Cas_Contract_Basic_Statistics_KM = 2;
                                    }
                                    else if (Contract.CR_Cas_Contract_Basic_Daily_Free_KM >= 201 && Contract.CR_Cas_Contract_Basic_Daily_Free_KM <= 250)
                                    {
                                        Contract.CR_Cas_Contract_Basic_Statistics_KM = 3;
                                    }
                                    else if (Contract.CR_Cas_Contract_Basic_Daily_Free_KM >= 251 && Contract.CR_Cas_Contract_Basic_Daily_Free_KM < 350)
                                    {
                                        Contract.CR_Cas_Contract_Basic_Statistics_KM = 4;
                                    }
                                    else if (Contract.CR_Cas_Contract_Basic_Daily_Free_KM >= 350)
                                    {
                                        Contract.CR_Cas_Contract_Basic_Statistics_KM = 5;
                                    }


                                }
  
                                ///////////////////////////////////////Update Car Status//////////////////////////
                                var car = db.CR_Cas_Sup_Car_Information.FirstOrDefault(c=>c.CR_Cas_Sup_Car_Serail_No==CarSerialNo);
                                car.CR_Cas_Sup_Car_Status = "R";
                                car.CR_Cas_Sup_Car_Last_Contract_Date = DateTime.Now;
                                if (car.CR_Cas_Sup_Car_Statistics_Conract_Count!=null)
                                {
                                    car.CR_Cas_Sup_Car_Statistics_Conract_Count += 1;
                                }
                                else
                                {
                                    car.CR_Cas_Sup_Car_Statistics_Conract_Count = 1;
                                }

                                if (car.CR_Cas_Sup_Car_Statistics_Conract_Days_No != null)
                                {
                                    car.CR_Cas_Sup_Car_Statistics_Conract_Days_No += int.Parse(RentalDays);
                                }
                                else
                                {
                                    car.CR_Cas_Sup_Car_Statistics_Conract_Days_No = int.Parse(RentalDays);
                                }

                                if (car.CR_Cas_Sup_Car_Statistics_Conract_Total_Value != null && TotHT!="")
                                {
                                    car.CR_Cas_Sup_Car_Statistics_Conract_Total_Value += decimal.Parse(TotHT);
                                } 


                                db.Entry(car).State = EntityState.Modified;
                                //////////////////////////////////////////////////////////////////////////////////
                                

                               
                                string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                                string folderRenter = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Renter"));
                                string folderRenterID = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Renter/" + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id));
                                string folderRenterIDID = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Renter/" + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id + "/ID"));
                                string folderRenterLicense = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Renter/" + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id + "/License"));
                                string folderRenterPassport = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Renter/" + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id + "/Passport"));
                                string folderRenterSignature = Server.MapPath(string.Format("~/{0}/", "/images/Bnan/Renter/" + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id + "/Signature"));
                                string FolderContract = Server.MapPath(string.Format("~/{0}/", "/images/Company"));
                                string FolderLessor = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode));
                                string FolderBranch = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode));
                                string FolderContractNo = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo));
                                string OpenPdf = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenPdf"));
                                string CreateCopyFolder = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenPdf"
                                    + "/" +"1"));
                                string OpenImage = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenImage"));
                               
                                if (!Directory.Exists(folderimages))
                                {
                                    Directory.CreateDirectory(folderimages);
                                }
                                if (!Directory.Exists(folderRenter))
                                {
                                    Directory.CreateDirectory(folderRenter);
                                }
                                if (!Directory.Exists(folderRenterID))
                                {
                                    Directory.CreateDirectory(folderRenterID);
                                }
                                if (!Directory.Exists(folderRenterIDID))
                                {
                                    Directory.CreateDirectory(folderRenterIDID);
                                }
                                if (!Directory.Exists(folderRenterPassport))
                                {
                                    Directory.CreateDirectory(folderRenterPassport);
                                }
                                if (!Directory.Exists(folderRenterSignature))
                                {
                                    Directory.CreateDirectory(folderRenterSignature);
                                }
                                if (!Directory.Exists(folderRenterLicense))
                                {
                                    Directory.CreateDirectory(folderRenterLicense);
                                }
                                if (!Directory.Exists(FolderLessor))
                                {
                                    Directory.CreateDirectory(FolderLessor);
                                }
                                if (!Directory.Exists(FolderBranch))
                                {
                                    Directory.CreateDirectory(FolderBranch);
                                }
                                if (!Directory.Exists(FolderContractNo))
                                {
                                    Directory.CreateDirectory(FolderContractNo);
                                }
                                if (!Directory.Exists(OpenPdf))
                                {
                                    Directory.CreateDirectory(OpenPdf);
                                }
                                if (!Directory.Exists(CreateCopyFolder))
                                {
                                    Directory.CreateDirectory(CreateCopyFolder);
                                }
                                if (!Directory.Exists(OpenImage))
                                {
                                    Directory.CreateDirectory(OpenImage);
                                }

                                string img1path = "";
                                if(img1 != null)
                                {
                                    if (img1.FileName.Length > 0)
                                    {
                                        img1path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenImage" + "/" + Path.GetFileName(img1.FileName);
                                        img1.SaveAs(HttpContext.Server.MapPath(img1path));
                                        //cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                else
                                {
                                    img1path = "/images/common/Empty.bmp";
                                }


                                string img2path = "";
                                if (img2 != null)
                                {
                                    if (img2.FileName.Length > 0)
                                    {
                                        img2path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenImage" + "/" + Path.GetFileName(img2.FileName);
                                        img2.SaveAs(HttpContext.Server.MapPath(img2path));
                                        //cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                else
                                {
                                    img2path = "/images/common/Empty.bmp";
                                }

                                string img3path = "";
                                if (img3 != null)
                                {
                                    if (img3.FileName.Length > 0)
                                    {
                                        img3path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenImage" + "/" + Path.GetFileName(img3.FileName);
                                        img3.SaveAs(HttpContext.Server.MapPath(img3path));
                                        //cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                else
                                {
                                    img3path = "/images/common/Empty.bmp";
                                }


                                string img4path = "";
                                if (img4 != null)
                                {
                                    if (img4.FileName.Length > 0)
                                    {
                                        img4path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenImage" + "/" + Path.GetFileName(img4.FileName);
                                        img4.SaveAs(HttpContext.Server.MapPath(img4path));
                                        //cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                else
                                {
                                    img4path = "/images/common/Empty.bmp";
                                }

                                string img5path = "";
                                if (img5 != null)
                                {
                                    if (img5.FileName.Length > 0)
                                    {
                                        img5path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenImage" + "/" + Path.GetFileName(img5.FileName);
                                        img5.SaveAs(HttpContext.Server.MapPath(img5path));
                                        //cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                else
                                {
                                    img5path = "/images/common/Empty.bmp";
                                }

                                string img6path = "";
                                if (img6 != null)
                                {
                                    if (img6.FileName.Length > 0)
                                    {
                                        img6path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenImage" + "/" + Path.GetFileName(img6.FileName);
                                        img6.SaveAs(HttpContext.Server.MapPath(img6path));
                                        //cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                else
                                {
                                    img6path = "/images/common/Empty.bmp";
                                }

                                string img7path = "";
                                if (img7 != null)
                                {
                                    if (img7.FileName.Length > 0)
                                    {
                                        img7path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenImage" + "/" + Path.GetFileName(img7.FileName);
                                        img7.SaveAs(HttpContext.Server.MapPath(img7path));
                                        //cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                else
                                {
                                    img7path = "/images/common/Empty.bmp";
                                }

                                string img8path = "";
                                if (img8 != null)
                                {
                                    if (img8.FileName.Length > 0)
                                    {
                                        img8path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenImage" + "/" + Path.GetFileName(img8.FileName);
                                        img8.SaveAs(HttpContext.Server.MapPath(img8path));
                                        //cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                else
                                {
                                    img8path = "/images/common/Empty.bmp";
                                }

                                string img9path = "";
                                if (img9 != null)
                                {
                                    if (img9.FileName.Length > 0)
                                    {
                                        img9path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + ContractSerialNo + "/" + "OpenImage" + "/" + Path.GetFileName(img9.FileName);
                                        img9.SaveAs(HttpContext.Server.MapPath(img9path));
                                        //cR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Logo = signaturepath;
                                    }
                                }
                                else
                                {
                                    img9path = "/images/common/Empty.bmp";
                                }

                                string RenterSignature = "";
                                if (RenterSignatureImg != null)
                                {
                                    if (RenterSignatureImg.FileName.Length > 0)
                                    {
                                        RenterSignature = "/images/Bnan/Renter/" + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id + "/Signature" + "/" + Path.GetFileName(RenterSignatureImg.FileName);
                                        RenterSignatureImg.SaveAs(HttpContext.Server.MapPath(RenterSignature));
                                        var MasRenterInfo = db.CR_Mas_Renter_Information.FirstOrDefault(r=>r.CR_Mas_Renter_Information_Id==cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id);
                                        if (MasRenterInfo != null)
                                        {
                                            MasRenterInfo.CR_Mas_Renter_Information_Signature = RenterSignature;
                                            db.Entry(MasRenterInfo).State = EntityState.Modified;
                                        }
                                    }
                                }
                                string fname = ContractSerialNo + ".pdf";
                                string fullpath = CreateCopyFolder + fname;
                                //////////////////////////////////////////////////////////////////////////////////
                                Contract.CR_Cas_Contract_Basic_CreateContract_Pdf = fullpath;
                                if(CurrentMeter!=null && CurrentMeter != "")
                                {
                                   Contract.CR_Cas_Contract_Basic_CurrentMeters = decimal.Parse(CurrentMeter);
                                }
                                
                                db.Entry(Contract).State = EntityState.Modified;
                                db.SaveChanges();
                                TempData["TempModel"] = "Saved";
                                dbTran.Commit();
                                SavePDF(Contract.CR_Cas_Contract_Basic_No, CarPrice,fullpath,LessorCode,BranchCode,UserLogin,img1path,img2path,img3path,img4path,img5path,img6path,
                                    img7path,img8path,img9path,RenterBalance,RenterLessor,Receipt.CR_Cas_Account_Receipt_No, Reasons, RenterReason, DriverReason, AdditionalDriverReason,
                                    Price, TotalToPay, ValueAdditionalDriver,PayType,CasherName);


                                var cb = db.CR_Cas_Contract_Basic.FirstOrDefault(l => l.CR_Cas_Contract_Basic_No == Contract.CR_Cas_Contract_Basic_No);
                                SendMail(cb);

                                return RedirectToAction("BranchStat", "BranchHome");
                            }
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                }

            }

            //ViewBag.PayType = new SelectList(db.CR_Mas_Sup_Payment_Method.Where(p => p.CR_Mas_Sup_Payment_Method_Type == "1" && p.CR_Mas_Sup_Payment_Method_Status == "A")
            //    , "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Ar_Name");        
            //ViewBag.CasherName = "";
            return View(cR_Cas_Contract_Basic);
        }







        private void SendMail(CR_Cas_Contract_Basic contract)
        {
            string projectFolder = Server.MapPath(string.Format("~/{0}/", "images"));
            string image1 = Path.Combine(projectFolder, "4.png");
            string image2 = Path.Combine(projectFolder, "3.png");

            Image image = Image.FromFile(image1);
            Image logo = Image.FromFile(image2);

            // Create a graphics object from the image
            Graphics graphics = Graphics.FromImage(image);

            // Set the position where the logo should be placed
            Point logoPosition = new Point(50, 70);
            Size logosize = new Size(110, 60);
            // Define the font and brush for the text\
            Font companyfont = new Font("Segoe UI", 23, FontStyle.Bold);
            Font renterfont = new Font("Segoe UI", 18, FontStyle.Bold);
            Brush companybrush = new SolidBrush(Color.White);
            Font font = new Font("Arial", 16);
            Font carfont = new Font("Arial", 14);
            Brush brush = new SolidBrush(Color.Black);

            // Draw the logo on the image
            graphics.DrawImage(logo, new Rectangle(logoPosition, logosize), 0, 0, logo.Width, logo.Height, GraphicsUnit.Pixel);

            // Draw the text on the image
            string Companyname = contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Ar_Long_Name;
            if (Companyname.Length <= 13)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(1650, 70));
            }
            else if (Companyname.Length <= 20 && Companyname.Length > 13)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(1550, 70));

            }
            else if (Companyname.Length <= 25 && Companyname.Length > 20)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(1450, 70));
            }
            else if (Companyname.Length <= 29 && Companyname.Length > 25)
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(1320, 70));
            }
            else
            {
                graphics.DrawString(Companyname, companyfont, companybrush, new PointF(1200, 70));
            }

            string Rentername = contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Ar_Name.Trim();
            if (Rentername.Length <= 20)
            {
                graphics.DrawString(Rentername, renterfont, companybrush, new PointF(1300, 138));
            }
            else if (Rentername.Length <= 32 && Rentername.Length > 20)
            {
                graphics.DrawString(Rentername, renterfont, companybrush, new PointF(1200, 138));

            }
            else if (Rentername.Length <= 41 && Rentername.Length > 32)
            {
                graphics.DrawString(Rentername, renterfont, companybrush, new PointF(1020, 138));

            }
            else if (Rentername.Length > 41)
            {
                graphics.DrawString(Rentername, renterfont, companybrush, new PointF(800, 138));

            }
            graphics.DrawString(contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_En_Name, renterfont, companybrush, new PointF(210, 138));
            graphics.DrawString(contract.CR_Cas_Contract_Basic_No, font, brush, new PointF(800, 335));
            graphics.DrawString(contract.CR_Cas_Contract_Basic_Start_Date.ToString(), font, brush, new PointF(800, 400));
            graphics.DrawString(contract.CR_Cas_Contract_Basic_Expected_End_Date.ToString(), font, brush, new PointF(800, 470));
            graphics.DrawString(contract.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Collect_Ar_Name.Trim(), carfont, brush, new PointF(675, 535));
            graphics.DrawString(contract.CR_Cas_Contract_Basic_Net_Value.ToString(), font, brush, new PointF(880, 595));

            // Save the modified image back to file or memory stream
            string pathModifiedImage = Server.MapPath(string.Format("~/{0}/", "/images/Modified"));
            if (!Directory.Exists(pathModifiedImage))
            {
                Directory.CreateDirectory(pathModifiedImage);
            }
            string guid = Guid.NewGuid().ToString() + ".png";
            string savedModified = Path.Combine(pathModifiedImage, guid);
            image.Save(savedModified);


            string htmlBody = "<html><body><h1>Contract Summary </h1><br><img src=cid:Contract style='width:100%;height:100%'></body></html>";

            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
               (htmlBody, null, MediaTypeNames.Text.Html);

            LinkedResource inline = new LinkedResource(savedModified, MediaTypeNames.Image.Jpeg);
            inline.ContentId = "Contract";
            avHtml.LinkedResources.Add(inline);

            MailMessage mail = new MailMessage();
            mail.AlternateViews.Add(avHtml);

            Attachment attachment = new Attachment(contract.CR_Cas_Contract_Basic_CreateContract_Pdf);
            mail.Attachments.Add(attachment);

            /*if (contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email != null)
            {
                mail.From = new MailAddress(contract.CR_Mas_Com_Lessor.CR_Mas_Com_Lessor_Email);
            }*/
            mail.From = new MailAddress("Bnanrent@outlook.com");


          /*  if (contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email == null)
            {
                *//*         mail.To.Add(contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email);*//*
            }*/
                mail.To.Add("bnanbnanmail@gmail.com");
            mail.Subject = " Contract Mail ";
            mail.Body = inline.ContentId;

            mail.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("Bnanrent@outlook.com", "bnan123123");

            // Send the message
            smtpClient.Send(mail);
            mail.Dispose();
           System.IO.File.Delete(savedModified);

        }








        ////////public ActionResult PrintContract(string Sno)
        ////////{
        ////////    try
        ////////    {
        ////////        ReportDocument rd = new ReportDocument();
        ////////        rd.Load(Path.Combine(Server.MapPath("~/Reports/ContractBasicReports/ContractCr"), "Cr.rpt"));
        ////////        rd.SetParameterValue("UserName", Session["UserName"].ToString());
        ////////        Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        ////////        stream.Seek(0, SeekOrigin.Begin);
        ////////        rd.Close();
        ////////        return File(stream, "Contract", Sno + ".pdf");

        ////////    }
        ////////    catch
        ////////    {
        ////////        throw;
        ////////    }
        ////////}
        ////////////@if(TempData["printCR"] != null)
        ////////////{
        ////////////    < script >
        ////////////        $(function() {

        ////////////        window.open('@Url.Action("PrintContract", "BasicContract",new {Sno= TempData["printCR"] })')
        ////////////        });
        ////////////    </ script >

        ////////////}

        ////////TempData["printCR"] = Contract.CR_Cas_Contract_Basic_No;



        public ActionResult PDFFlyer(string p)
        {
            if(p!=null && p != "")
            {
                //include the .pdf extention at the end
                string path = p;

                string mime = MimeMapping.GetMimeMapping(path);
                return File(path, mime);

            }
            else
            {
                TempData["printCR"] = "NotFound";
                return RedirectToAction("NotFound", "BasicContract");
            }
            
        }

        public ActionResult NotFound()
        {
            return View();

        }

        private void SavePDF(string Sno, CR_Cas_Car_Price_Basic CarPrice, string fullpath, string LessorCode, string BranchCode, string UserLogin, string img1, string img2, string img3,
           string img4, string img5, string img6, string img7, string img8, string img9, decimal RenterBalance, CR_Cas_Renter_Lessor RenterLessor, string ReceiptNo,
           string Reasons, string RenterReason, string DriverReason, string AdditionalDriverReason, string Price, string TotalToPay, string ValueAdditionalDriver,
           string PayType, string CasherName)
        {
            //try
            //{
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/ContractBasicReports/ContractCr"), "ContractOpen.rpt"));



            List<AdditionalRPTMD> ListAddMD = new List<AdditionalRPTMD>();
            var Additional = db.CR_Cas_Contract_Additional.Where(a => a.CR_Cas_Contract_Additional_No == Sno);
            if (Additional != null)
            {
                var nb = 5 - Additional.Count();
                foreach (var Add in Additional)
                {
                    AdditionalRPTMD AddMd = new AdditionalRPTMD();
                    AddMd.CR_Cas_Contract_Additional_Code = Add.CR_Cas_Contract_Additional_Code;
                    AddMd.CR_Cas_Contract_Additional_No = Add.CR_Cas_Contract_Additional_No;
                    AddMd.CR_Cas_Contract_Additional_Value = (decimal)Add.CR_Cas_Contract_Additional_Value;
                    var AddName = db.CR_Mas_Sup_Additional.FirstOrDefault(a => a.CR_Mas_Sup_Additional_Code == Add.CR_Cas_Contract_Additional_Code);
                    if (AddName != null)
                    {
                        AddMd.CR_Mas_Sup_Additional_Ar_Name = AddName.CR_Mas_Sup_Additional_Ar_Name;
                        AddMd.CR_Mas_Sup_Additional_En_Name = AddName.CR_Mas_Sup_Additional_En_Name;
                        AddMd.CR_Mas_Sup_Additional_Fr_Name = AddName.CR_Mas_Sup_Additional_Fr_Name;
                    }
                    ListAddMD.Add(AddMd);
                }

                if (nb > 0)
                {
                    for (int i = 1; i <= nb; i++)
                    {
                        AdditionalRPTMD AddMd = new AdditionalRPTMD();
                        AddMd.CR_Cas_Contract_Additional_Code = "";
                        AddMd.CR_Cas_Contract_Additional_No = "";
                        AddMd.CR_Cas_Contract_Additional_Value = 0;
                        AddMd.CR_Mas_Sup_Additional_Ar_Name = "";
                        ListAddMD.Add(AddMd);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 5; i++)
                {
                    AdditionalRPTMD AddMd = new AdditionalRPTMD();
                    AddMd.CR_Cas_Contract_Additional_Code = "";
                    AddMd.CR_Cas_Contract_Additional_No = "";
                    AddMd.CR_Cas_Contract_Additional_Value = 0;
                    AddMd.CR_Mas_Sup_Additional_Ar_Name = "";
                    ListAddMD.Add(AddMd);
                }
            }

            rd.Subreports["AddtionalSubReport"].SetDataSource(ListAddMD);


            List<ChoicesRPTMD> ListChoicesMD = new List<ChoicesRPTMD>();
            var Choices = db.CR_Cas_Contract_Choices.Where(a => a.CR_Cas_Contract_Choices_No == Sno);
            if (Choices != null)
            {
                var nb = 5 - Choices.Count();
                foreach (var c in Choices)
                {
                    ChoicesRPTMD ch = new ChoicesRPTMD();
                    ch.CR_Mas_Sup_Choices_Code = c.CR_Cas_Contract_Choices_Code;
                    ch.CR_Mas_Sup_Choices_Ar_Name = c.CR_Cas_Contract_Choices_No;
                    ch.CR_Cas_Car_Price_Choices_Value = (decimal)c.CR_Cas_Contract_Choices_Value;
                    var chName = db.CR_Mas_Sup_Choices.FirstOrDefault(a => a.CR_Mas_Sup_Choices_Code == c.CR_Cas_Contract_Choices_Code);
                    if (chName != null)
                    {
                        ch.CR_Mas_Sup_Choices_Ar_Name = chName.CR_Mas_Sup_Choices_Ar_Name;
                        ch.CR_Mas_Sup_Choices_En_Name = chName.CR_Mas_Sup_Choices_En_Name;
                        ch.CR_Mas_Sup_Choices_Fr_Name = chName.CR_Mas_Sup_Choices_Fr_Name;
                    }
                    ListChoicesMD.Add(ch);
                }
                if (nb > 0)
                {
                    for (int i = 1; i <= nb; i++)
                    {
                        ChoicesRPTMD ch = new ChoicesRPTMD();
                        ch.CR_Mas_Sup_Choices_Code = "";
                        ch.CR_Mas_Sup_Choices_Ar_Name = "";
                        ch.CR_Cas_Car_Price_Choices_Value = 0;
                        ch.CR_Mas_Sup_Choices_Ar_Name = "";
                        ListChoicesMD.Add(ch);
                    }

                }

            }
            else
            {
                for (int i = 1; i <= 5; i++)
                {
                    ChoicesRPTMD ch = new ChoicesRPTMD();
                    ch.CR_Mas_Sup_Choices_Code = "";
                    ch.CR_Mas_Sup_Choices_Ar_Name = "";
                    ch.CR_Cas_Car_Price_Choices_Value = 0;
                    ch.CR_Mas_Sup_Choices_Ar_Name = "";
                    ListChoicesMD.Add(ch);
                }
            }
            rd.Subreports["ChoicesSubReport"].SetDataSource(ListChoicesMD);



            List<VirtualInspectionRptMD> VirtualInspectionMD = new List<VirtualInspectionRptMD>();
            var Vinspection = db.CR_Cas_Contract_Virtual_Inspection.Where(a => a.CR_Cas_Contract_Virtual_Inspection_No == Sno);
            if (Vinspection != null)
            {
                var nb = 15 - Vinspection.Count();
                foreach (var v in Vinspection)
                {
                    VirtualInspectionRptMD VI = new VirtualInspectionRptMD();
                    VI.CR_Cas_Contract_Virtual_Inspection_Code = v.CR_Cas_Contract_Virtual_Inspection_Code;
                    VI.CR_Cas_Contract_Virtual_Inspection_Remarks = v.CR_Cas_Contract_Virtual_Inspection_Remarks;
                    VI.CR_Cas_Contract_Virtual_Inspection_Action = (bool)v.CR_Cas_Contract_Virtual_Inspection_Action;
                    var InsName = db.CR_Mas_Sup_Virtual_Inspection.FirstOrDefault(i => i.CR_Mas_Sup_Virtual_Inspection_Code.ToString() == v.CR_Cas_Contract_Virtual_Inspection_Code.ToString());
                    if (InsName != null)
                    {
                        VI.CR_Mas_Sup_Virtual_Inspection_Ar_Name = InsName.CR_Mas_Sup_Virtual_Inspection_Ar_Name;
                        VI.CR_Mas_Sup_Virtual_Inspection_En_Name = InsName.CR_Mas_Sup_Virtual_Inspection_En_Name;
                        VI.CR_Mas_Sup_Virtual_Inspection_Fr_Name = InsName.CR_Mas_Sup_Virtual_Inspection_Fr_Name;
                    }
                    VirtualInspectionMD.Add(VI);
                }
                if (nb > 0)
                {
                    for (int i = 1; i <= nb; i++)
                    {
                        VirtualInspectionRptMD VI = new VirtualInspectionRptMD();
                        VI.CR_Cas_Contract_Virtual_Inspection_Code = 0;
                        VI.CR_Cas_Contract_Virtual_Inspection_Remarks = "";
                        VI.CR_Cas_Contract_Virtual_Inspection_Action = false;
                        VI.CR_Mas_Sup_Virtual_Inspection_Ar_Name = "";

                        VirtualInspectionMD.Add(VI);
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 15; i++)
                {
                    VirtualInspectionRptMD VI = new VirtualInspectionRptMD();
                    VI.CR_Cas_Contract_Virtual_Inspection_Code = 0;
                    VI.CR_Cas_Contract_Virtual_Inspection_Remarks = "";
                    VI.CR_Cas_Contract_Virtual_Inspection_Action = false;
                    VI.CR_Mas_Sup_Virtual_Inspection_Ar_Name = "";

                    VirtualInspectionMD.Add(VI);
                }
            }
            rd.Subreports["VirtualInspectionSubRPT"].SetDataSource(VirtualInspectionMD);








            var contract = db.CR_Cas_Contract_Basic.FirstOrDefault(c => c.CR_Cas_Contract_Basic_No == Sno);
            if (contract != null)
            {
                var lessor = db.CR_Mas_Com_Lessor.FirstOrDefault(l => l.CR_Mas_Com_Lessor_Code == contract.CR_Cas_Contract_Basic_Lessor);
                if (lessor != null)
                {
                    if (lessor.CR_Mas_Com_Lessor_Ar_Long_Name != null)
                    {
                        rd.SetParameterValue("CompanyName", lessor.CR_Mas_Com_Lessor_Ar_Long_Name.Trim());
                    }
                    if (lessor.CR_Mas_Com_Lessor_En_Long_Name != null)
                    {
                        rd.SetParameterValue("CompanyNameEng", lessor.CR_Mas_Com_Lessor_En_Long_Name.Trim());

                    }
                    if (lessor.CR_Mas_Com_Lessor_Tolk_Free != null)
                    {

                        rd.SetParameterValue("FreeTall", lessor.CR_Mas_Com_Lessor_Tolk_Free.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("FreeTall", "   ");
                    }

                    if (lessor.CR_Mas_Com_Lessor_Contract_Conditions != null)
                    {
                        var Cond1 = lessor.CR_Mas_Com_Lessor_Contract_Conditions;
                        if (Cond1 != "" && Cond1 != null)
                        {
                            Cond1 = Cond1.Replace("~", "");
                            Cond1 = Cond1.Replace("/", "\\");
                            Cond1 = Cond1.Substring(1, Cond1.Length - 1);
                            var Condition1 = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Cond1;
                            rd.SetParameterValue("Cond1", Condition1);
                        }
                        else
                        {
                            rd.SetParameterValue("Cond1", "   ");
                        }
                        
                    }
                    else
                    {
                        rd.SetParameterValue("Cond1", "   ");
                    }

                    if (lessor.CR_Mas_Com_Lessor_Contract_Conditions_No2 != null)
                    {
                        var Cond2 = lessor.CR_Mas_Com_Lessor_Contract_Conditions;
                        if (Cond2 != "" && Cond2 != null)
                        {
                            Cond2 = Cond2.Replace("~", "");
                            Cond2 = Cond2.Replace("/", "\\");
                            Cond2 = Cond2.Substring(1, Cond2.Length - 1);
                            var Condition2 = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Cond2;
                            rd.SetParameterValue("Cond2", Condition2);
                        }
                        else
                        {
                            rd.SetParameterValue("Cond2", "   ");
                        }
                        
                    }
                    else
                    {
                        rd.SetParameterValue("Cond2", "   ");
                    }

                    if (lessor.CR_Mas_Com_Lessor_Signature_Ar_Director_Name != null)
                    {
                        rd.SetParameterValue("DirectorName", lessor.CR_Mas_Com_Lessor_Signature_Ar_Director_Name.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("DirectorName", "   ");
                    }


                    if (ReceiptNo != null && ReceiptNo != "")
                    {
                        rd.SetParameterValue("ReceiptNo", ReceiptNo.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("ReceiptNo", "   ");
                    }


                    if (RenterReason != "")
                    {
                        rd.SetParameterValue("RenterReason", RenterReason);
                    }
                    else
                    {
                        rd.SetParameterValue("RenterReason", "   ");
                    }

                    if (DriverReason != "")
                    {
                        rd.SetParameterValue("DriverReason", DriverReason);
                    }
                    else
                    {
                        rd.SetParameterValue("DriverReason", "   ");
                    }
                    if (AdditionalDriverReason != "")
                    {
                        rd.SetParameterValue("AdditionalDriverReason", DriverReason);
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverReason", "   ");
                    }

                    if (Price != "")
                    {
                        rd.SetParameterValue("Price", Price);
                    }
                    else
                    {
                        rd.SetParameterValue("Price", "   ");
                    }

                    if (TotalToPay != "")
                    {
                        rd.SetParameterValue("TotalToPay", TotalToPay);
                    }
                    else
                    {
                        rd.SetParameterValue("TotalToPay", "   ");
                    }

                    if (Reasons != "")
                    {
                        rd.SetParameterValue("Reasons", Reasons);
                    }
                    else
                    {
                        rd.SetParameterValue("Reasons", "   ");
                    }

                    if (ValueAdditionalDriver != "")
                    {
                        rd.SetParameterValue("ValueAdditionalDriver", ValueAdditionalDriver);
                    }
                    else
                    {
                        rd.SetParameterValue("ValueAdditionalDriver", "   ");
                    }

                    if (contract.CR_Cas_Contract_Basic_Authorization_Value != null)
                    {
                        rd.SetParameterValue("AuthValue", contract.CR_Cas_Contract_Basic_Authorization_Value);
                    }
                    else
                    {
                        rd.SetParameterValue("AuthValue", "   ");
                    }

                    if (PayType != null && PayType != "")
                    {
                        var PayMethod = db.CR_Mas_Sup_Payment_Method.FirstOrDefault(m => m.CR_Mas_Sup_Payment_Method_Code == PayType);
                        if (PayMethod != null)
                        {
                            rd.SetParameterValue("PayMethod", PayMethod.CR_Mas_Sup_Payment_Method_Ar_Name.Trim());
                        }
                        else
                        {
                            rd.SetParameterValue("PayMethod", "    ");
                        }
                    }
                    else
                    {
                        rd.SetParameterValue("PayMethod", "    ");
                    }
                    if (CasherName != null && CasherName != "")
                    {
                        var casher = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(c => c.CR_Cas_Sup_SalesPoint_Code == CasherName);
                        if (casher != null && casher.CR_Cas_Sup_SalesPoint_Bank_Code!=LessorCode+"0000")
                        {
                            rd.SetParameterValue("CasherName", casher.CR_Cas_Sup_SalesPoint_Ar_Name.Trim());
                            var BankName = db.CR_Cas_Sup_Bank.FirstOrDefault(b => b.CR_Cas_Sup_Bank_Code == casher.CR_Cas_Sup_SalesPoint_Bank_Code);
                            if (BankName != null)
                            {
                                rd.SetParameterValue("AccountNo", BankName.CR_Cas_Sup_Bank_Account_No.Trim());
                                rd.SetParameterValue("AccountName", BankName.CR_Cas_Sup_Bank_Ar_Name.Trim());
                                rd.SetParameterValue("BankName", BankName.CR_Mas_Sup_Bank.CR_Mas_Sup_Bank_Ar_Name.Trim());
                            }
                            else
                            {
                                rd.SetParameterValue("AccountName", "      ");
                                rd.SetParameterValue("BankName", "    ");
                                rd.SetParameterValue("AccountNo", "      ");
                            }
                            if (casher.CR_Cas_Sup_SalesPoint_Code != null)
                            {
                                rd.SetParameterValue("CasherCode", casher.CR_Cas_Sup_SalesPoint_Bank_No.Trim());
                            }
                            else
                            {
                                rd.SetParameterValue("CasherCode", "      ");
                            }
                        }
                        else
                        {
                            rd.SetParameterValue("CasherName", "    ");
                            rd.SetParameterValue("AccountName", "      ");
                            rd.SetParameterValue("AccountNo", "      ");
                            rd.SetParameterValue("BankName", "     ");
                            rd.SetParameterValue("CasherCode", "      ");
                        }
                    }
                    else
                    {
                        rd.SetParameterValue("CasherName", "    ");
                        rd.SetParameterValue("AccountName", "      ");
                        rd.SetParameterValue("AccountNo", "      ");
                        rd.SetParameterValue("BankName", "     ");
                        rd.SetParameterValue("CasherCode", "      ");
                    }


                    if (contract.CR_Cas_Contract_Basic_Choices_Value != null)
                    {
                        rd.SetParameterValue("ChoicesTotal", contract.CR_Cas_Contract_Basic_Choices_Value);
                    }
                    else
                    {
                        rd.SetParameterValue("ChoicesTotal", "   ");
                    }

                    if (contract.CR_Cas_Contract_Basic_Additional_Value != null)
                    {
                        rd.SetParameterValue("AdditionalTotal", contract.CR_Cas_Contract_Basic_Additional_Value);
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalTotal", "   ");
                    }

                    var address = db.CR_Mas_Address.FirstOrDefault(adr => adr.CR_Mas_Address_Id_Code == lessor.CR_Mas_Com_Lessor_Government_No);
                    if (address != null)
                    {
                        string street = address.CR_Mas_Address_Ar_Street;
                        string district = address.CR_Mas_Address_Ar_District;
                        //string buildingNo = address.CR_Mas_Address_Building.ToString();
                        //string UnitNo = address.CR_Mas_Address_Unit_No;
                        //string ZipCode = address.CR_Mas_Address_Zip_Code.ToString();
                        string reg = "";
                        string cit = "";
                        var region = db.CR_Mas_Sup_Regions.FirstOrDefault(r => r.CR_Mas_Sup_Regions_Code == address.CR_Mas_Address_Regions);
                        if (region != null)
                        {
                            reg = region.CR_Mas_Sup_Regions_Ar_Name;
                        }
                        var city = db.CR_Mas_Sup_City.FirstOrDefault(c => c.CR_Mas_Sup_City_Code == address.CR_Mas_Address_City);
                        if (city != null)
                        {
                            cit = city.CR_Mas_Sup_City_Ar_Name;
                        }

                        //string addr = reg + " / " + cit + " حي " + district + " شارع " + street + " مبنى رقم " + buildingNo + " وحدة رقم " + UnitNo + " الرمز البريدي " + ZipCode;
                        string addr = reg + " / " + cit + " حي " + district + " شارع " + street;

                        if (addr != null && addr != "")
                        {
                            rd.SetParameterValue("CompanyAddress", addr.Trim());
                        }

                    }
                    if (lessor.CR_Mas_Com_Lessor_Commercial_Registration_No != null)
                    {
                        rd.SetParameterValue("CommercialRegisterNo", lessor.CR_Mas_Com_Lessor_Commercial_Registration_No.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("CommercialRegisterNo", "        ");
                    }
                    if (lessor.CR_Mas_Com_Lessor_Tax_Number != null)
                    {
                        rd.SetParameterValue("TaxNumber", lessor.CR_Mas_Com_Lessor_Tax_Number.Trim());
                    }

                    var branch = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Code == BranchCode);
                    if (branch != null)
                    {
                        var BranchAddress = db.CR_Mas_Address.FirstOrDefault(adr => adr.CR_Mas_Address_Id_Code == branch.CR_Cas_Sup_Branch_Government_No);
                        if (BranchAddress != null)
                        {
                            string street = BranchAddress.CR_Mas_Address_Ar_Street;
                            string district = BranchAddress.CR_Mas_Address_Ar_District;
                            string reg = "";
                            string cit = "";
                            var region = db.CR_Mas_Sup_Regions.FirstOrDefault(r => r.CR_Mas_Sup_Regions_Code == BranchAddress.CR_Mas_Address_Regions);
                            if (region != null)
                            {
                                reg = region.CR_Mas_Sup_Regions_Ar_Name;
                            }
                            var city = db.CR_Mas_Sup_City.FirstOrDefault(c => c.CR_Mas_Sup_City_Code == BranchAddress.CR_Mas_Address_City);
                            if (city != null)
                            {
                                cit = city.CR_Mas_Sup_City_Ar_Name;
                            }
                            //string BrAdr = reg + " / " + cit + " حي " + district + " شارع " + street + " مبنى رقم " + buildingNo + " وحدة رقم " + UnitNo + " الرمز البريدي " + ZipCode;
                            string BrAdr = reg + " / " + cit + " حي " + district + " شارع " + street;
                            if (BrAdr != null && BrAdr != "")
                            {
                                rd.SetParameterValue("BranchAddress", BrAdr.Trim());
                            }
                        }
                        if (branch.CR_Cas_Sup_Branch_Ar_Name != null)
                        {
                            rd.SetParameterValue("BranchName", branch.CR_Cas_Sup_Branch_Ar_Name.Trim());
                        }
                        if (branch.CR_Cas_Sup_Branch_Tel != null)
                        {
                            rd.SetParameterValue("BranchContact", branch.CR_Cas_Sup_Branch_Tel.Trim());
                        }

                        if (branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name != null)
                        {
                            rd.SetParameterValue("BranchDirectorName", branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name.Trim());
                        }
                        else
                        {
                            rd.SetParameterValue("BranchDirectorName", "    ");
                        }

                        var BranchDirectorSignature = branch.CR_Cas_Sup_Branch_Signature_Director;
                        if (BranchDirectorSignature != "" && BranchDirectorSignature != null)
                        {
                            var BDsignature = BranchDirectorSignature.Replace("~", "");
                            BDsignature = BDsignature.Replace("/", "\\");
                            BDsignature = BDsignature.Substring(1, BDsignature.Length - 1);
                            var BDS = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + BDsignature;
                            rd.SetParameterValue("BranchDirectorSignature", BDS);
                        }
                        else
                        {
                            rd.SetParameterValue("BranchDirectorSignature", "    ");
                        }

                        var BranchDocs = db.CR_Cas_Sup_Branch_Documentation.FirstOrDefault(d=>d.CR_Cas_Sup_Branch_Documentation_Lessor_Code==LessorCode &&
                        d.CR_Cas_Sup_Branch_Documentation_Branch_Code==BranchCode);
                        if (BranchDocs != null)
                        {
                            if (BranchDocs.CR_Cas_Sup_Branch_Documentation_No != null)
                            {
                                rd.SetParameterValue("CompanyAuthNo", BranchDocs.CR_Cas_Sup_Branch_Documentation_No.Trim());
                            }
                            else
                            {
                                rd.SetParameterValue("CompanyAuthNo", "      ");
                            }
                        }

                    }

                    var car = db.CR_Cas_Sup_Car_Information.FirstOrDefault(c => c.CR_Cas_Sup_Car_Serail_No == contract.CR_Cas_Contract_Basic_Car_Serail_No);
                    if (car != null)
                    {
                        if (car.CR_Cas_Sup_Car_Serail_No != null)
                        {
                            rd.SetParameterValue("CarSerialNo", car.CR_Cas_Sup_Car_Serail_No.Trim());
                        }
                        else
                        {
                            rd.SetParameterValue("CarSerialNo", "    ");
                        }
                        if (car.CR_Cas_Sup_Car_No_Current_Meter != null)
                        {
                            rd.SetParameterValue("CurrentKm", car.CR_Cas_Sup_Car_No_Current_Meter + "كم");
                        }
                        else
                        {
                            rd.SetParameterValue("CurrentKm", "    ");
                        }


                        var CarName = car.CR_Cas_Sup_Car_Collect_Ar_Name + " " +
                            car.CR_Mas_Sup_Category_Car.CR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name.Trim();
                        rd.SetParameterValue("CarName", CarName);

                        var cat = db.CR_Mas_Sup_Category_Car.FirstOrDefault(c => c.CR_Mas_Sup_Category_Model_Code == car.CR_Cas_Sup_Car_Model_Code && c.CR_Mas_Sup_Category_Car_Year == car.CR_Cas_Sup_Car_Year);
                        if (cat != null)
                        {
                            if (cat.CR_Mas_Sup_Category_Car_Passengers_No != null)
                            {
                                rd.SetParameterValue("CarPassengersNo", cat.CR_Mas_Sup_Category_Car_Passengers_No);
                            }
                            else
                            {
                                rd.SetParameterValue("CarPassengersNo", "  ");
                            }
                            if (cat.CR_Mas_Sup_Category_Car_Door_No != null)
                            {
                                rd.SetParameterValue("CarDoorNo", cat.CR_Mas_Sup_Category_Car_Door_No);
                            }
                            else
                            {
                                rd.SetParameterValue("CarDoorNo", "  ");
                            }
                            if (cat.CR_Mas_Sup_Category_Car_Bag_Bags != null)
                            {
                                rd.SetParameterValue("CarBigBagsNo", cat.CR_Mas_Sup_Category_Car_Bag_Bags);
                            }
                            else
                            {
                                rd.SetParameterValue("CarBigBagsNo", "  ");
                            }
                            if (cat.CR_Mas_Sup_Category_Car_Small_Bags != null)
                            {
                                rd.SetParameterValue("CarSmallBagsNo", cat.CR_Mas_Sup_Category_Car_Small_Bags);
                            }
                            else
                            {
                                rd.SetParameterValue("CarSmallBagsNo", "  ");
                            }

                        }

                        var cardocs = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Serail_No == car.CR_Cas_Sup_Car_Serail_No);
                        if (cardocs != null)
                        {
                            foreach (var docs in cardocs)
                            {

                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "29")
                                {
                                    rd.SetParameterValue("CarInspectionEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date);
                                }
                                else
                                {
                                    rd.SetParameterValue("CarInspectionEndDate", "    ");
                                }
                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "27")
                                {
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_No != null)
                                    {
                                        rd.SetParameterValue("InsuranceDocNo", docs.CR_Cas_Sup_Car_Doc_Mainten_No.Trim());
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("InsuranceDocNo", "    ");
                                    }
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                                    {
                                        rd.SetParameterValue("InsuranceEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("InsuranceEndDate", "   ");
                                    }
                                }

                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "26")
                                {
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                                    {
                                        rd.SetParameterValue("CarDrivingLicenceEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("CarDrivingLicenceEndDate", "    ");
                                    }
                                }


                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "36")
                                {
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_KM != null)
                                    {
                                        rd.SetParameterValue("CurrentWheelKm", docs.CR_Cas_Sup_Car_Doc_Mainten_End_KM);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("CurrentWheelKm", "    ");
                                    }
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                                    {
                                        rd.SetParameterValue("CurrentWheelKmEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("CurrentWheelKmEndDate", "    ");
                                    }

                                }

                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "37")
                                {
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_KM != null)
                                    {
                                        rd.SetParameterValue("CurrentOilKm", docs.CR_Cas_Sup_Car_Doc_Mainten_End_KM);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("CurrentOilKm", "    ");
                                    }
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                                    {
                                        rd.SetParameterValue("CurrentOilKmEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("CurrentOilKmEndDate", "    ");
                                    }
                                }
                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "28")
                                {
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_No != null)
                                    {
                                        rd.SetParameterValue("TrafficPermit", docs.CR_Cas_Sup_Car_Doc_Mainten_No);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("TrafficPermit", "     ");
                                    }
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                                    {
                                        rd.SetParameterValue("TrafficPermitEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("TrafficPermitEndDate", "     ");
                                    }
                                }
                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "38")
                                {
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                                    {
                                        rd.SetParameterValue("PeriodicMaintenanceEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date);
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("PeriodicMaintenanceEndDate", "     ");
                                    }

                                }

                            }
                        }
                    }

                    var LessorDirectorSignature = lessor.CR_Mas_Com_Lessor_Signature_Director;
                    if (LessorDirectorSignature != "" && LessorDirectorSignature != null)
                    {
                        var LDsignature = LessorDirectorSignature.Replace("~", "");
                        LDsignature = LDsignature.Replace("/", "\\");
                        LDsignature = LDsignature.Substring(1, LDsignature.Length - 1);
                        var LDS = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + LDsignature;
                        rd.SetParameterValue("LessorDirectorSignature", LDS);
                    }
                    else
                    {
                        rd.SetParameterValue("LessorDirectorSignature", "    ");
                    }



                    if (UserLogin != null && UserLogin != "")
                    {
                        UserLogin = UserLogin.Trim();
                    }
                    var user = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == UserLogin);
                    if (user != null)
                    {
                        var UserSignature = user.CR_Cas_User_Information_Signature;
                        if (UserSignature != "" && UserSignature != null)
                        {
                            var Usersignature = UserSignature.Replace("~", "");
                            Usersignature = Usersignature.Replace("/", "\\");
                            Usersignature = Usersignature.Substring(1, Usersignature.Length - 1);
                            var UserS = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Usersignature;
                            rd.SetParameterValue("UserSignature", UserS);
                        }
                        else
                        {
                            rd.SetParameterValue("UserSignature", " ");
                        }
                        if (user.CR_Cas_User_Information_Ar_Name != null)
                        {
                            rd.SetParameterValue("UserName", user.CR_Cas_User_Information_Ar_Name.Trim());
                        }
                        else
                        {
                            rd.SetParameterValue("UserName", " ");
                        }
                    }
                    else
                    {
                        rd.SetParameterValue("UserSignature", " ");
                        rd.SetParameterValue("UserName", " ");
                    }
                }

                var renter = db.CR_Mas_Renter_Information.FirstOrDefault(r => r.CR_Mas_Renter_Information_Id == contract.CR_Cas_Contract_Basic_Renter_Id);
                if (renter != null)
                {
                    rd.SetParameterValue("RenterId", contract.CR_Cas_Contract_Basic_Renter_Id.Trim());
                    rd.SetParameterValue("RenterName", renter.CR_Mas_Renter_Information_Ar_Name.Trim());
                    string RenterRegion = "";
                    string RenterCity = "";
                    string RenterDistrict = "";
                    string RenterStreet = "";

                    var RenterAddress = db.CR_Mas_Address.FirstOrDefault(addr => addr.CR_Mas_Address_Id_Code == renter.CR_Mas_Renter_Information_Id);
                    if (RenterAddress != null)
                    {
                        RenterRegion = RenterAddress.CR_Mas_Sup_Regions.CR_Mas_Sup_Regions_Ar_Name.Trim();
                        RenterCity = RenterAddress.CR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name.Trim();
                        RenterDistrict = RenterAddress.CR_Mas_Address_Ar_District.Trim();
                        RenterStreet = RenterAddress.CR_Mas_Address_Ar_Street.Trim();

                        //string RenterAdr = RenterRegion + " " + RenterCity + " " + RenterDistrict + " " + RenterStreet;
                        string RenterAdr = RenterRegion + " / " + RenterCity + " حي " + RenterDistrict + " شارع " + RenterStreet;
                        rd.SetParameterValue("RenterAdr", RenterAdr.Trim());
                    }
                    else
                    {
                        string RenterAdr = "    ";
                        rd.SetParameterValue("RenterAdr", RenterAdr.Trim());
                    }
                    if (renter.CR_Mas_Renter_Information_Tax_No != null)
                    {
                        rd.SetParameterValue("RenterTaxNo", renter.CR_Mas_Renter_Information_Tax_No.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("RenterTaxNo", "");
                    }

                    if (renter.CR_Mas_Renter_Information_Email != null)
                    {
                        rd.SetParameterValue("RenterEmail", renter.CR_Mas_Renter_Information_Email.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("RenterEmail", "");
                    }

                }


                /////////////////////////////////////////////////////////////////Driver Info////////////////////////////////////////////////////////////
                var Driver = db.CR_Mas_Renter_Information.FirstOrDefault(r => r.CR_Mas_Renter_Information_Id == contract.CR_Cas_Contract_Basic_Driver_Id);
                if (Driver != null)
                {
                    rd.SetParameterValue("DriverId", contract.CR_Cas_Contract_Basic_Driver_Id.Trim());
                    rd.SetParameterValue("DriverName", Driver.CR_Mas_Renter_Information_Ar_Name.Trim());
                    var nationality = Driver.CR_Mas_Sup_Nationalities.CR_Mas_Sup_Nationalities_Ar_Name;
                    if (nationality != null)
                    {
                        rd.SetParameterValue("DriverNationality", nationality.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("DriverNationality", "     ");
                    }
                    var workplace = db.CR_Mas_Sup_Employer.FirstOrDefault(emp => emp.CR_Mas_Sup_Employer_Code == Driver.CR_Mas_Renter_Information_Workplace_Subscription);
                    if (workplace != null)
                    {
                        rd.SetParameterValue("DriverWorkPlace", workplace.CR_Mas_Sup_Employer_Ar_Name.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("DriverWorkPlace", "     ");
                    }
                    var job = db.CR_Mas_Sup_Jobs.FirstOrDefault(emp => emp.CR_Mas_Sup_Jobs_Code == Driver.CR_Mas_Renter_Information_Jobs);
                    if (workplace != null)
                    {
                        rd.SetParameterValue("DriverJob", job.CR_Mas_Sup_Jobs_Ar_Name.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("DriverJob", "     ");
                    }
                    if (Driver.CR_Mas_Renter_Information_Email != null)
                    {
                        rd.SetParameterValue("DriverEmail", Driver.CR_Mas_Renter_Information_Email.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("DriverEmail", "     ");
                    }

                    //////////////////////////////Driver birthdate///////////////////////////////
                    if (Driver.CR_Mas_Renter_Information_BirthDate != null)
                    {
                        rd.SetParameterValue("DriverBirthDate", Driver.CR_Mas_Renter_Information_BirthDate);
                    }
                    else
                    {
                        rd.SetParameterValue("DriverBirthDate", "     ");
                    }
                    //////////////////////////////Driver Membership///////////////////////////////
                    if (Driver.CR_Mas_Renter_Information_Membership != null)
                    {
                        rd.SetParameterValue("DriverMembership", Driver.CR_Mas_Renter_Information_Membership.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("DriverMembership", "     ");
                    }
                    //////////////////////////////////////////////////////////////////////////////

                    string DriverRegion = "";
                    string DriverCity = "";
                    string DriverDistrict = "";
                    string DriverStreet = "";
                    var DriverAddress = db.CR_Mas_Address.FirstOrDefault(addr => addr.CR_Mas_Address_Id_Code == Driver.CR_Mas_Renter_Information_Id);
                    if (DriverAddress != null)
                    {
                        DriverRegion = DriverAddress.CR_Mas_Sup_Regions.CR_Mas_Sup_Regions_Ar_Name.Trim();
                        DriverCity = DriverAddress.CR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name.Trim();
                        DriverDistrict = DriverAddress.CR_Mas_Address_Ar_District.Trim();
                        DriverStreet = DriverAddress.CR_Mas_Address_Ar_Street.Trim();
                        //string DriverAdr = DriverRegion + " " + DriverCity + " " + DriverDistrict + " " + DriverStreet;
                        string DriverAdr = DriverRegion + " / " + DriverCity + " حي " + DriverDistrict + " شارع " + DriverStreet;
                        rd.SetParameterValue("DriverAdr", DriverAdr.Trim());
                    }
                    else
                    {
                        string DriverAdr = "     ";
                        rd.SetParameterValue("DriverAdr", DriverAdr);
                    }


                    var gender = db.CR_Mas_Sup_Gender.FirstOrDefault(g => g.CR_Mas_Sup_Gender_Code == Driver.CR_Mas_Renter_Information_Gender);
                    if (gender != null)
                    {
                        rd.SetParameterValue("DriverGender", gender.CR_Mas_Sup_Gender_Ar_Name.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("DriverGender", "     ");
                    }

                    //var job = db.CR_Mas_Sup_Jobs.FirstOrDefault(j => j.CR_Mas_Sup_Jobs_Code == Driver.CR_Mas_Renter_Information_Jobs);
                    //if (job != null)
                    //{
                    //    rd.SetParameterValue("DriverJob", job.CR_Mas_Sup_Jobs_Ar_Name);
                    //}
                    //else
                    //{
                    //    rd.SetParameterValue("DriverJob", "");
                    //}
                    if (Driver.CR_Mas_Renter_Information_Expiry_Driving_License_Date != null)
                    {
                        rd.SetParameterValue("DriverDrivingLicenceEndDate", string.Format("{0:yyyy-MM-dd}", Driver.CR_Mas_Renter_Information_Expiry_Driving_License_Date));
                    }
                    else
                    {
                        rd.SetParameterValue("DriverDrivingLicenceEndDate","      ");
                    }

                }
                /////////////////////////////////////////////////////////////////Additional Driver Info////////////////////////////////////////////////////////////
                var AdditionalDriver = db.CR_Mas_Renter_Information.FirstOrDefault(r => r.CR_Mas_Renter_Information_Id == contract.CR_Cas_Contract_Basic_Additional_Driver_Id);
                if (AdditionalDriver != null)
                {
                    if (contract.CR_Cas_Contract_Basic_Additional_Driver_Id != null)
                    {
                        rd.SetParameterValue("AdditionalDriverId", contract.CR_Cas_Contract_Basic_Additional_Driver_Id.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverId", "     ");
                    }

                    if (AdditionalDriver.CR_Mas_Renter_Information_Ar_Name != null)
                    {
                        rd.SetParameterValue("AdditionalDriverName", AdditionalDriver.CR_Mas_Renter_Information_Ar_Name.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverName", "     ");
                    }

                    var nationality = db.CR_Mas_Sup_Nationalities.FirstOrDefault(n => n.CR_Mas_Sup_Nationalities_Code == AdditionalDriver.CR_Mas_Renter_Information_Nationality);
                    if (nationality != null)
                    {
                        rd.SetParameterValue("AdditionalDriverNationality", nationality.CR_Mas_Sup_Nationalities_Ar_Name.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverNationality", "     ");
                    }
                    var workplace = db.CR_Mas_Sup_Employer.FirstOrDefault(emp => emp.CR_Mas_Sup_Employer_Code == AdditionalDriver.CR_Mas_Renter_Information_Workplace_Subscription);
                    if (workplace != null)
                    {
                        rd.SetParameterValue("AdditionalDriverWorkPlace", workplace.CR_Mas_Sup_Employer_Ar_Name.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverWorkPlace", "      ");
                    }

                    var AdditionalDriverMembership = db.CR_Mas_Sup_Membership.FirstOrDefault(m => m.CR_Mas_Sup_Membership_Code == AdditionalDriver.CR_Mas_Renter_Information_Membership);
                    if (AdditionalDriverMembership != null)
                    {
                        rd.SetParameterValue("AdditionalDriverMembership", AdditionalDriverMembership.CR_Mas_Sup_Membership_Ar_Name.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverMembership", "      ");
                    }

                    if (AdditionalDriver.CR_Mas_Renter_Information_BirthDate != null)
                    {
                        rd.SetParameterValue("AdditionalDriverBirthDate", AdditionalDriver.CR_Mas_Renter_Information_BirthDate);
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverBirthDate", "      ");
                    }

                    var AdditionalDriverJob = db.CR_Mas_Sup_Jobs.FirstOrDefault(j => j.CR_Mas_Sup_Jobs_Code == AdditionalDriver.CR_Mas_Renter_Information_Jobs);
                    if (AdditionalDriverJob != null)
                    {
                        rd.SetParameterValue("AdditionalDriverJob", AdditionalDriverJob.CR_Mas_Sup_Jobs_Ar_Name.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverJob", "     ");
                    }

                    if (AdditionalDriver.CR_Mas_Renter_Information_Email != null)
                    {
                        rd.SetParameterValue("AdditionalDriverEmail", AdditionalDriver.CR_Mas_Renter_Information_Email);
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverEmail", "     ");
                    }

                    string AdditionalDriverRegion = "";
                    string AdditionalDriverCity = "";
                    string AdditionalDriverDistrict = "";
                    string AdditionalDriverStreet = "";

                    var AdditionalDriverAddress = db.CR_Mas_Address.FirstOrDefault(addr => addr.CR_Mas_Address_Id_Code == AdditionalDriver.CR_Mas_Renter_Information_Id);
                    if (AdditionalDriverAddress != null)
                    {
                        AdditionalDriverRegion = AdditionalDriverAddress.CR_Mas_Sup_Regions.CR_Mas_Sup_Regions_Ar_Name.Trim();
                        AdditionalDriverCity = AdditionalDriverAddress.CR_Mas_Sup_City.CR_Mas_Sup_City_Ar_Name.Trim();
                        AdditionalDriverDistrict = AdditionalDriverAddress.CR_Mas_Address_Ar_District.Trim();
                        AdditionalDriverStreet = AdditionalDriverAddress.CR_Mas_Address_Ar_Street.Trim();

                        //string AdditionalDriverAdr = AdditionalDriverRegion + " " + AdditionalDriverCity + " " + AdditionalDriverDistrict + " " +AdditionalDriverStreet;
                        string AdditionalDriverAdr = AdditionalDriverRegion + " / " + AdditionalDriverCity + " حي " + AdditionalDriverDistrict + " شارع " + AdditionalDriverStreet;
                        rd.SetParameterValue("AdditionalDriverAdr", AdditionalDriverAdr.Trim());
                    }
                    else
                    {
                        string AdditionalDriverAdr = "     ";
                        rd.SetParameterValue("AdditionalDriverAdr", AdditionalDriverAdr);
                    }


                    var gender = db.CR_Mas_Sup_Gender.FirstOrDefault(g => g.CR_Mas_Sup_Gender_Code == AdditionalDriver.CR_Mas_Renter_Information_Gender);
                    if (gender != null)
                    {
                        rd.SetParameterValue("AdditionalDriverGender", gender.CR_Mas_Sup_Gender_Ar_Name);
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverGender", "      ");
                    }

                    //var job = db.CR_Mas_Sup_Jobs.FirstOrDefault(j => j.CR_Mas_Sup_Jobs_Code == AdditionalDriver.CR_Mas_Renter_Information_Jobs);
                    //if (job != null)
                    //{
                    //    rd.SetParameterValue("AdditionalDriverJob", job.CR_Mas_Sup_Jobs_Ar_Name);
                    //}
                    //else
                    //{
                    //    rd.SetParameterValue("AdditionalDriverJob", "     ");
                    //}

                    if (AdditionalDriver.CR_Mas_Renter_Information_Expiry_Driving_License_Date != null)
                    {
                        rd.SetParameterValue("AdditionalDriverDrivingLicenceEndDate", string.Format("{0:yyyy-MM-dd}", AdditionalDriver.CR_Mas_Renter_Information_Expiry_Driving_License_Date));
                    }
                    else
                    {
                        rd.SetParameterValue("AdditionalDriverDrivingLicenceEndDate", "      ");
                    }

                }
                else
                {
                    rd.SetParameterValue("AdditionalDriverId", "     ");
                    rd.SetParameterValue("AdditionalDriverName", "    ");
                    rd.SetParameterValue("AdditionalDriverNationality", "     ");
                    rd.SetParameterValue("AdditionalDriverWorkPlace", "      ");
                    rd.SetParameterValue("AdditionalDriverAdr", "      ");
                    rd.SetParameterValue("AdditionalDriverGender", "     ");
                    rd.SetParameterValue("AdditionalDriverMembership", "      ");
                    rd.SetParameterValue("AdditionalDriverBirthDate", "      ");
                    rd.SetParameterValue("AdditionalDriverJob", "      ");
                    rd.SetParameterValue("AdditionalDriverEmail", "     ");
                    rd.SetParameterValue("AdditionalDriverDrivingLicenceEndDate", "      ");
                }

                if (CarPrice != null)
                {
                    rd.SetParameterValue("FreeKm", CarPrice.CR_Cas_Car_Price_Basic_No_Daily_Free_KM);
                    rd.SetParameterValue("KmValue", CarPrice.CR_Cas_Car_Price_Basic_Additional_KM_Value);
                    rd.SetParameterValue("FreeHours", CarPrice.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours);
                    rd.SetParameterValue("HoursMax", CarPrice.CR_Cas_Car_Price_Basic_Hour_Max);
                    rd.SetParameterValue("ExtraHourValue", CarPrice.CR_Cas_Car_Price_Basic_Extra_Hour_Value);
                    rd.SetParameterValue("DailyRentPrice", contract.CR_Cas_Contract_Basic_Daily_Rent);
                    rd.SetParameterValue("DiscountPercentage", CarPrice.CR_Cas_Car_Price_Basic_Rental_Tax_Rate + "%");
                    rd.SetParameterValue("ContractAccidentFees", CarPrice.CR_Cas_Car_Price_Basic_Carrying_Accident + "ريال");
                    rd.SetParameterValue("ContractStealingFees", CarPrice.CR_Cas_Car_Price_Basic_Stealing + "ريال");
                    rd.SetParameterValue("ContractDrawningFees", CarPrice.CR_Cas_Car_Price_Basic_Drowning + "ريال");
                    rd.SetParameterValue("ContractFireFees", CarPrice.CR_Cas_Car_Price_Basic_Carrying_Fire + "ريال");
                }
                else
                {
                    rd.SetParameterValue("FreeKm", "     ");
                    rd.SetParameterValue("KmValue", "     ");
                    rd.SetParameterValue("FreeHours", "     ");
                    rd.SetParameterValue("HoursMax", "     ");
                    rd.SetParameterValue("ExtraHourValue", "      ");
                    rd.SetParameterValue("DailyRentPrice", "      ");
                    rd.SetParameterValue("DiscountPercentage", "     ");
                    rd.SetParameterValue("ContractAccidentFees", "   ");
                    rd.SetParameterValue("ContractSteelingFees", "   ");
                    rd.SetParameterValue("ContractDrawningFees", "   ");
                    rd.SetParameterValue("ContractFireFees", "       s");
                }


                


                var Stamp = lessor.CR_Mas_Com_Lessor_Stamp;
                var stm = Stamp.Replace("~", "");
                stm = stm.Replace("/", "\\");
                stm = stm.Substring(1, stm.Length - 1);
                var stp = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + stm;
                rd.SetParameterValue("CompanyStamp", stp);



                rd.SetParameterValue("ContractNo", Sno);
                if (contract.CR_Cas_Contract_Basic_Date != null)
                {
                    DateTime ContractDate = (DateTime)contract.CR_Cas_Contract_Basic_Date;
                    string CDate=string.Format("{0:yyyy-MM-dd}", ContractDate.ToShortDateString());
                    rd.SetParameterValue("ContractDate", CDate);
                }
                else
                {
                    rd.SetParameterValue("ContractDate", "  ");
                }

                if (contract.CR_Cas_Contract_Basic_Start_Date != null)
                {
                    DateTime ContractStartDate = (DateTime)contract.CR_Cas_Contract_Basic_Start_Date;
                    string CStartDate = string.Format("{0:yyyy-MM-dd}", ContractStartDate.ToShortDateString());
                    rd.SetParameterValue("ContractStartDate", CStartDate);
                }
                else
                {
                    rd.SetParameterValue("ContractStartDate", "  ");
                }

                if (contract.CR_Cas_Contract_Basic_Expected_End_Date != null)
                {
                    DateTime ContractEndDate = (DateTime)contract.CR_Cas_Contract_Basic_Expected_End_Date;
                    string CEndDate = string.Format("{0:yyyy-MM-dd}", ContractEndDate.ToShortDateString());
                    rd.SetParameterValue("ContractEndDate", CEndDate);
                }
                else
                {
                    rd.SetParameterValue("ContractEndDate", "  ");
                }

                //rd.SetParameterValue("ContractDate", contract.CR_Cas_Contract_Basic_Date);
                //rd.SetParameterValue("ContractStartDate", contract.CR_Cas_Contract_Basic_Start_Date);
                //rd.SetParameterValue("ContractEndDate", contract.CR_Cas_Contract_Basic_Expected_End_Date);
                rd.SetParameterValue("ExpectedDays", contract.CR_Cas_Contract_Basic_Expected_Rental_Days);
                if (contract.CR_Cas_Contract_Basic_Start_Time != null)
                {
                    rd.SetParameterValue("ContractStartTime", contract.CR_Cas_Contract_Basic_Start_Time.ToString());
                }
                else
                {
                    rd.SetParameterValue("ContractStartTime", "   ");
                }
                if (contract.CR_Cas_Contract_Basic_Expected_End_Time != null)
                {
                    rd.SetParameterValue("ContractEndTime", contract.CR_Cas_Contract_Basic_Expected_End_Time.ToString());
                }
                else
                {
                    rd.SetParameterValue("ContractEndTime", "     ");
                }

                //if (contract.CR_Cas_Contract_Basic_Tax_Rate != null)
                //{
                //    rd.SetParameterValue("ContractTax", contract.CR_Cas_Contract_Basic_Tax_Rate);
                //}
                //else
                //{
                //    rd.SetParameterValue("ContractTax", "     ");
                //}

                if (contract.CR_Cas_Contract_Basic_User_Discount != null && contract.CR_Cas_Contract_Basic_User_Discount != 0)
                {
                    rd.SetParameterValue("ContractUserDiscount", contract.CR_Cas_Contract_Basic_User_Discount + "%");
                }
                else
                {
                    rd.SetParameterValue("ContractUserDiscount", "");
                }


                if (contract.CR_Cas_Contract_Basic_Value != null)
                {
                    rd.SetParameterValue("ContractValue", contract.CR_Cas_Contract_Basic_Value);
                }
                else
                {
                    rd.SetParameterValue("ContractValue", "     ");
                }

                if (contract.CR_Cas_Contract_Basic_Discount_Value != null && contract.CR_Cas_Contract_Basic_Discount_Value != 0)
                {
                    rd.SetParameterValue("DiscountValue", contract.CR_Cas_Contract_Basic_Discount_Value);
                }
                else
                {
                    rd.SetParameterValue("DiscountValue", "     ");
                }


                if (contract.CR_Cas_Contract_Basic_After_Discount_Value != null)
                {
                    rd.SetParameterValue("ContractAfterDiscountValue", contract.CR_Cas_Contract_Basic_After_Discount_Value);
                }
                else
                {
                    rd.SetParameterValue("ContractAfterDiscountValue", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_Tax_Value != null)
                {
                    rd.SetParameterValue("TaxValue", contract.CR_Cas_Contract_Basic_Tax_Value);
                }
                else
                {
                    rd.SetParameterValue("TaxValue", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_Net_Value != null)
                {
                    rd.SetParameterValue("ContractNetValue", contract.CR_Cas_Contract_Basic_Net_Value);
                }
                else
                {
                    rd.SetParameterValue("ContractNetValue", "      ");
                }


                if (contract.CR_Cas_Contract_Basic_Payed_Value != null)
                {
                    rd.SetParameterValue("ContractPayedValue", contract.CR_Cas_Contract_Basic_Payed_Value);
                }
                else
                {
                    rd.SetParameterValue("ContractPayedValue", "      ");
                }


                if (contract.CR_Cas_Contract_Basic_Use_Within_Country == false)
                {
                    rd.SetParameterValue("AuthType", "داخلي");
                    rd.SetParameterValue("AuthorizationPercentage", contract.CR_Cas_Contract_Basic_Authorization_Value);
                }
                else
                {
                    rd.SetParameterValue("AuthType", "خارجي");
                    rd.SetParameterValue("AuthorizationPercentage", contract.CR_Cas_Contract_Basic_Authorization_Value);
                }
                if (contract.CR_Cas_Contract_Basic_End_Authorization != null)
                {
                    rd.SetParameterValue("AuthEndDate", contract.CR_Cas_Contract_Basic_End_Authorization);
                }
                else
                {
                    rd.SetParameterValue("AuthEndDate", "    ");
                }

                if (contract.CR_Cas_Contract_Basic_Previous_Balance != null)
                {
                    rd.SetParameterValue("PreviousBalance", contract.CR_Cas_Contract_Basic_Previous_Balance);
                }
                else
                {
                    rd.SetParameterValue("PreviousBalance", "0");
                }


                //var RenterLessor = db.CR_Cas_Renter_Lessor.FirstOrDefault(r => r.CR_Cas_Renter_Lessor_Id == contract.CR_Cas_Contract_Basic_Renter_Id);
                if (RenterLessor != null)
                {
                    if (RenterLessor.CR_Cas_Renter_Admin_Membership_Code != null)
                    {
                        rd.SetParameterValue("RenterMembership", RenterLessor.CR_Cas_Renter_Admin_Membership_Code.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("RenterMembership", "       ");
                    }

                    var RenterSignature = RenterLessor.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Signature;

                    if (RenterSignature != "" && RenterSignature != null)
                    {
                        var Rentersignature = RenterSignature.Replace("~", "");
                        Rentersignature = Rentersignature.Replace("/", "\\");
                        Rentersignature = Rentersignature.Substring(1, Rentersignature.Length - 1);
                        var RenterS = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + Rentersignature;
                        rd.SetParameterValue("RenterSignature", RenterS);
                    }
                    else
                    {
                        rd.SetParameterValue("RenterSignature", " ");
                    }

                }
                else
                {
                    rd.SetParameterValue("PreviousBalance", "      ");
                    rd.SetParameterValue("RenterMembership", "      ");
                    rd.SetParameterValue("RenterSignature", "      ");

                }

                var logo = lessor.CR_Mas_Com_Lessor_Logo_Print.ToString();
                var log = logo.Replace("~", "");
                log = log.Replace("/", "\\");
                log = log.Substring(1, log.Length - 1);
                var lm = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + log;
                rd.SetParameterValue("Logo", lm);


                

                if (img1 != null && img1 != "")
                {
                    img1 = img1.Replace("/", "\\");
                    img1 = img1.Substring(1, img1.Length - 1);
                    var img1path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img1;
                    rd.SetParameterValue("img1", img1path);
                }
                else
                {
                    rd.SetParameterValue("img1", "         ");
                }
                if (img2 != null && img2 != "")
                {
                    img2 = img2.Replace("/", "\\");
                    img2 = img2.Substring(1, img2.Length - 1);
                    var img2path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img2;
                    rd.SetParameterValue("img2", img2path);
                }
                else
                {
                    rd.SetParameterValue("img2", "         ");
                }
                if (img3 != null && img3 != "")
                {
                    img3 = img3.Replace("/", "\\");
                    img3 = img3.Substring(1, img3.Length - 1);
                    var img3path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img3;
                    rd.SetParameterValue("img3", img3path);
                }
                else
                {
                    rd.SetParameterValue("img3", "         ");
                }
                if (img4 != null && img4 != "")
                {
                    img4 = img4.Replace("/", "\\");
                    img4 = img4.Substring(1, img4.Length - 1);
                    var img4path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img4;
                    rd.SetParameterValue("img4", img4path);
                }
                else
                {
                    rd.SetParameterValue("img4", "         ");
                }
                if (img5 != null && img5 != "")
                {
                    img5 = img5.Replace("/", "\\");
                    img5 = img5.Substring(1, img5.Length - 1);
                    var img5path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img5;
                    rd.SetParameterValue("img5", img5path);
                }
                else
                {
                    rd.SetParameterValue("img5", "         ");
                }
                if (img6 != null && img6 != "")
                {
                    img6 = img6.Replace("/", "\\");
                    img6 = img6.Substring(1, img6.Length - 1);
                    var img6path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img6;
                    rd.SetParameterValue("img6", img6path);
                }
                else
                {
                    rd.SetParameterValue("img6", "         ");
                }
                if (img7 != null && img7 != "")
                {
                    img7 = img7.Replace("/", "\\");
                    img7 = img7.Substring(1, img7.Length - 1);
                    var img7path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img7;
                    rd.SetParameterValue("img7", img7path);
                }
                else
                {
                    rd.SetParameterValue("img7", "        ");
                }
                if (img8 != null && img8 != "")
                {
                    img8 = img8.Replace("/", "\\");
                    img8 = img8.Substring(1, img8.Length - 1);
                    var img8path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img8;
                    rd.SetParameterValue("img8", img8path);
                }
                else
                {
                    rd.SetParameterValue("img8", "       ");
                }
                if (img9 != null && img9 != "")
                {
                    img9 = img9.Replace("/", "\\");
                    img9 = img9.Substring(1, img9.Length - 1);
                    var img9path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img9;
                    rd.SetParameterValue("img9", img9path);
                }
                else
                {
                    rd.SetParameterValue("img9", "        ");
                }




            }

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, fullpath);
            TempData["printCR"] = fullpath;

            //}
            //catch (DbEntityValidationException ex)
            //{
            //    throw ex;
            //}

        }



        // GET: BasicContract/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
            if (cR_Cas_Contract_Basic == null)
            {
                return HttpNotFound();
            }
            ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor);
            return View(cR_Cas_Contract_Basic);
        }

        // POST: BasicContract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Contract_Basic_No,CR_Cas_Contract_Basic_Year,CR_Cas_Contract_Basic_Type,CR_Cas_Contract_Basic_Lessor,CR_Cas_Contract_Basic_Owner_Branch,CR_Cas_Contract_Basic_Rented_Branch,CR_Cas_Contract_Basic_Date,CR_Cas_Contract_Basic_Time,CR_Cas_Contract_Basic_Start_Date,CR_Cas_Contract_Basic_Start_Time,CR_Cas_Contract_Basic_Expected_End_Date,CR_Cas_Contract_Basic_Expected_End_Time,CR_Cas_Contract_Basic_Expected_Rental_Days,CR_Cas_Contract_Basic_Car_Price_Basic_No,CR_Cas_Contract_Basic_Data_Contract,CR_Cas_Contract_Basic_Bennan_Contract,CR_Cas_Contract_Basic_Tamm_Contract,CR_Cas_Contract_Basic_Msg_Contract,CR_Cas_Contract_Basic_Public_Discount,CR_Cas_Contract_Basic_Special_Discount,CR_Cas_Contract_Basic_Car_Serail_No,CR_Cas_Contract_Basic_Renter_Id,CR_Cas_Contract_Basic_Sector,CR_Cas_Contract_Basic_is_Renter_Driver,CR_Cas_Contract_Basic_Driver_Id,CR_Cas_Contract_Basic_is_Additional_Driver,CR_Cas_Contract_Basic_Additional_Driver_Id,CR_Cas_Contract_Basic_is_Special_Driver,CR_Cas_Contract_Basic_Special_Driver_Id,CR_Cas_Contract_Basic_Counter,CR_Cas_Contract_Basic_is_Data_From_Elm,CR_Cas_Contract_Basic_Confessions_Data,CR_Cas_Contract_Basic_Verification_Code,CR_Cas_Contract_Basic_Use_Within_Country,CR_Cas_Contract_Basic_Authorization_Code,CR_Cas_Contract_Basic_Daily_Rent,CR_Cas_Contract_Basic_Weekly_Rent,CR_Cas_Contract_Basic_Monthly_Rent,CR_Cas_Contract_Basic_Tax_Rate,CR_Cas_Contract_Basic_Daily_Free_KM,CR_Cas_Contract_Basic_Additional_KM_Value,CR_Cas_Contract_Basic_Free_Additional_Hours,CR_Cas_Contract_Basic_Hour_Max,CR_Cas_Contract_Basic_Extra_Hour_Value,CR_Cas_Contract_Basic_Receiving_Branch,CR_Cas_Contract_Basic_Actual_End_Date,CR_Cas_Contract_Basic_Actual_End_Time,CR_Cas_Contract_Basic_Actual_Rental_Days,CR_Cas_Contract_Basic_Total_KM,CR_Cas_Contract_Basic_Delivery_Reading_KM,CR_Cas_Contract_Basic_Additional_Free_KM,CR_Cas_Contract_Basic_Additional_Hours")] CR_Cas_Contract_Basic cR_Cas_Contract_Basic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Contract_Basic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor);
            return View(cR_Cas_Contract_Basic);
        }

        // GET: BasicContract/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
            if (cR_Cas_Contract_Basic == null)
            {
                return HttpNotFound();
            }
            return View(cR_Cas_Contract_Basic);
        }

        // POST: BasicContract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
            db.CR_Cas_Contract_Basic.Remove(cR_Cas_Contract_Basic);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
