using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers
{
    public class ContractSettlementController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: ContractSettlement
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
                    RedirectToAction("Account", "Login");
                }
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }
            var cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode 
            && (c.CR_Cas_Contract_Basic_Status == "A" || c.CR_Cas_Contract_Basic_Status=="E")
            && (c.CR_Cas_Contract_Basic_Owner_Branch == BranchCode || c.CR_Cas_Contract_Basic_is_Receiving_Branch==true))   
                .Include(c => c.CR_Cas_Car_Price_Basic)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Sup_Sector)
                .Include(c => c.CR_Mas_Renter_Information)
                .Include(c => c.CR_Cas_Sup_Car_Information);
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
            if (paycode == "11")
            {
                SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(x => x.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && x.CR_Cas_Sup_SalesPoint_Brn_Code == BranchCode &&
                x.CR_Cas_Sup_SalesPoint_Bank_Code == Code && x.CR_Cas_Sup_SalesPoint_Status == "A" && x.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "A").ToList();
            }
            else if (paycode != "" && paycode != null)
            {
                SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(x => x.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && x.CR_Cas_Sup_SalesPoint_Brn_Code == BranchCode &&
               x.CR_Cas_Sup_SalesPoint_Bank_Code != Code && x.CR_Cas_Sup_SalesPoint_Status == "A" && x.CR_Cas_Sup_Bank.CR_Cas_Sup_Bank_Status == "A").ToList();
            }
            else
            {
                SalesPoint = null;
            }
            return Json(SalesPoint, JsonRequestBehavior.AllowGet);
        }
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

       
        // GET: ContractSettlement/Create
        public ActionResult Create(string id1)
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
            if (id1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.FirstOrDefault(c=>c.CR_Cas_Contract_Basic_No==id1);
            if (cR_Cas_Contract_Basic == null)
            {
                return HttpNotFound();
            }
            else
            {
                ViewBag.ContractNo = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_No;
                ViewBag.Date = DateTime.Now.ToString("yyyy/MM/dd");
                ViewBag.CarSerialNo = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Serail_No;
                ViewBag.CarName = cR_Cas_Contract_Basic.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Collect_Ar_Name;
                ViewBag.RenterId = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id;
                ViewBag.RenterName = cR_Cas_Contract_Basic.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Ar_Name;
                ViewBag.ContractStartDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Start_Date);
                ViewBag.ContractStartTime = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Start_Time;
                
                ViewBag.DailyFreeKm = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Free_KM;
                ViewBag.FreeAdditionalHours = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Free_Additional_Hours;

                var CurrentHour =TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
                var EndTime = TimeSpan.Parse(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Time.ToString()) ;
                //////var AdditionalHours= EndTime - CurrentHour;
                //////if (CurrentHour > EndTime)
                //////{
                //////    ViewBag.AdditionalHours = AdditionalHours;
                //////}
                //////else
                //////{
                //////    ViewBag.AdditionalHours = 0;
                //////}
                var AddDays = 0;
                ViewBag.FreeAdditionalHours = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Free_Additional_Hours;
                ViewBag.MaxHours = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Hour_Max;
                var MaxHours =Convert.ToInt32(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Hour_Max);
                if (CurrentHour > EndTime)
                {
                    TimeSpan freeH = TimeSpan.FromHours(Convert.ToDouble(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Free_Additional_Hours));
                    var H = CurrentHour - EndTime;
                    var r = H - freeH;
                    ViewBag.AdditionalHours = r.Hours;
                    if(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Hour_Max - cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Free_Additional_Hours > r.Hours)
                    {
                        AddDays = 0;
                    }
                    else
                    {
                        AddDays = 1;
                    }

                    
                }
                else
                {
                    ViewBag.AdditionalHours = "0";
                }

                


                ViewBag.AdditionalKmValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_KM_Value;
                ViewBag.ExtraHourValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Extra_Hour_Value;
                
                var branch = db.CR_Cas_Sup_Branch.FirstOrDefault(b=>b.CR_Cas_Sup_Branch_Code==BranchCode && b.CR_Cas_Sup_Lessor_Code==LessorCode);
                if (branch != null)
                {
                    ViewBag.BranchReceipt = branch.CR_Cas_Sup_Branch_Ar_Name;
                }

                var branchDelivery = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Owner_Branch && b.CR_Cas_Sup_Lessor_Code==LessorCode);
                if (branchDelivery != null)
                {
                    ViewBag.DeliveryBranch = branchDelivery.CR_Cas_Sup_Branch_Ar_Name;
                }
                ViewBag.AuthEndDate = DateTime.Now.ToString("yyyy/MM/dd h:m:s tt");
                
                
                ViewBag.AdditionalDriverVal = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_Driver_Value;
                if (Session["ContractCancel"].ToString() == "True")
                {
                    ViewBag.ContractEndDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date);
                    ViewBag.ContractCancel = "True";
                }
                else
                {
                    if (cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date > DateTime.Now)
                    {
                        ViewBag.ContractEndDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date);
                        ViewBag.ContractCancel = "False";
                    }
                    else
                    {
                        ViewBag.ContractEndDate = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
                        ViewBag.ContractCancel = "True";
                    }
                }

                ViewBag.ContractEndTime = CurrentHour;


                var CurrentDate = DateTime.Now.ToShortDateString();
                DateTime CDate = Convert.ToDateTime(CurrentDate);
                var StartDate = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Start_Date;
                DateTime Sdate = Convert.ToDateTime(StartDate);
                var nbrdays = (CDate-Sdate).Days;
                if (nbrdays == 0)
                {
                    nbrdays = nbrdays + 1+ AddDays;
                    ViewBag.ContractDaysNbr = 1;
                    //ViewBag.AdditionalHours = 0;
                }
                else
                {
                    ViewBag.ContractDaysNbr = nbrdays+ AddDays;
                    ViewBag.AdditionalHours = 0;
                }
               

                if (nbrdays < 7)
                {
                    ViewBag.DailyRentValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Rent;
                    
                } else if(nbrdays>=7 && nbrdays < 30){
                    ViewBag.DailyRentValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Weekly_Rent;
                   
                } else
                {                                                                                                       
                    ViewBag.DailyRentValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Monthly_Rent;
                   
                }

                ViewBag.ChoicesVal = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Choices_Value * nbrdays;
                ViewBag.AdditionalVal = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_Value;

                var DailyFreeKm = nbrdays * cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Free_KM;
                ViewBag.DailyFreeKm = DailyFreeKm;
                ViewBag.TotalFreeKm = DailyFreeKm;

                

                if (cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_is_Km_Open == true)
                {
                    ViewBag.IsOpenKm = "True";
                }
                else
                {
                    ViewBag.IsOpenKm = "False";
                }
                ViewBag.OldCurrentKm = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_CurrentMeters;
                ViewBag.AuthValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Authorization_Value;
                ViewBag.Discount = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_User_Discount;
                ViewBag.Tax = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Tax_Rate;
                var renter = db.CR_Cas_Renter_Lessor.FirstOrDefault(r => r.CR_Cas_Renter_Lessor_Id == cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id);
                if (renter != null)
                {
                    ViewBag.RenterPrevBalance = renter.CR_Cas_Renter_Lessor_Balance;
                }
                else
                {
                    ViewBag.RenterPrevBalance = "0.00";
                }

                if (cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_is_Insurance == "1")
                {
                    ViewBag.InsuranceVal = "شامل";
                }
                else
                {
                    ViewBag.InsuranceVal = "ضد الغير";
                }

                ViewBag.OldKm = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_CurrentMeters;

                ViewBag.PayType = new SelectList(db.CR_Mas_Sup_Payment_Method.Where(p => p.CR_Mas_Sup_Payment_Method_Type == "2" && p.CR_Mas_Sup_Payment_Method_Status == "A")
                , "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Ar_Name");
                ViewBag.CasherName = "";
            }
            return View(cR_Cas_Contract_Basic);
        }

        // POST: ContractSettlement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string ContractNo,string ContractEndDate,string ContractEndTime,string ContractDaysNbr, string ContractValED,string ContractValID,string TaxVal,
            string TotalContractIT,string TotPayed,string CurrentMeter,string OldKm, string TotalFreeKm,string AdditionalHours,string ExtraKmValue, string TotalHoursValue , string Chk_Depences, string Chk_Compensation,
            string Depences,string DepencesReason,string CompensationVal,string CompensationReason,string RenterPrevBalance,string reste,
            HttpPostedFileBase img1, HttpPostedFileBase img2, HttpPostedFileBase img3, HttpPostedFileBase img4, HttpPostedFileBase img5, HttpPostedFileBase img6, HttpPostedFileBase img7,
            HttpPostedFileBase img8, HttpPostedFileBase img9, FormCollection collection, HttpPostedFileBase imgx1, HttpPostedFileBase imgx2, HttpPostedFileBase imgx3, HttpPostedFileBase imgx4,
            HttpPostedFileBase imgy1, HttpPostedFileBase imgy2, HttpPostedFileBase imgy3, HttpPostedFileBase imgy4)
        {
            if (ModelState.IsValid)
            {
                using (DbContextTransaction dbTran = db.Database.BeginTransaction())
                {
                    try
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
                        var Contract = db.CR_Cas_Contract_Basic.Single(c=>c.CR_Cas_Contract_Basic_No== ContractNo && c.CR_Cas_Contract_Basic_Status!="y");
                        if (Contract != null)
                        {
                            Contract.CR_Cas_Contract_Basic_Status = "C";
                            Contract.CR_Cas_Contract_Basic_Receiving_Branch = BranchCode;
                            if (ContractEndDate != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Expected_End_Date = Convert.ToDateTime(ContractEndDate);
                            }
                            if (ContractEndTime!="")
                            {
                                Contract.CR_Cas_Contract_Basic_Expected_End_Time = TimeSpan.Parse(ContractEndTime);
                            }
                            if (ContractDaysNbr != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Expected_Rental_Days = int.Parse(ContractDaysNbr);
                            }
                            
                            Contract.CR_Cas_Contract_Basic_End_Authorization = DateTime.Now;
                            Contract.CR_Cas_Contract_BasicAuthorization_Staus = "B";
                            if(ContractValED!="")
                            {
                                Contract.CR_Cas_Contract_Basic_Value = Convert.ToDecimal(ContractValED);
                            }
                            if(ContractValED!="" && ContractValID != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Discount_Value = Convert.ToDecimal(ContractValED) - Convert.ToDecimal(ContractValID);
                            }
                            if (TaxVal != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Tax_Value = Convert.ToDecimal(TaxVal);
                            }
                            if (TotalContractIT!="")
                            {
                                Contract.CR_Cas_Contract_Basic_Net_Value = Convert.ToDecimal(TotalContractIT);
                            }
                            if (TotPayed!="")
                            {
                                Contract.CR_Cas_Contract_Basic_Payed_Value = Convert.ToDecimal(TotPayed);
                            }
                            if (CurrentMeter!="")
                            {
                                Contract.CR_Cas_Contract_Basic_Delivery_Reading_KM = Convert.ToInt32(CurrentMeter);
                            }
                            if (TotalFreeKm!="")
                            {
                                Contract.CR_Cas_Contract_Basic_Actual_Additional_Free_KM = Convert.ToInt32(TotalFreeKm);
                            }
                            if (AdditionalHours!="")
                            {
                                Contract.CR_Cas_Contract_Basic_Additional_Hours = Convert.ToInt32(AdditionalHours);
                            }
                            if (ExtraKmValue!="")
                            {
                                Contract.CR_Cas_Contract_Basic_Actual_Extra_KM_Value = Convert.ToDecimal(ExtraKmValue);
                            }
                            if (TotalHoursValue!="")
                            {
                                Contract.CR_Cas_Contract_Basic_Actual_Extra_Hour_Value = Convert.ToDecimal(TotalHoursValue);
                            }
                            
                            if (Chk_Depences == "true")
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_is_Expenses ="1";
                            }
                            else
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_is_Expenses = "0";
                            }
                            Contract.CR_Cas_Contract_Basic_Contarct_Expenses_Value = Depences;
                            Contract.CR_Cas_Contract_Basic_Contarct_Expenses_Description = DepencesReason;

                            if (Chk_Compensation == "true")
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_is_Compensation = "1";
                            }
                            else
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_is_Compensation = "0";
                            }
                            Contract.CR_Cas_Contract_Basic_Contarct_Compensation_Value = CompensationVal;
                            Contract.CR_Cas_Contract_Basic_Contarct_Compensation_Description = CompensationReason;

                            Contract.CR_Cas_Contract_Basic_Close_Previous_Balance = RenterPrevBalance;
                            Contract.CR_Cas_Contract_Basic_Contarct_Remaining_Amount = reste;

                            if (ContractDaysNbr != "")
                            {
                                if (Convert.ToInt32(ContractDaysNbr) <= 3)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 1;
                                }
                                else if (Convert.ToInt32(ContractDaysNbr) > 3 && Convert.ToInt32(ContractDaysNbr) <= 7)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 2;
                                }
                                else if (Convert.ToInt32(ContractDaysNbr) >= 8 && Convert.ToInt32(ContractDaysNbr) <= 10)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 3;
                                }
                                else if (Convert.ToInt32(ContractDaysNbr) >= 11 && Convert.ToInt32(ContractDaysNbr) <= 15)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 4;
                                }
                                else if (Convert.ToInt32(ContractDaysNbr) >= 16 && Convert.ToInt32(ContractDaysNbr) <= 25)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 5;
                                }
                                else if (Convert.ToInt32(ContractDaysNbr) >= 26 && Convert.ToInt32(ContractDaysNbr) <= 30)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_Day_Count = 6;
                                }
                                else if (Convert.ToInt32(ContractDaysNbr) > 30)
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
                            
                            if(CurrentMeter!="" && OldKm!="" && ContractDaysNbr != "")
                            {
                                var RealKm = (Convert.ToDecimal(CurrentMeter) - Convert.ToDecimal(OldKm)) / Convert.ToInt32(ContractDaysNbr);
                                if (RealKm <= 100)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_KM = 1;
                                }
                                else if (RealKm >= 101 && RealKm < 200)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_KM = 2;
                                }
                                else if (RealKm >= 201 && RealKm < 250)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_KM = 3;
                                }
                                else if (RealKm >= 301 && RealKm < 350)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_KM = 4;
                                }
                                else if (RealKm >= 350)
                                {
                                    Contract.CR_Cas_Contract_Basic_Statistics_KM = 5;
                                }

                                Contract.CR_Cas_Contract_Basic_Actual_Total_KM = Convert.ToInt32(Convert.ToDecimal(CurrentMeter)) - Convert.ToInt32(Convert.ToDecimal(OldKm));
                            }

                            Contract.CR_Cas_Contract_Basic_Statistics_Day_No = Contract.CR_Cas_Contract_Basic_Statistics_Day_No - Contract.CR_Cas_Contract_Basic_Expected_Rental_Days + int.Parse(ContractDaysNbr);

                            //////////////////////////////Save Virtual Inspection///////////////////////
                            List<CR_Cas_Contract_Virtual_Inspection> Linspection = new List<CR_Cas_Contract_Virtual_Inspection>();
                            foreach (string item in collection.AllKeys)
                            {
                                if (item.StartsWith("ChkInspection_"))
                                {
                                    CR_Cas_Contract_Virtual_Inspection inspection = new CR_Cas_Contract_Virtual_Inspection();
                                    inspection.CR_Cas_Contract_Virtual_Inspection_No = Contract.CR_Cas_Contract_Basic_No;
                                    inspection.CR_Cas_Contract_Virtual_Inspection_In_Out = 2;

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
                            ////////////////////////////////////////////////////////////////////////////
                            ///
                            /////////////////////////Create Tax Owed////////////////////////////
                            /*CR_Cas_Account_Tax_Owed TaxOwed = new CR_Cas_Account_Tax_Owed();
                            TaxOwed.CR_Cas_Account_Tax_Owed_Contract_No = Contract.CR_Cas_Contract_Basic_No;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Com_Code = LessorCode;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Brn_Code = BranchCode;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Contract_Value = Contract.CR_Cas_Contract_Basic_Net_Value;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Percentage = Contract.CR_Cas_Contract_Basic_Tax_Rate;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Value = Contract.CR_Cas_Contract_Basic_Tax_Value;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Due_Date = DateTime.Now;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Is_Paid = false;
                            db.CR_Cas_Account_Tax_Owed.Add(TaxOwed);*/
                            /////////////////////////////////////////////////////////////
                            ///////////////////////Create Bnan Owed/////////////////////////////
                            /*CR_Cas_Account_Bnan_Owed BnanOwed = new CR_Cas_Account_Bnan_Owed();
                            BnanOwed.CR_Cas_Account_Bnan_Owed_Contract_No = Contract.CR_Cas_Contract_Basic_No;
                            BnanOwed.CR_Cas_Account_Bnan_Owed_Com_Code = LessorCode;
                            var CompanyContract = db.CR_Cas_Company_Contract.FirstOrDefault(c=>c.CR_Cas_Company_Contract_Lessor==LessorCode && c.CR_Cas_Company_Contract_Code=="18" && c.CR_Cas_Company_Contract_Status=="A");
                            if (CompanyContract != null)
                            {
                                BnanOwed.CR_Cas_Account_Bnan_Owed_Contract_Com = CompanyContract.CR_Cas_Company_Contract_No;
                            }
                            BnanOwed.CR_Cas_Account_Bnan_Owed_Value = Contract.CR_Cas_Contract_Basic_Net_Value;
                            var ServiceBnan = db.CR_Mas_Service_Bnan_Contract.FirstOrDefault(s => s.CR_Mas_Service_Bnan_Contract_No == CompanyContract.CR_Cas_Company_Contract_No);
                            if (ServiceBnan != null)
                            {
                                if (ServiceBnan.CR_Mas_Service_Bnan_Contract_Price_Or_Percentage == true)
                                {
                                    BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Type = true;
                                    BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Percentage = 0;
                                }
                                else
                                {
                                    BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Type = false;
                                    BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Value = 0;
                                }
                            }*/


                            
                            /*var Services = db.CR_Mas_Service_Bnan_Contract.Where(s => s.CR_Mas_Service_Bnan_Contract_No == CompanyContract.CR_Cas_Company_Contract_No);
                            var LastRecord = Services.OrderByDescending(s => s.CR_Mas_Service_Bnan_Contract_Code).FirstOrDefault();
                            foreach (var Service in Services)
                            {
                                
                                if (Contract.CR_Cas_Contract_Basic_Daily_Rent < Service.CR_Mas_Service_Bnan_Contract_Price)
                                {
                                    if (BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Type == false)
                                    {
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Percentage = Service.CR_Mas_Service_Bnan_Contract_Percentage;
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Amount = (Service.CR_Mas_Service_Bnan_Contract_Percentage * Contract.CR_Cas_Contract_Basic_Net_Value) / 100;
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Value = 0;
                                        ////new Fields////
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Before_Due_Amount = (Service.CR_Mas_Service_Bnan_Contract_Percentage * Contract.CR_Cas_Contract_Basic_Net_Value) / 100;
                                        var discount = CompanyContract.CR_Cas_Company_Contract_Discount_Rate;
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_After_Due_Amount = BnanOwed.CR_Cas_Account_Bnan_Owed_Before_Due_Amount * (1 - discount / 100);
                                    }
                                    else
                                    {
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Value = Service.CR_Mas_Service_Bnan_Contract_Value;
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Percentage = 0;

                                        ////new Fields////
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Before_Due_Amount = Service.CR_Mas_Service_Bnan_Contract_Value;
                                        var discount = CompanyContract.CR_Cas_Company_Contract_Discount_Rate;
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_After_Due_Amount = BnanOwed.CR_Cas_Account_Bnan_Owed_Before_Due_Amount * (1 - discount / 100);
                                    }
                                    
                                }
                                else if (Contract.CR_Cas_Contract_Basic_Daily_Rent > LastRecord.CR_Mas_Service_Bnan_Contract_Price)
                                {
                                    if (BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Type == false)
                                    {
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Percentage = LastRecord.CR_Mas_Service_Bnan_Contract_Percentage;
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Amount = (LastRecord.CR_Mas_Service_Bnan_Contract_Percentage * Contract.CR_Cas_Contract_Basic_Net_Value) / 100;
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Value = 0;

                                        ////new Fields////
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Before_Due_Amount = (LastRecord.CR_Mas_Service_Bnan_Contract_Percentage * Contract.CR_Cas_Contract_Basic_Net_Value) / 100;
                                        var discount = CompanyContract.CR_Cas_Company_Contract_Discount_Rate;
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_After_Due_Amount = BnanOwed.CR_Cas_Account_Bnan_Owed_Before_Due_Amount * (1 - discount / 100);
                                    }
                                    else
                                    {
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Value = LastRecord.CR_Mas_Service_Bnan_Contract_Value;
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Percentage = 0;

                                        ////new Fields////
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_Before_Due_Amount = LastRecord.CR_Mas_Service_Bnan_Contract_Value;
                                        var discount = CompanyContract.CR_Cas_Company_Contract_Discount_Rate;
                                        BnanOwed.CR_Cas_Account_Bnan_Owed_After_Due_Amount = BnanOwed.CR_Cas_Account_Bnan_Owed_Before_Due_Amount * (1 - discount / 100);
                                    }
                                    
                                }
                               
                            }*/

                            var CasRenter = db.CR_Cas_Renter_Lessor.FirstOrDefault(r=>r.CR_Cas_Renter_Lessor_Id==Contract.CR_Cas_Contract_Basic_Renter_Id);
                            if (CasRenter != null)
                            {
                                var ContractNumber = CasRenter.CR_Cas_Renter_Lessor_Contract_Number;
                                var InteractionValue = CasRenter.CR_Cas_Renter_Lessor_Interaction_Amount_Value;
                                var Km = CasRenter.CR_Cas_Renter_Lessor_KM;

                            }
/*
                            BnanOwed.CR_Cas_Account_Bnan_Owed_Tax_Percentage = CompanyContract.CR_Cas_Company_Contract_Tax_Rate;
                            BnanOwed.CR_Cas_Account_Bnan_Owed_Tax_Value = (BnanOwed.CR_Cas_Account_Bnan_Owed_After_Due_Amount * CompanyContract.CR_Cas_Company_Contract_Tax_Rate) / 100;
                            BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Amount_After_Tax_Value = BnanOwed.CR_Cas_Account_Bnan_Owed_After_Due_Amount + BnanOwed.CR_Cas_Account_Bnan_Owed_Tax_Value;
                            BnanOwed.CR_Cas_Account_Bnan_Owed_Daily_Value = Contract.CR_Cas_Contract_Basic_Daily_Rent;
                            BnanOwed.CR_Cas_Account_Bnan_Owed_Due_Date = DateTime.Now;
                            BnanOwed.CR_Cas_Account_Bnan_Owed_Is_Paid = false;
                            db.CR_Cas_Account_Bnan_Owed.Add(BnanOwed);*/
                            /////////////////////////////////////////////////////////////

                            Contract.CR_Cas_Contract_Basic_User_Close = UserLogin;


                            string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                            string FolderContract = Server.MapPath(string.Format("~/{0}/", "/images/Company"));
                            string FolderLessor = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode));
                            string FolderBranch = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode));
                            string FolderContractNo = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No));
                            string OpenPdf = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "ClosePDF"));
                            string CreateCopyFolder = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "ClosePDF"
                                + "/" + Contract.CR_Cas_Contract_Basic_Copy));
                            string CloseImage = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage"));
                            string CloseImageDepences = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage/Depences"));
                            string CloseImageCompensation = Server.MapPath(string.Format("~/{0}/", "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage/Compensation"));


                            if (!Directory.Exists(folderimages))
                            {
                                Directory.CreateDirectory(folderimages);
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
                            if (!Directory.Exists(CloseImage))
                            {
                                Directory.CreateDirectory(CloseImage);
                            }
                            if (!Directory.Exists(CloseImageDepences))
                            {
                                Directory.CreateDirectory(CloseImageDepences);
                            }
                            if (!Directory.Exists(CloseImageCompensation))
                            {
                                Directory.CreateDirectory(CloseImageCompensation);
                            }


                            string img1path = "";
                            if (img1 != null)
                            {
                                if (img1.FileName.Length > 0)
                                {
                                    img1path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage" + "/" + Path.GetFileName(img1.FileName);
                                    img1.SaveAs(HttpContext.Server.MapPath(img1path));
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
                                    img2path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage" + "/" + Path.GetFileName(img2.FileName);
                                    img2.SaveAs(HttpContext.Server.MapPath(img2path));
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
                                    img3path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage" + "/" + Path.GetFileName(img3.FileName);
                                    img3.SaveAs(HttpContext.Server.MapPath(img3path));
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
                                    img4path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage" + "/" + Path.GetFileName(img4.FileName);
                                    img4.SaveAs(HttpContext.Server.MapPath(img4path));
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
                                    img5path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage" + "/" + Path.GetFileName(img5.FileName);
                                    img5.SaveAs(HttpContext.Server.MapPath(img5path));
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
                                    img6path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage" + "/" + Path.GetFileName(img6.FileName);
                                    img6.SaveAs(HttpContext.Server.MapPath(img6path));
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
                                    img7path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage" + "/" + Path.GetFileName(img7.FileName);
                                    img7.SaveAs(HttpContext.Server.MapPath(img7path));
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
                                    img8path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage" + "/" + Path.GetFileName(img8.FileName);
                                    img8.SaveAs(HttpContext.Server.MapPath(img8path));
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
                                    img9path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage" + "/" + Path.GetFileName(img9.FileName);
                                    img9.SaveAs(HttpContext.Server.MapPath(img9path));
                                }
                            }
                            else
                            {
                                img9path = "/images/common/Empty.bmp";
                            }




                            ////////////////////////////////////depences////////////////////////////////
                            string imgx1path = "";
                            if (imgx1 != null)
                            {
                                if (imgx1.FileName.Length > 0)
                                {
                                    imgx1path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage/Depences" + "/" + Path.GetFileName(imgx1.FileName);
                                    imgx1.SaveAs(HttpContext.Server.MapPath(imgx1path));
                                }
                            }
                            else
                            {
                                imgx1path = "/images/common/Empty.bmp";
                            }

                            string imgx2path = "";
                            if (imgx2 != null)
                            {
                                if (imgx2.FileName.Length > 0)
                                {
                                    imgx2path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage/Depences" + "/" + Path.GetFileName(imgx2.FileName);
                                    imgx2.SaveAs(HttpContext.Server.MapPath(imgx2path));
                                }
                            }
                            else
                            {
                                imgx2path = "/images/common/Empty.bmp";
                            }

                            string imgx3path = "";
                            if (imgx3 != null)
                            {
                                if (imgx3.FileName.Length > 0)
                                {
                                    imgx3path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage/Depences" + "/" + Path.GetFileName(imgx3.FileName);
                                    imgx3.SaveAs(HttpContext.Server.MapPath(imgx3path));
                                }
                            }
                            else
                            {
                                imgx3path = "/images/common/Empty.bmp";
                            }

                            string imgx4path = "";
                            if (imgx4 != null)
                            {
                                if (imgx4.FileName.Length > 0)
                                {
                                    imgx4path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage/Depences" + "/" + Path.GetFileName(imgx4.FileName);
                                    imgx4.SaveAs(HttpContext.Server.MapPath(imgx4path));
                                }
                            }
                            else
                            {
                                imgx4path = "/images/common/Empty.bmp";
                            }
                            /////////////////////////////////////////////////////////////////////////
                            ///

                            ////////////////////////////////////////////////////////////Compensation///////////////////////////////////////////////////////
                            string imgy1path = "";
                            if (imgy1 != null)
                            {
                                if (imgy1.FileName.Length > 0)
                                {
                                    imgy1path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage/Compensation" + "/" + Path.GetFileName(imgy1.FileName);
                                    imgy1.SaveAs(HttpContext.Server.MapPath(imgy1path));
                                }
                            }
                            else
                            {
                                imgy1path = "/images/common/Empty.bmp";
                            }

                            string imgy2path = "";
                            if (imgy2 != null)
                            {
                                if (imgy2.FileName.Length > 0)
                                {
                                    imgy2path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage/Compensation" + "/" + Path.GetFileName(imgy2.FileName);
                                    imgy2.SaveAs(HttpContext.Server.MapPath(imgy2path));
                                }
                            }
                            else
                            {
                                imgy2path = "/images/common/Empty.bmp";
                            }

                            string imgy3path = "";
                            if (imgy3 != null)
                            {
                                if (imgy3.FileName.Length > 0)
                                {
                                    imgy3path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage/Compensation" + "/" + Path.GetFileName(imgy3.FileName);
                                    imgy3.SaveAs(HttpContext.Server.MapPath(imgy3path));
                                }
                            }
                            else
                            {
                                imgy3path = "/images/common/Empty.bmp";
                            }

                            string imgy4path = "";
                            if (imgy4 != null)
                            {
                                if (imgy4.FileName.Length > 0)
                                {
                                    imgy4path = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + Contract.CR_Cas_Contract_Basic_No + "/" + "CloseImage/Compensation" + "/" + Path.GetFileName(imgy4.FileName);
                                    imgy4.SaveAs(HttpContext.Server.MapPath(imgy4path));
                                }
                            }
                            else
                            {
                                imgy4path = "/images/common/Empty.bmp";
                            }
                            /////////////////////////////////////////////////////////////////////////


                            db.Entry(Contract).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["TempModel"] = "Saved";
                            dbTran.Commit();
                        }
                        
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                    return RedirectToAction("Index");
                    }
            }

            ViewBag.PayType = new SelectList(db.CR_Mas_Sup_Payment_Method.Where(p => p.CR_Mas_Sup_Payment_Method_Type == "2" && p.CR_Mas_Sup_Payment_Method_Status == "A")
,           "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Ar_Name");
            ViewBag.CasherName = "";
            return View();
        }

        // GET: ContractSettlement/Edit/5
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
            ViewBag.CR_Cas_Contract_Basic_Car_Price_Basic_No = new SelectList(db.CR_Cas_Car_Price_Basic, "CR_Cas_Car_Price_Basic_No", "CR_Cas_Car_Price_Basic_Year", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Price_Basic_No);
            ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Long_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor);
            ViewBag.CR_Cas_Contract_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Sector);
            ViewBag.CR_Cas_Contract_Basic_Renter_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id);
            ViewBag.CR_Cas_Contract_Basic_Car_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Serail_No);
            return View(cR_Cas_Contract_Basic);
        }

        // POST: ContractSettlement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Contract_Basic_No,CR_Cas_Contract_Basic_Copy,CR_Cas_Contract_Basic_Year,CR_Cas_Contract_Basic_Type,CR_Cas_Contract_Basic_Lessor,CR_Cas_Contract_Basic_Sector,CR_Cas_Contract_Basic_Owner_Branch,CR_Cas_Contract_Basic_Date,CR_Cas_Contract_Basic_Time,CR_Cas_Contract_Basic_Renter_Id,CR_Cas_Contract_Basic_is_Renter_Driver,CR_Cas_Contract_Basic_Driver_Id,CR_Cas_Contract_Basic_Car_Serail_No,CR_Cas_Contract_Basic_is_Additional_Driver,CR_Cas_Contract_Basic_Additional_Driver_Id,CR_Cas_Contract_Basic_Start_Date,CR_Cas_Contract_Basic_Start_Time,CR_Cas_Contract_Basic_Expected_End_Date,CR_Cas_Contract_Basic_Expected_End_Time,CR_Cas_Contract_Basic_Expected_Rental_Days,CR_Cas_Contract_Basic_Additional_Driver_Value,CR_Cas_Contract_Basic_Authorization_Value,CR_Cas_Contract_Basic_Use_Within_Country,CR_Cas_Contract_Basic_End_Authorization,CR_Cas_Contract_BasicAuthorization_Staus,CR_Cas_Contract_BasicAuthorization_No,CR_Cas_Contract_Basic_Daily_Rent,CR_Cas_Contract_Basic_Weekly_Rent,CR_Cas_Contract_Basic_Monthly_Rent,CR_Cas_Contract_Basic_CurrentMeters,CR_Cas_Contract_Basic_Additional_Value,CR_Cas_Contract_Basic_Choices_Value,CR_Cas_Contract_Basic_Tax_Rate,CR_Cas_Contract_Basic_User_Discount,CR_Cas_Contract_Basic_Daily_Free_KM,CR_Cas_Contract_Basic_Additional_KM_Value,CR_Cas_Contract_Basic_Free_Additional_Hours,CR_Cas_Contract_Basic_Hour_Max,CR_Cas_Contract_Basic_Extra_Hour_Value,CR_Cas_Contract_Basic_is_Km_Open,CR_Cas_Contract_Basic_is_Insurance,CR_Cas_Contract_Basic_is_Receiving_Branch,CR_Cas_Contract_Basic_is_Receiving_Renter,CR_Cas_Contract_Basic_Value,CR_Cas_Contract_Basic_Discount_Value,CR_Cas_Contract_Basic_After_Discount_Value,CR_Cas_Contract_Basic_Tax_Value,CR_Cas_Contract_Basic_Net_Value,CR_Cas_Contract_Basic_Payed_Value,CR_Cas_Contract_Basic_Previous_Balance,CR_Cas_Contract_Basic_User_Insert,CR_Cas_Contract_Basic_Hour_DateTime_Alert,CR_Cas_Contract_Basic_Hour_MsgNo,CR_Cas_Contract_Basic_Day_DateTime_Alert,CR_Cas_Contract_Basic_Day_MsgNo,CR_Cas_Contract_Basic_EndContract_MsgNo,CR_Cas_Contract_Basic_Alert_Status,CR_Cas_Contract_Basic_CreateContract_Pdf,CR_Cas_Contract_Basic_CloseContract_Pdf,CR_Cas_Contract_Basic_Car_Price_Basic_No,CR_Cas_Contract_Basic_Status,CR_Cas_Contract_Basic_Receiving_Branch,CR_Cas_Contract_Basic_Actual_Total_KM,CR_Cas_Contract_Basic_Delivery_Reading_KM,CR_Cas_Contract_Basic_Actual_Additional_Free_KM,CR_Cas_Contract_Basic_Additional_Hours,CR_Cas_Contract_Basic_Actual_Extra_KM_Value,CR_Cas_Contract_Basic_Actual_Extra_Hour_Value,CR_Cas_Contract_Basic_Contarct_is_Expenses,CR_Cas_Contract_Basic_Contarct_Expenses_Value,CR_Cas_Contract_Basic_Contarct_Expenses_Description,CR_Cas_Contract_Basic_Contarct_is_Compensation,CR_Cas_Contract_Basic_Contarct_Compensation_Value,CR_Cas_Contract_Basic_Contarct_Compensation_Description,CR_Cas_Contract_Basic_Close_Previous_Balance,CR_Cas_Contract_Basic_Contarct_Remaining_Amount,CR_Cas_Contract_Basic_Statistics_Nationalities,CR_Cas_Contract_Basic_Statistics_Country,CR_Cas_Contract_Basic_Statistics_Gender,CR_Cas_Contract_Basic_Statistics_Jobs,CR_Cas_Contract_Basic_Statistics_Regions_Branch,CR_Cas_Contract_Basic_Statistics_City_Branch,CR_Cas_Contract_Basic_Statistics_Regions_Renter,CR_Cas_Contract_Basic_Statistics_City_Renter,CR_Cas_Contract_Basic_Statistics_Brand,CR_Cas_Contract_Basic_Statistics_Model,CR_Cas_Contract_Basic_Statistics_Year,CR_Cas_Contract_Basic_Statistics_Membership_Code,CR_Cas_Contract_Basic_Statistics_Day_No,CR_Cas_Contract_Basic_Statistics_Time_No,CR_Cas_Contract_Basic_Statistics_Day_Count,CR_Cas_Contract_Basic_Statistics_Age_No,CR_Cas_Contract_Basic_Statistics_Value_No,CR_Cas_Contract_Basic_Statistics_KM,CR_Cas_Contract_Basic_User_Close")] CR_Cas_Contract_Basic cR_Cas_Contract_Basic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cR_Cas_Contract_Basic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CR_Cas_Contract_Basic_Car_Price_Basic_No = new SelectList(db.CR_Cas_Car_Price_Basic, "CR_Cas_Car_Price_Basic_No", "CR_Cas_Car_Price_Basic_Year", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Price_Basic_No);
            ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Long_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor);
            ViewBag.CR_Cas_Contract_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Sector);
            ViewBag.CR_Cas_Contract_Basic_Renter_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id);
            ViewBag.CR_Cas_Contract_Basic_Car_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Serail_No);
            return View(cR_Cas_Contract_Basic);
        }

        // GET: ContractSettlement/Delete/5
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

        // POST: ContractSettlement/Delete/5
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
