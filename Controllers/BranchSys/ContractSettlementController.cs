using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using iTextSharp.text;
using iTextSharp.text.pdf;
using RentCar.Models;
using RentCar.Models.RptModels;
using System.Threading;
using System.Web.UI;
using System.Diagnostics.Contracts;

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
            && (c.CR_Cas_Contract_Basic_Status == "A" || c.CR_Cas_Contract_Basic_Status == "E")
            && (c.CR_Cas_Contract_Basic_Owner_Branch == BranchCode || c.CR_Cas_Contract_Basic_is_Receiving_Branch == true))
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
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.FirstOrDefault(c => c.CR_Cas_Contract_Basic_No == id1);
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

                var CurrentHour = TimeSpan.Parse(DateTime.Now.ToString("HH:mm:ss"));
                var EndTime = TimeSpan.Parse(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Time.ToString());
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
                //ViewBag.FreeAdditionalHours = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Free_Additional_Hours;
                ViewBag.AdditionalHours = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Hour_Max;
                var MaxHours = Convert.ToInt32(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Hour_Max);
                //if (CurrentHour > EndTime)
                //{
                //    TimeSpan freeH = TimeSpan.FromHours(Convert.ToDouble(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Free_Additional_Hours));
                //    var H = CurrentHour - EndTime;
                //    var r = H - freeH;
                //    ViewBag.AdditionalHours = r.Hours;
                //    if(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Hour_Max - cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Free_Additional_Hours > r.Hours)
                //    {
                //        AddDays = 0;
                //    }
                //    else
                //    {
                //        AddDays = 1;
                //    }


                //}
                //else
                //{
                //    ViewBag.AdditionalHours = "0";
                //}




                ViewBag.AdditionalKmValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_KM_Value;
                ViewBag.ExtraHourValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Extra_Hour_Value;

                var branch = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == BranchCode && b.CR_Cas_Sup_Lessor_Code == LessorCode);
                if (branch != null)
                {
                    ViewBag.BranchReceipt = branch.CR_Cas_Sup_Branch_Ar_Name;
                }

                var branchDelivery = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Branch_Code == cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Owner_Branch && b.CR_Cas_Sup_Lessor_Code == LessorCode);
                if (branchDelivery != null)
                {
                    ViewBag.DeliveryBranch = branchDelivery.CR_Cas_Sup_Branch_Ar_Name;
                }
                ViewBag.AuthEndDate = DateTime.Now.ToString("yyyy/MM/dd h:m:s tt");

                if (cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_is_Renter_Driver == true)
                {
                    ViewBag.AdditionalDriverVal = 0;
                }
                else
                {
                    ViewBag.AdditionalDriverVal = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_Driver_Value;
                }
                //if (Session["ContractCancel"].ToString() == "True")
                //{
                //    ViewBag.ContractEndDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date);
                //    ViewBag.ContractCancel = "True";
                //}
                //else
                //{
                //    if (cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date > DateTime.Now)
                //    {
                //        ViewBag.ContractEndDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date);
                //        ViewBag.ContractCancel = "False";
                //    }
                //    else
                //    {
                //        ViewBag.ContractEndDate = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
                //        ViewBag.ContractCancel = "True";
                //    }
                //}
                ViewBag.ContractEndDateEx = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
                ViewBag.ContractEndTimeEx = DateTime.Now.ToString("HH:mm:ss");

                ViewBag.ContractEndTime = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Time;
                ViewBag.ContractEndDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date);


                var CurrentDate = DateTime.Now.ToShortDateString();
                DateTime CDate = Convert.ToDateTime(CurrentDate);
                var StartDate = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Start_Date;
                DateTime Sdate = Convert.ToDateTime(StartDate);
                var nbrdays = (CDate - Sdate).Days;
                if (nbrdays == 0)
                {
                    nbrdays = nbrdays + 1 + AddDays;
                    ViewBag.ContractDaysNbr = 1;
                    //ViewBag.AdditionalHours = 0;
                }
                else
                {
                    ViewBag.ContractDaysNbr = nbrdays + AddDays;
                    //ViewBag.AdditionalHours = 0;
                }


                if (nbrdays < 7)
                {
                    ViewBag.DailyRentValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Rent;

                }
                else if (nbrdays >= 7 && nbrdays < 30)
                {
                    ViewBag.DailyRentValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Weekly_Rent;

                }
                else
                {
                    ViewBag.DailyRentValue = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Monthly_Rent;

                }
                ViewBag.ExContractDaysNbr = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_Rental_Days;
                ViewBag.ChoicesVal = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Choices_Value * nbrdays;
                ViewBag.AdditionalVal = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_Value;
                var nbDays = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_Rental_Days;
                var DailyFreeKm = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Free_KM;
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

                var user = db.CR_Cas_User_Information.FirstOrDefault(u => u.CR_Cas_User_Information_Id == UserLogin);
                if (user != null)
                {
                    if (user.CR_Cas_User_Information_Balance == null)
                    {
                        ViewBag.UserBalance = 0;
                    }
                    else
                    {
                        var convertUserBalanceToFloat = (float)user.CR_Cas_User_Information_Balance;
                        var Receipt_Receipt = convertUserBalanceToFloat.ToString("N0");
                        ViewBag.UserBalance = Receipt_Receipt;
                    }

                }

                ViewBag.PayType = new SelectList(db.CR_Mas_Sup_Payment_Method.Where(p => p.CR_Mas_Sup_Payment_Method_Type == "2" && p.CR_Mas_Sup_Payment_Method_Status == "A")
                , "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Ar_Name");
                ViewBag.CasherName = "";
            }
            return View(cR_Cas_Contract_Basic);
        }

        // POST: ContractSettlement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string ContractNo, string ContractEndDate, string ContractEndTime, string ContractDaysNbr, string CarSerialNo, string ContractEndDateEx, string ContractEndTimeEx, string ContractValED, string ContractValID, string TaxVal,
           string TotalContractIT, string TotPayed, string CurrentMeter, string OldKm, string TotalFreeKm, decimal? AdditionalHours, string ExtraKmValue, string TotalHoursValue, string Chk_Depences, string Chk_Compensation,
           string Depences, string DepencesReason, string CompensationVal, string CompensationReason, string RenterPrevBalance, string reste, string TotToPay, string PayType, string CasherName, string remarque,string AdditionalKmNo,string ExContractDaysNbr,string FreeAdditionalHours, string BranchReceipt,string DeliveryBranch,string Discount,
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
                                RedirectToAction("Login", "Account");
                            }
                        }
                        catch
                        {
                            RedirectToAction("Login", "Account");
                        }
                        var Contract = db.CR_Cas_Contract_Basic.Single(c => c.CR_Cas_Contract_Basic_No == ContractNo && c.CR_Cas_Contract_Basic_Status != "y");
                        if (Contract != null)
                        {
                            DateTime StartDate = DateTime.Now;
                            string CurrentTime = DateTime.Now.ToString("HH:mm:ss");
                            var StartTime = TimeSpan.Parse(CurrentTime);

                            Contract.CR_Cas_Contract_Basic_Status = "C";
                            Contract.CR_Cas_Contract_Basic_Receiving_Branch = BranchCode;
                            if (ContractEndDate != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Expected_End_Date = DateTime.Now.Date;
                            }
                            if (ContractEndTime != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Expected_End_Time = TimeSpan.Parse(CurrentTime);
                            }
                            if (ContractDaysNbr != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Expected_Rental_Days = int.Parse(ContractDaysNbr);
                            }

                            Contract.CR_Cas_Contract_Basic_End_Authorization = DateTime.Now;
                            Contract.CR_Cas_Contract_BasicAuthorization_Staus = "B";
                            if (ContractValED != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Value = Convert.ToDecimal(ContractValED);
                            }
                            if (ContractValED != "" && ContractValID != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Discount_Value = Convert.ToDecimal(ContractValED) - Convert.ToDecimal(ContractValID);
                            }
                            if (TaxVal != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Tax_Value = Convert.ToDecimal(TaxVal);
                            }
                            if (TotalContractIT != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Net_Value = Convert.ToDecimal(TotalContractIT);
                            }
                            if (TotPayed=="")
                            {
                                TotPayed = "0";
                            }
                            if (TotPayed != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Payed_Value = Convert.ToDecimal(TotPayed);
                            }
                            if (CurrentMeter != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Delivery_Reading_KM = Convert.ToInt32(CurrentMeter);
                            }
                            if (TotalFreeKm != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Actual_Additional_Free_KM = Convert.ToInt32(TotalFreeKm);
                            }
                            if (AdditionalHours != null)
                            {
                                Contract.CR_Cas_Contract_Basic_Additional_Hours = Convert.ToInt32(AdditionalHours);
                            }
                            if (ExtraKmValue != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Actual_Extra_KM_Value = Convert.ToDecimal(ExtraKmValue);
                            }
                            if (TotalHoursValue != "")
                            {
                                Contract.CR_Cas_Contract_Basic_Actual_Extra_Hour_Value = Convert.ToDecimal(TotalHoursValue);
                            }

                            if (Chk_Depences == "false" || Chk_Depences == null)
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_is_Expenses = "0";
                            }
                            else
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_is_Expenses = "1";
                            }
                            if (Contract.CR_Cas_Contract_Basic_Contarct_is_Expenses == "1")
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_Expenses_Value = Depences;
                                Contract.CR_Cas_Contract_Basic_Contarct_Expenses_Description = DepencesReason;
                            }
                            else
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_Expenses_Value = "0";
                                Contract.CR_Cas_Contract_Basic_Contarct_Expenses_Description = "";
                            }


                            if (Chk_Compensation == "false" || Chk_Compensation == null)
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_is_Compensation = "0";
                            }
                            else
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_is_Compensation = "1";
                            }

                            if (Contract.CR_Cas_Contract_Basic_Contarct_is_Compensation == "1")
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_Compensation_Value = CompensationVal;
                                Contract.CR_Cas_Contract_Basic_Contarct_Compensation_Description = CompensationReason;
                            }
                            else
                            {
                                Contract.CR_Cas_Contract_Basic_Contarct_Compensation_Value = "0";
                                Contract.CR_Cas_Contract_Basic_Contarct_Compensation_Description = "";
                            }
                            

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

                            if (CurrentMeter != "" && OldKm != "" && ContractDaysNbr != "")
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
                                else if (RealKm >= 201 && RealKm < 300)
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
                            CR_Cas_Account_Tax_Owed TaxOwed = new CR_Cas_Account_Tax_Owed();
                            TaxOwed.CR_Cas_Account_Tax_Owed_Contract_No = Contract.CR_Cas_Contract_Basic_No;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Com_Code = LessorCode;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Brn_Code = BranchCode;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Value = Contract.CR_Cas_Contract_Basic_Tax_Value;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Due_Date = DateTime.Now;
                            TaxOwed.CR_Cas_Account_Tax_Owed_Is_Paid = false;
                            db.CR_Cas_Account_Tax_Owed.Add(TaxOwed);
                            

                            // Renter Lessor /////////
                            var CasRenter = db.CR_Cas_Renter_Lessor.FirstOrDefault(r => r.CR_Cas_Renter_Lessor_Id == Contract.CR_Cas_Contract_Basic_Renter_Id && r.CR_Cas_Renter_Lessor_Code == LessorCode);
                            var CasRenterContracts = db.CR_Cas_Contract_Basic.Where(x => x.CR_Cas_Contract_Basic_Renter_Id == Contract.CR_Cas_Contract_Basic_Renter_Id && x.CR_Cas_Contract_Basic_Lessor == LessorCode && x.CR_Cas_Contract_Basic_Status != "U");
                            if (CasRenter != null)
                            {

                                if (CasRenterContracts != null)
                                {
                                    int allDaysForRenter = 0;
                                    int allkms = 0;
                                    foreach (var item in CasRenterContracts)
                                    {
                                        allDaysForRenter += (int)item.CR_Cas_Contract_Basic_Expected_Rental_Days;
                                        if (item.CR_Cas_Contract_Basic_Actual_Total_KM != null)
                                        {
                                            allkms += (int)item.CR_Cas_Contract_Basic_Actual_Total_KM;
                                        }
                                    }
                                    CasRenter.CR_Cas_Renter_Lessor_Days = allDaysForRenter;
                                    CasRenter.CR_Cas_Renter_Lessor_Interaction_Amount_Value = Convert.ToDecimal(TotalContractIT);
                                    CasRenter.CR_Cas_Renter_Lessor_KM = allkms;
                                    CasRenter.CR_Cas_Renter_Lessor_Status = "A";
                                    if (reste != null && reste != "")
                                    {
                                        CasRenter.CR_Cas_Renter_Lessor_Balance = Convert.ToDecimal(reste);
                                    }
                                    CasRenter.CR_Cas_Renter_Lessor_Date_Last_Interaction = DateTime.Now.Date;
                                    CasRenter.CR_Cas_Renter_Lessor_Statistics_Nationalities = Contract.CR_Cas_Contract_Basic_Statistics_Nationalities;
                                    CasRenter.CR_Cas_Renter_Lessor_Statistics_Country = Contract.CR_Cas_Contract_Basic_Statistics_Country;
                                    CasRenter.CR_Cas_Renter_Lessor_Statistics_Gender = Contract.CR_Cas_Contract_Basic_Statistics_Gender;
                                    CasRenter.CR_Cas_Renter_Lessor_Statistics_Jobs = Contract.CR_Cas_Contract_Basic_Statistics_Jobs;
                                    CasRenter.CR_Cas_Renter_Lessor_Statistics_Regions = Contract.CR_Cas_Contract_Basic_Statistics_Regions_Renter;
                                    CasRenter.CR_Cas_Renter_Lessor_Statistics_Age = Contract.CR_Cas_Contract_Basic_Statistics_Age_No;
                                    CasRenter.CR_Cas_Renter_Lessor_Statistics_City = Contract.CR_Cas_Contract_Basic_Statistics_City_Renter;


                                }
                            }
                            // Renter Information for All Company /////////

                            var CasRenterComs = db.CR_Cas_Renter_Lessor.Where(r => r.CR_Cas_Renter_Lessor_Id == Contract.CR_Cas_Contract_Basic_Renter_Id);
                            var CasRenterinfo = db.CR_Mas_Renter_Information.FirstOrDefault(r => r.CR_Mas_Renter_Information_Id == Contract.CR_Cas_Contract_Basic_Renter_Id);

                            var CasRenterContractsComs = db.CR_Cas_Contract_Basic.Where(x => x.CR_Cas_Contract_Basic_Renter_Id == Contract.CR_Cas_Contract_Basic_Renter_Id);
                            if (CasRenterComs != null)
                            {
                                var contractNo = 0;
                                var contractTotVal = 0;
                                var dayNoTotal = 0;

                                foreach (var item in CasRenterComs)
                                {
                                    contractNo += (int)item.CR_Cas_Renter_Lessor_Contract_Number;
                                    contractTotVal += (int)item.CR_Cas_Renter_Lessor_Interaction_Amount_Value;
                                    dayNoTotal += (int)item.CR_Cas_Renter_Lessor_Days;
                                }
                                if (CasRenterinfo != null)
                                {
                                    CasRenterinfo.CR_Mas_Renter_Information_Contract_Number = contractNo;
                                    CasRenterinfo.CR_Mas_Renter_Information_Value = Convert.ToDecimal(contractTotVal);
                                    CasRenterinfo.CR_Mas_Renter_Information_Date_Last_Interaction = DateTime.Now.Date;
                                    CasRenterinfo.CR_Mas_Renter_Information_Days = dayNoTotal;
                                }
                            }

                            // Create ACcount Receipt ' مستند صرف ' if totaltoPay < 0
                            CR_Cas_Account_Receipt Receipt = new CR_Cas_Account_Receipt();
                            if (Convert.ToDecimal(TotPayed) > 0)
                            {
                                if (Convert.ToDecimal(TotToPay) < 0)
                                {
                                    DateTime year = DateTime.Now;
                                    var y = year.ToString("yy");
                                    var Sector = "1";
                                    var autoinc = GetReceiptLastRecord(LessorCode, BranchCode).CR_Cas_Account_Receipt_No;
                                    Receipt.CR_Cas_Account_Receipt_No = y + "-" + Sector + "-" + "61" + "-" + LessorCode + "-" + BranchCode + autoinc;

                                    Receipt.CR_Cas_Account_Receipt_Year = y;
                                    Receipt.CR_Cas_Account_Receipt_Type = "61";
                                    Receipt.CR_Cas_Account_Receipt_Lessor_Code = LessorCode;
                                    Receipt.CR_Cas_Account_Receipt_Branch_Code = BranchCode;
                                    Receipt.CR_Cas_Account_Receipt_Date = DateTime.Now;
                                    Receipt.CR_Cas_Account_Receipt_Contract_Operation = Contract.CR_Cas_Contract_Basic_No;
                                    Receipt.CR_Cas_Account_Receipt_Payment = 0;
                                    Receipt.CR_Cas_Account_Receipt_Receipt = Convert.ToDecimal(TotPayed);
                                    Receipt.CR_Cas_Account_Receipt_Payment_Method = PayType;
                                    /////////////////////////////////Update Sales Point//////////////////////
                                    var salesPoint = db.CR_Cas_Sup_SalesPoint.Single(s => s.CR_Cas_Sup_SalesPoint_Code == CasherName);
                                    if (salesPoint != null)
                                    {
                                        Receipt.CR_Cas_Account_Receipt_SalesPoint_No = CasherName;
                                        Receipt.CR_Cas_Account_Receipt_Bank_Code = salesPoint.CR_Cas_Sup_SalesPoint_Bank_Code;
                                        Receipt.CR_Cas_Account_Receipt_SalesPoint_Previous_Balance = salesPoint.CR_Cas_Sup_SalesPoint_Balance;
                                        salesPoint.CR_Cas_Sup_SalesPoint_Balance -= Convert.ToDecimal(TotPayed);
                                        db.Entry(salesPoint).State = EntityState.Modified;
                                    }
                                    /////////////////////////////////Update Cas User Information//////////////////////
                                    var userinfo = db.CR_Cas_User_Information.Single(u => u.CR_Cas_User_Information_Id == UserLogin);
                                    if (userinfo != null)
                                    {
                                        Receipt.CR_Cas_Account_Receipt_User_Previous_Balance = userinfo.CR_Cas_User_Information_Balance;
                                        userinfo.CR_Cas_User_Information_Balance -= Convert.ToDecimal(TotPayed);
                                        db.Entry(userinfo).State = EntityState.Modified;
                                    }
                                    //////////////////////////////////////////////////////////////////////////////////
                                    Receipt.CR_Cas_Account_Receipt_Renter_Code = Contract.CR_Cas_Contract_Basic_Renter_Id;
                                    Receipt.CR_Cas_Account_Receipt_Renter_Previous_Balance = Convert.ToDecimal(RenterPrevBalance);
                                    Receipt.CR_Cas_Account_Receipt_User_Code = UserLogin;
                                    //Receipt.CR_Cas_Account_Receipt_User_Previous_Balance = 0;
                                    Receipt.CR_Cas_Account_Receipt_Car_Code = Contract.CR_Cas_Contract_Basic_Car_Serail_No;
                                    Receipt.CR_Cas_Account_Receipt_Status = "A";
                                    Receipt.CR_Cas_Account_Receipt_Reference_Type = "عقد";
                                    Receipt.CR_Cas_Account_Receipt_Is_Passing = "1";
                                    Receipt.CR_Cas_Account_Receipt_Reasons = remarque;
                                    db.CR_Cas_Account_Receipt.Add(Receipt);
                                }
                                if (Convert.ToDecimal(TotToPay) > 0)
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
                                    Receipt.CR_Cas_Account_Receipt_Payment = Convert.ToDecimal(TotPayed);
                                    Receipt.CR_Cas_Account_Receipt_Receipt = 0;
                                    Receipt.CR_Cas_Account_Receipt_Payment_Method = PayType;
                                    /////////////////////////////////Update Sales Point//////////////////////
                                    var salesPoint = db.CR_Cas_Sup_SalesPoint.Single(s => s.CR_Cas_Sup_SalesPoint_Code == CasherName);
                                    if (salesPoint != null)
                                    {
                                        Receipt.CR_Cas_Account_Receipt_SalesPoint_No = CasherName;
                                        Receipt.CR_Cas_Account_Receipt_Bank_Code = salesPoint.CR_Cas_Sup_SalesPoint_Bank_Code;
                                        Receipt.CR_Cas_Account_Receipt_SalesPoint_Previous_Balance = salesPoint.CR_Cas_Sup_SalesPoint_Balance;
                                        salesPoint.CR_Cas_Sup_SalesPoint_Balance -= Convert.ToDecimal(TotPayed);
                                        db.Entry(salesPoint).State = EntityState.Modified;
                                    }
                                    /////////////////////////////////Update Cas User Information//////////////////////
                                    var userinfo = db.CR_Cas_User_Information.Single(u => u.CR_Cas_User_Information_Id == UserLogin);
                                    if (userinfo != null)
                                    {
                                        Receipt.CR_Cas_Account_Receipt_User_Previous_Balance = userinfo.CR_Cas_User_Information_Balance;
                                        userinfo.CR_Cas_User_Information_Balance += Convert.ToDecimal(TotPayed);
                                        db.Entry(userinfo).State = EntityState.Modified;
                                    }
                                    //////////////////////////////////////////////////////////////////////////////////
                                    Receipt.CR_Cas_Account_Receipt_Renter_Code = Contract.CR_Cas_Contract_Basic_Renter_Id;
                                    Receipt.CR_Cas_Account_Receipt_Renter_Previous_Balance = Convert.ToDecimal(RenterPrevBalance);
                                    Receipt.CR_Cas_Account_Receipt_User_Code = UserLogin;
                                    //Receipt.CR_Cas_Account_Receipt_User_Previous_Balance = 0;
                                    Receipt.CR_Cas_Account_Receipt_Car_Code = Contract.CR_Cas_Contract_Basic_Car_Serail_No;
                                    Receipt.CR_Cas_Account_Receipt_Status = "A";
                                    Receipt.CR_Cas_Account_Receipt_Reference_Type = "عقد";
                                    Receipt.CR_Cas_Account_Receipt_Is_Passing = "1";
                                    Receipt.CR_Cas_Account_Receipt_Reasons = remarque;
                                    db.CR_Cas_Account_Receipt.Add(Receipt);
                                }
                            }
                           
                            var receiptNo = db.CR_Cas_Account_Receipt.FirstOrDefault(c => c.CR_Cas_Account_Receipt_Contract_Operation == Contract.CR_Cas_Contract_Basic_No);

                            var carInfo = db.CR_Cas_Sup_Car_Information.FirstOrDefault(c => c.CR_Cas_Sup_Car_Lessor_Code == LessorCode && c.CR_Cas_Sup_Car_Location_Branch_Code == BranchCode && c.CR_Cas_Sup_Car_Serail_No == Contract.CR_Cas_Contract_Basic_Car_Serail_No);
                            if (carInfo!=null)
                            {
                                carInfo.CR_Cas_Sup_Car_Status = "A";
                            }



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



                            string fname = Contract.CR_Cas_Contract_Basic_No + ".pdf";
                            string fullpath = CreateCopyFolder + fname;
                            




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
                            if (Contract.CR_Cas_Contract_Basic_Contarct_is_Compensation=="0")
                            {
                                imgy4path = "";
                                imgy3path = "";
                                imgy2path = "";
                                imgy1path = "";
                            }
                            if (Contract.CR_Cas_Contract_Basic_Contarct_is_Expenses=="0")
                            {
                                imgx1path = "";
                                imgx2path = "";
                                imgx3path = "";
                                imgx4path = "";
                            }


                            /////////////////////////////////////////////////////////////////////////
                            SavePdf(Contract, fullpath, ContractEndDateEx, ContractEndTimeEx, ContractNo, CarSerialNo, ContractEndDate, ContractEndTime, ContractDaysNbr, ContractValED, ContractValID, TaxVal,
                                       TotalContractIT, TotPayed, CurrentMeter, OldKm, TotalFreeKm, AdditionalHours, ExtraKmValue, TotalHoursValue, Chk_Depences, Chk_Compensation,
                                       Depences, DepencesReason, CompensationVal, CompensationReason, RenterPrevBalance, reste, TotToPay, PayType, CasherName, remarque, AdditionalKmNo, ExContractDaysNbr, Receipt.CR_Cas_Account_Receipt_No, FreeAdditionalHours, AdditionalHours.ToString(), BranchReceipt, DeliveryBranch, Discount,
                                       imgx1path, imgx2path, imgx3path, imgx4path, imgy1path, imgy2path, imgy3path, imgy4path, img1path, img2path, img3path, img4path, img5path, img6path, img7path, img8path, img9path);

                            //Contract.CR_Cas_Contract_Basic_CreateContract_Pdf = fullpath;
                            if (ViewBag.ShowToastr==true)
                            {
                                TempData["toastr"] = true;
                                return RedirectToAction("BranchStat", "BranchHome");
                            }
                            db.Entry(Contract).State = EntityState.Modified;
                            db.SaveChanges();
                            TempData["TempModel"] = "Saved";
                            dbTran.Commit();
                            //SendMail(Contract);



                        }

                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        throw ex;
                    }
                    return RedirectToAction("BranchStat","BranchHome");
                }
            }

            ViewBag.PayType = new SelectList(db.CR_Mas_Sup_Payment_Method.Where(p => p.CR_Mas_Sup_Payment_Method_Type == "2" && p.CR_Mas_Sup_Payment_Method_Status == "A")
                , "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Ar_Name");
            ViewBag.CasherName = "";
            return View();
        }

        private void SavePdf(CR_Cas_Contract_Basic contract,string fullpath, string ContractEndDateEx, string ContractEndTimeEx, string contractNo, string carSerialNo, string contractEndDate, string contractEndTime, string contractDaysNbr, string contractValED, string contractValID, string taxVal, string totalContractIT, string totPayed, string currentMeter, string oldKm, string totalFreeKm, decimal? additionalHours, string extraKmValue, string totalHoursValue, string chk_Depences, string chk_Compensation, string depences, string depencesReason, string compensationVal, string compensationReason, string renterPrevBalance, string reste, string totToPay, string payType, string casherName, string remarque,string AdditionalKmNo,string ExContractDaysNbr,string ReceiptNo,string FreeAdditionalHours, string AdditionalHours,string BranchReceipt,string DeliveryBranch , string Discount,
            string imgx1path, string imgx2path, string imgx3path, string imgx4path, string imgy1path, string imgy2path, string imgy3path, string imgy4path, string img1, string img2, string img3, string img4, string img5, string img6, string img7, string img8, string img9)
        {
            var LessorCode = Session["LessorCode"].ToString();
            var BranchCode = Session["BranchCode"].ToString();
            var UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/ContractBasicReports/ContractCr"), "ContractClose.rpt"));

            try
            {
                List<VirtualInspectionRptMD> VirtualInspectionMD = new List<VirtualInspectionRptMD>();
                var Vinspection = db.CR_Cas_Contract_Virtual_Inspection.Where(a => a.CR_Cas_Contract_Virtual_Inspection_No == contractNo);
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

                if (contract != null)
                {
                    var branch = db.CR_Cas_Sup_Branch.FirstOrDefault(b => b.CR_Cas_Sup_Lessor_Code == LessorCode && b.CR_Cas_Sup_Branch_Code == BranchCode);
                    var lessor = db.CR_Mas_Com_Lessor.FirstOrDefault(l => l.CR_Mas_Com_Lessor_Code == contract.CR_Cas_Contract_Basic_Lessor);
                    if (lessor != null)
                    {
                        rd.SetParameterValue("CompanyName", lessor.CR_Mas_Com_Lessor_Ar_Long_Name.Trim());
                        rd.SetParameterValue("CompanyNameEng", lessor.CR_Mas_Com_Lessor_En_Long_Name.Trim());
                        rd.SetParameterValue("CompanyAuthNo", lessor.CR_Mas_Com_Lessor_Commercial_Registration_No.Trim());
                        rd.SetParameterValue("ContractNo", contract.CR_Cas_Contract_Basic_No.Trim());
                        rd.SetParameterValue("ContractDate", contract.CR_Cas_Contract_Basic_Date.ToString());
                        if (contractEndDate!=null)
                        {
                            rd.SetParameterValue("contractEndDate", contractEndDate.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("contractEndDate", "  ");
                        }
                        if (contractEndTime != null)
                        {
                            rd.SetParameterValue("contractEndTime", contractEndTime.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("contractEndTime", "  ");
                        }
                        if (lessor.CR_Mas_Com_Lessor_Commercial_Registration_No != null)
                        {
                            rd.SetParameterValue("CommercialRegisterNo", lessor.CR_Mas_Com_Lessor_Commercial_Registration_No.Trim());
                        }
                        else
                        {
                            rd.SetParameterValue("CommercialRegisterNo", "        ");
                        }
                        var address = db.CR_Mas_Address.FirstOrDefault(adr => adr.CR_Mas_Address_Id_Code == lessor.CR_Mas_Com_Lessor_Government_No);
                        if (address != null)
                        {
                            string street = address.CR_Mas_Address_Ar_Street;
                            string district = address.CR_Mas_Address_Ar_District;
                            string buildingNo = address.CR_Mas_Address_Building.ToString();
                            string UnitNo = address.CR_Mas_Address_Unit_No;
                            string ZipCode = address.CR_Mas_Address_Zip_Code.ToString();
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

                            string addr = reg + " " + cit + " " + district + " " + street + " " + buildingNo + " " + UnitNo + " " + ZipCode;

                            rd.SetParameterValue("CompanyAddress", addr.Trim());


                        }
                        else
                        {
                            rd.SetParameterValue("CompanyAddress", "");
                        }

                        if (lessor.CR_Mas_Com_Lessor_Tax_Number != null)
                        {
                            rd.SetParameterValue("TaxNumber", lessor.CR_Mas_Com_Lessor_Tax_Number.Trim());
                        }
                        else
                        {
                            rd.SetParameterValue("TaxNumber", "     ");
                        }

                        if (lessor.CR_Mas_Com_Lessor_Signature_Ar_Director_Name != null)
                        {
                            rd.SetParameterValue("DirectorName", lessor.CR_Mas_Com_Lessor_Signature_Ar_Director_Name.Trim());
                        }
                        else
                        {
                            rd.SetParameterValue("DirectorName", "     ");
                        }



                        if (branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name != null)
                        {
                            rd.SetParameterValue("BranchDirectorName", branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name.Trim());
                        }
                        else
                        {
                            rd.SetParameterValue("BranchDirectorName", "    ");
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

                        var logo = lessor.CR_Mas_Com_Lessor_Logo_Print.ToString();
                        var log = logo.Replace("~", "");
                        log = log.Replace("/", "\\");
                        log = log.Substring(1, log.Length - 1);
                        var lm = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + log;
                        if (logo != null && lm != null)
                        {
                            rd.SetParameterValue("Logo", lm.Trim().ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("Logo", "   ");
                        }


                        if (lessor.CR_Mas_Com_Lessor_Tolk_Free != null)
                        {
                            rd.SetParameterValue("FreeTall", lessor.CR_Mas_Com_Lessor_Tolk_Free.Trim());
                        }
                        else
                        {
                            rd.SetParameterValue("FreeTall", "    ");
                        }
                        if (branch != null)
                        {
                            rd.SetParameterValue("BranchName", branch.CR_Cas_Sup_Branch_Ar_Name.Trim());
                            rd.SetParameterValue("BranchContact", branch.CR_Cas_Sup_Branch_Tel.Trim());
                            var Branchaddress = db.CR_Mas_Address.FirstOrDefault(adr => adr.CR_Mas_Address_Id_Code == branch.CR_Cas_Sup_Branch_Government_No);
                            if (Branchaddress != null)
                            {
                                string street = Branchaddress.CR_Mas_Address_Ar_Street;
                                string district = Branchaddress.CR_Mas_Address_Ar_District;
                                string buildingNo = Branchaddress.CR_Mas_Address_Building.ToString();
                                string UnitNo = Branchaddress.CR_Mas_Address_Unit_No;
                                string ZipCode = Branchaddress.CR_Mas_Address_Zip_Code.ToString();
                                string reg = "";
                                string cit = "";
                                var region = db.CR_Mas_Sup_Regions.FirstOrDefault(r => r.CR_Mas_Sup_Regions_Code == Branchaddress.CR_Mas_Address_Regions);
                                if (region != null)
                                {
                                    reg = region.CR_Mas_Sup_Regions_Ar_Name;
                                }
                                var city = db.CR_Mas_Sup_City.FirstOrDefault(c => c.CR_Mas_Sup_City_Code == Branchaddress.CR_Mas_Address_City);
                                if (city != null)
                                {
                                    cit = city.CR_Mas_Sup_City_Ar_Name;
                                }

                                string addr = reg + " " + cit + " " + district + " " + street + " " + buildingNo + " " + UnitNo + " " + ZipCode;

                                rd.SetParameterValue("BranchAddress", addr.Trim());
                            }
                            else
                            {
                                rd.SetParameterValue("BranchAddress", "");

                            }
                        }
                        else
                        {
                            rd.SetParameterValue("BranchAddress", "");

                        }

                        rd.SetParameterValue("contractSerialNo", contract.CR_Cas_Contract_Basic_Car_Serail_No);
                        rd.SetParameterValue("CusId", contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Id);
                        rd.SetParameterValue("contractEndDateActual", ContractEndDateEx);

                        var carinfo = db.CR_Cas_Sup_Car_Information.FirstOrDefault(c=>c.CR_Cas_Sup_Car_Serail_No== contract.CR_Cas_Contract_Basic_Car_Serail_No);
                        if (carinfo!=null)
                        {
                            if (carinfo.CR_Cas_Sup_Car_Collect_Ar_Name != null)
                            {
                                rd.SetParameterValue("CarName", carinfo.CR_Cas_Sup_Car_Collect_Ar_Name.ToString());
                            }
                            else
                            {
                                rd.SetParameterValue("CarName", "   ");
                            }

                        }
                        if (contract.CR_Cas_Contract_Basic_Actual_Extra_Hour_Value != null)
                        {
                            rd.SetParameterValue("ExtraHourValue", contract.CR_Cas_Contract_Basic_Actual_Extra_Hour_Value.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("ExtraHourValue", "0");
                        }

                        if (contract.CR_Cas_Contract_Basic_End_Authorization != null)
                        {
                            rd.SetParameterValue("AuthEndDate", contract.CR_Cas_Contract_Basic_End_Authorization.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("AuthEndDate", "0");
                        }
                        if (contract.CR_Cas_Contract_Basic_Daily_Rent != null)
                        {
                            rd.SetParameterValue("DailyRentPrice", contract.CR_Cas_Contract_Basic_Daily_Rent.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("DailyRentPrice", "0");
                        }

                        if (ContractEndTimeEx != null)
                        {
                            rd.SetParameterValue("contractEndTimeActual", ContractEndTimeEx.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("contractEndTimeActual", "0");
                        }
                        if (contractDaysNbr != null)
                        {
                            rd.SetParameterValue("ActualDays", contractDaysNbr);
                        }
                        else
                        {
                            rd.SetParameterValue("ActualDays", "0");
                        }
                        if (ExContractDaysNbr != null)
                        {
                            rd.SetParameterValue("ExContractDaysNbr", ExContractDaysNbr);
                        }
                        else
                        {
                            rd.SetParameterValue("ExContractDaysNbr", " ");
                        }
                        if (FreeAdditionalHours != null)
                        {
                            rd.SetParameterValue("FreeAdditionalHours", FreeAdditionalHours);
                        }
                        else
                        {
                            rd.SetParameterValue("ExContractDaysNbr", "0");
                        }
                        if (AdditionalHours != null)
                        {
                            rd.SetParameterValue("AdditionalHours", AdditionalHours);
                        }
                        else
                        {
                            rd.SetParameterValue("AdditionalHours", "0");
                        }

                        if (BranchReceipt != null)
                        {
                            rd.SetParameterValue("BranchReceipt", BranchReceipt.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("BranchReceipt", "  ");
                        }
                        if (DeliveryBranch != null)
                        {
                            rd.SetParameterValue("DeliveryBranch", DeliveryBranch.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("DeliveryBranch", "  ");
                        }
                        if (Discount != null)
                        {
                            rd.SetParameterValue("Discount", Discount.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("Discount", "0");
                        }



                        //********************************
                        if (ReceiptNo!=null)
                        {
                            rd.SetParameterValue("ReceiptNo", ReceiptNo);
                        }
                        else
                        {
                            rd.SetParameterValue("ReceiptNo", "  ");
                        }


                        if (contract.CR_Cas_Contract_Basic_Previous_Balance != null)
                        {
                            rd.SetParameterValue("PreviousBalance", contract.CR_Cas_Contract_Basic_Previous_Balance.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("PreviousBalance", "0");
                        }

                        if (contract.CR_Cas_Contract_Basic_Additional_Driver_Value != null)
                        {
                            rd.SetParameterValue("ValueAdditionalDriver", contract.CR_Cas_Contract_Basic_Additional_Driver_Value.ToString());

                        }
                        else
                        {
                            rd.SetParameterValue("ValueAdditionalDriver", "0");
                        }

                        if (contract.CR_Cas_Contract_Basic_Authorization_Value != null)
                        {
                            rd.SetParameterValue("AuthorizationPercentage", contract.CR_Cas_Contract_Basic_Authorization_Value.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("AuthorizationPercentage", "0");
                        }
                        if (contract.CR_Cas_Contract_Basic_User_Discount != null)
                        {
                            rd.SetParameterValue("DiscountPercentage", contract.CR_Cas_Contract_Basic_User_Discount.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("DiscountPercentage", "0");
                        }
                        if (contractValED != null)
                        {
                            rd.SetParameterValue("contractValueBeforeDis", contractValED);
                        }
                        else
                        {
                            rd.SetParameterValue("contractValueBeforeDis", "0");
                        }
                        if (contract.CR_Cas_Contract_Basic_Tax_Value != null)
                        {
                            rd.SetParameterValue("TaxValue", contract.CR_Cas_Contract_Basic_Tax_Value.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("TaxValue", "0");
                        }
                        if (contractValID != null)
                        {
                            rd.SetParameterValue("contractValueAfterDIs", contractValID.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("contractValueAfterDIs", "0");
                        }

                        if (payType != null && payType != "")
                        {
                            var PayMethod = db.CR_Mas_Sup_Payment_Method.FirstOrDefault(m => m.CR_Mas_Sup_Payment_Method_Code == payType);
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

                        if (casherName != null && casherName != "")
                        {
                            var casher = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(c => c.CR_Cas_Sup_SalesPoint_Code == casherName);
                            if (casher != null && casher.CR_Cas_Sup_SalesPoint_Bank_Code != LessorCode + "0000")
                            {
                                rd.SetParameterValue("casherName", casher.CR_Cas_Sup_SalesPoint_Ar_Name.Trim());
                            }
                            else
                            {
                                rd.SetParameterValue("casherName", "    ");
                            }
                        }
                        else
                        {
                            rd.SetParameterValue("casherName", "    ");
                        }


                        var date = DateTime.Now;
                        if (date != null)
                        {
                            rd.SetParameterValue("Date", date.ToString("yyyy/MM/dd"));
                        }
                        else
                        {
                            rd.SetParameterValue("Date", "  ");
                        }

                        if (totToPay != null)
                        {
                            rd.SetParameterValue("TotalToPay", totToPay);
                        }
                        else
                        {
                            rd.SetParameterValue("TotalToPay", "0");
                        }
                        if (contract.CR_Cas_Contract_Basic_Additional_Value != null)
                        {
                            rd.SetParameterValue("AdditionalTotal", contract.CR_Cas_Contract_Basic_Additional_Value.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("AdditionalTotal", "0");
                        }
                        if (contract.CR_Cas_Contract_Basic_Choices_Value != null)
                        {
                            rd.SetParameterValue("ChoicesTotal", contract.CR_Cas_Contract_Basic_Choices_Value.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("ChoicesTotal", "0");
                        }
                        if (contract.CR_Cas_Contract_Basic_Actual_Extra_Hour_Value != null)
                        {
                            rd.SetParameterValue("ExtraHourValue", contract.CR_Cas_Contract_Basic_Actual_Extra_Hour_Value.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("ExtraHourValue", "");
                        }

                        if (contract.CR_Cas_Contract_Basic_Additional_KM_Value != null)
                        {
                            rd.SetParameterValue("KmValue", contract.CR_Cas_Contract_Basic_Additional_KM_Value.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("KmValue", "0");
                        }
                        if (contract.CR_Cas_Contract_Basic_Contarct_Remaining_Amount != null)
                        {
                            rd.SetParameterValue("remains", contract.CR_Cas_Contract_Basic_Contarct_Remaining_Amount.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("remains", "0");
                        }
                        if (contract.CR_Cas_Contract_Basic_Daily_Rent != null)
                        {
                            rd.SetParameterValue("Price", contract.CR_Cas_Contract_Basic_Daily_Rent.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("Price", "0");
                        }
                        
                        var Stamp = lessor.CR_Mas_Com_Lessor_Stamp;
                        var stm = Stamp.Replace("~", "");
                        stm = stm.Replace("/", "\\");
                        stm = stm.Substring(1, stm.Length - 1);
                        var stp = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + stm;
                        rd.SetParameterValue("CompanyStamp", stp);


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



                        if (contract.CR_Cas_Contract_Basic_Net_Value != null)
                        {
                            rd.SetParameterValue("ContractNetValue", contract.CR_Cas_Contract_Basic_Net_Value);
                        }
                        else
                        {
                            rd.SetParameterValue("ContractNetValue", "0");
                        }
                        if (totPayed != null)
                        {
                            rd.SetParameterValue("amountPaid", totPayed);
                        }
                        else
                        {
                            rd.SetParameterValue("amountPaid", "0");
                        }
                        if (remarque != null)
                        {
                            rd.SetParameterValue("Reasons", remarque);
                        }
                        else
                        {
                            rd.SetParameterValue("Reasons", "0");
                        }

                        ////depences ===>> expenses
                        if (chk_Depences == "false" || chk_Depences == null)
                        {
                            rd.SetParameterValue("expensesval", "0");
                            rd.SetParameterValue("expensesstat", " ");
                            rd.SetParameterValue("expenseImg1", "         ");
                            rd.SetParameterValue("expenseImg2", "         ");
                            rd.SetParameterValue("expenseImg3", "         ");
                            rd.SetParameterValue("expenseImg4", "         ");
                        }
                        else
                        {
                            if (depences != null)
                            {
                                rd.SetParameterValue("expensesval", depences);
                            }
                            else
                            {
                                rd.SetParameterValue("expensesval", "0");
                            }

                            if (depencesReason != null)
                            {
                                rd.SetParameterValue("expensesstat", depencesReason);
                            }
                            else
                            {
                                rd.SetParameterValue("expensesstat", " ");
                            }

                            if (imgy1path != null && imgy1path != "")
                            {
                                imgy1path = imgy1path.Replace("/", "\\");
                                imgy1path = imgy1path.Substring(1, imgy1path.Length - 1);
                                var img1path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imgy1path;
                                rd.SetParameterValue("expenseImg1", img1path);
                            }
                            else
                            {
                                rd.SetParameterValue("expenseImg1", "         ");
                            }

                            if (imgy2path != null && imgy2path != "")
                            {
                                imgy2path = imgy2path.Replace("/", "\\");
                                imgy2path = imgy2path.Substring(1, imgy2path.Length - 1);
                                var img2path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imgy2path;
                                rd.SetParameterValue("expenseImg2", img2path);
                            }
                            else
                            {
                                rd.SetParameterValue("expenseImg2", "         ");
                            }

                            if (imgy3path != null && imgy3path != "")
                            {
                                imgy3path = imgy2path.Replace("/", "\\");
                                imgy3path = imgy2path.Substring(1, imgy3path.Length - 1);
                                var img3path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imgy3path;
                                rd.SetParameterValue("expenseImg3", img3path);
                            }
                            else
                            {
                                rd.SetParameterValue("expenseImg3", "         ");
                            }

                            if (imgy4path != null && imgy4path != "")
                            {
                                imgy4path = imgy4path.Replace("/", "\\");
                                imgy4path = imgy4path.Substring(1, imgy4path.Length - 1);
                                var img4path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imgy4path;
                                rd.SetParameterValue("expenseImg4", img4path);
                            }
                            else
                            {
                                rd.SetParameterValue("expenseImg4", "         ");
                            }

                        }




                        //compestaion
                        if (chk_Compensation == "false" || chk_Compensation == null)
                        {
                            rd.SetParameterValue("compensationVal", "0");
                            rd.SetParameterValue("compensationstat", " ");
                            rd.SetParameterValue("compensationImg1", "         ");
                            rd.SetParameterValue("compensationImg2", "         ");
                            rd.SetParameterValue("compensationImg3", "         ");
                            rd.SetParameterValue("compensationImg4", "         ");
                        }
                        else
                        {
                            if (compensationVal != null)
                            {
                                rd.SetParameterValue("compensationVal", compensationVal);
                            }
                            else
                            {
                                rd.SetParameterValue("compensationVal", "0");
                            }

                            if (compensationReason != null)
                            {
                                rd.SetParameterValue("compensationstat", compensationReason);
                            }
                            else
                            {
                                rd.SetParameterValue("compensationstat", " ");
                            }

                            if (imgx1path != null && imgx1path != "")
                            {
                                imgx1path = imgx1path.Replace("/", "\\");
                                imgx1path = imgx1path.Substring(1, imgx1path.Length - 1);
                                var img1path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imgx1path;
                                rd.SetParameterValue("compensationImg1", img1path);
                            }
                            else
                            {
                                rd.SetParameterValue("compensationImg1", "         ");
                            }

                            if (imgx2path != null && imgx2path != "")
                            {
                                imgx2path = imgx2path.Replace("/", "\\");
                                imgx2path = imgx2path.Substring(1, imgx2path.Length - 1);
                                var img1path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imgx2path;
                                rd.SetParameterValue("compensationImg2", img1path);
                            }
                            else
                            {
                                rd.SetParameterValue("compensationImg2", "         ");
                            }

                            if (imgx3path != null && imgx3path != "")
                            {
                                imgx3path = imgx3path.Replace("/", "\\");
                                imgx3path = imgx3path.Substring(1, imgx3path.Length - 1);
                                var img1path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imgx3path;
                                rd.SetParameterValue("compensationImg3", img1path);
                            }
                            else
                            {
                                rd.SetParameterValue("compensationImg3", "         ");
                            }

                            if (imgx4path != null && imgx4path != "")
                            {
                                imgx4path = imgx4path.Replace("/", "\\");
                                imgx4path = imgx4path.Substring(1, imgx4path.Length - 1);
                                var img1path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + imgx4path;
                                rd.SetParameterValue("compensationImg4", img1path);
                            }
                            else
                            {
                                rd.SetParameterValue("compensationImg4", "         ");
                            }
                        }
                            

                        //inspection
                        if (currentMeter != null)
                        {
                            rd.SetParameterValue("CurrentMeter", currentMeter);
                        }
                        else
                        {
                            rd.SetParameterValue("CurrentMeter", "0");
                        }
                        if (oldKm != null)
                        {
                            rd.SetParameterValue("OldKm", oldKm);
                        }
                        else
                        {
                            rd.SetParameterValue("OldKm", "0");
                        }

                        if (totalFreeKm != null)
                        {
                            rd.SetParameterValue("TotalFreeKm", totalFreeKm);
                        }
                        else
                        {
                            rd.SetParameterValue("TotalFreeKm", "0");
                        }
                        if (AdditionalKmNo != null)
                        {
                            rd.SetParameterValue("AdditionalKmNo", AdditionalKmNo.ToString());
                        }
                        else
                        {
                            rd.SetParameterValue("AdditionalKmNo", "0");
                        }

                        if (img1 != null)
                        {
                            img1 = img1.Replace("/", "\\");
                            img1 = img1.Substring(1, img1.Length - 1);
                            var img1path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img1;
                            rd.SetParameterValue("insImg1", img1path);
                        }
                        else
                        {
                            rd.SetParameterValue("insImg1", "");
                        } 
                        if (img2 != null)
                        {
                            img2 = img2.Replace("/", "\\");
                            img2 = img2.Substring(1, img2.Length - 1);
                            var img2path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img2;
                            rd.SetParameterValue("insImg2", img2path);
                        }
                        else
                        {
                            rd.SetParameterValue("insImg2", "");
                        }
                        if (img3 != null)
                        {
                            img3 = img3.Replace("/", "\\");
                            img3 = img3.Substring(1, img3.Length - 1);
                            var img3path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img3;
                            rd.SetParameterValue("insImg3", img3path);
                        }
                        else
                        {
                            rd.SetParameterValue("insImg3", "");
                        }
                        if (img4 != null)
                        {
                            img4 = img4.Replace("/", "\\");
                            img4 = img4.Substring(1, img4.Length - 1);
                            var img4path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img4;
                            rd.SetParameterValue("insImg4", img4path);
                        }
                        else
                        {
                            rd.SetParameterValue("insImg4", "");
                        } 
                        if (img5 != null)
                        {
                            img5 = img5.Replace("/", "\\");
                            img5 = img5.Substring(1, img5.Length - 1);
                            var img5path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img5;
                            rd.SetParameterValue("insImg5", img5path);
                        }
                        else
                        {
                            rd.SetParameterValue("insImg5", "");
                        } 
                        
                        if (img6 != null)
                        {
                            img6 = img6.Replace("/", "\\");
                            img6 = img6.Substring(1, img6.Length - 1);
                            var img6path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img6;
                            rd.SetParameterValue("insImg6", img6path);
                        }
                        else
                        {
                            rd.SetParameterValue("insImg6", "");
                        } 
                        if (img7 != null)
                        {
                            img7 = img7.Replace("/", "\\");
                            img7 = img7.Substring(1, img7.Length - 1);
                            var img7path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img7;
                            rd.SetParameterValue("insImg7", img7path);
                        }
                        else
                        {
                            rd.SetParameterValue("insImg7", "");
                        }
                        if (img8 != null)
                        {
                            img8 = img8.Replace("/", "\\");
                            img8 = img8.Substring(1, img8.Length - 1);
                            var img8path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img8;
                            rd.SetParameterValue("insImg8", img8path);
                        }
                        else
                        {
                            rd.SetParameterValue("insImg8", "");
                        }
                        if (img9 != null)
                        {
                            img9 = img9.Replace("/", "\\");
                            img9 = img9.Substring(1, img9.Length - 1);
                            var img9path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + img9;
                            rd.SetParameterValue("insImg9", img9path);
                        }
                        else
                        {
                            rd.SetParameterValue("insImg9", "");
                        }

                        var x = "/images/Company/" + LessorCode + "/" + BranchCode + "/" + contract.CR_Cas_Contract_Basic_No + "/" + "ClosePDF" + "/" + contract.CR_Cas_Contract_Basic_Copy + "/" + contract.CR_Cas_Contract_Basic_No + ".pdf";
                        var fullPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + x;
                        fullPath = fullPath.Replace("/", "\\");
                        rd.ExportToDisk(ExportFormatType.PortableDocFormat, fullPath);


                        var firstPath = contract.CR_Cas_Contract_Basic_CreateContract_Pdf;
                        var secondPath = fullPath;

                        string[] paths = { firstPath, secondPath };
                        var output = firstPath;
                        MergePDFs(paths, output + "1");


                    }

                }

            }
            catch (Exception)
            {
                 ViewBag.ShowToastr = true;
                 return;
            }

        }


        public void MergePDFs(string[] fileNames, string outFile)
        {

            
                using (var stream = new FileStream(outFile, FileMode.Append, FileAccess.Write))
                {
                    var document = new Document();
                    var writer = new PdfCopy(document, stream);
                    document.Open();

                    foreach (string fileName in fileNames)
                    {
                        using (var reader = new PdfReader(fileName))
                        {
                            writer.AddDocument(reader);
                        }
                    }

                    document.Close();
                    stream.Close();
                }

                Thread.Sleep(1000);


                if (System.IO.File.Exists(outFile.Remove(outFile.Length - 1)))
                {
                    // Open the file stream with FileShare.ReadWrite to allow other processes to read or write to the file
                    using (var fileStream = System.IO.File.Open(outFile, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                    {
                        // Perform operations on the stream
                        // ...

                        // Close the stream
                        fileStream.Close();
                    }
                    System.IO.File.Delete(outFile.Remove(outFile.Length - 1));
                // Retry deleting the file
                    bool deleteSuccess = false;
                    int retryCount = 3;
                    while (!deleteSuccess && retryCount > 0)
                    {
                        try
                        {
                            System.IO.File.Delete(outFile.Remove(outFile.Length - 1));
                            deleteSuccess = true;
                        }
                        catch (IOException)
                        {
                            // Failed to delete the file, retry after a delay
                            Thread.Sleep(1000);
                            retryCount--;
                        }
                    }
                }

                if (System.IO.File.Exists(outFile))
                {
                    System.IO.File.Move(outFile, outFile.Remove(outFile.Length - 1));
                }
            
        }

        private void SendMail(CR_Cas_Contract_Basic contract)
        {
            string projectFolder = Server.MapPath(string.Format("~/{0}/", "images"));
            string image1 = Path.Combine(projectFolder, "End.jpeg");


            string htmlBody = "<html><body><h1>اغلاق العقد</h1><br><img src=cid:Contract style='width:100%;height:100%'></body></html>";

            htmlBody = System.Web.HttpUtility.HtmlEncode(htmlBody); // Encode the HTML string
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString
               (htmlBody, null, MediaTypeNames.Text.Html);
            LinkedResource inline = new LinkedResource(image1, MediaTypeNames.Image.Jpeg);
            inline.ContentId = "Contract"; // Replace the non-ASCII characters with an ASCII string
            avHtml.LinkedResources.Add(inline);

            MailMessage mail = new MailMessage();
            mail.AlternateViews.Add(avHtml);

            Attachment attachment = new Attachment(contract.CR_Cas_Contract_Basic_CreateContract_Pdf);
            mail.Attachments.Add(attachment);

            mail.From = new MailAddress("bnanbnanout@outlook.com");


            if (contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email != null)
            {
                mail.To.Add("bnanbnanmail@gmail.com");
            }
            mail.Subject = "اغلاق العقد";
            mail.Body = inline.ContentId;
            mail.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("bnanbnanout@outlook.com", "bnan123123");

            // Send the message
            smtpClient.Send(mail);

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
