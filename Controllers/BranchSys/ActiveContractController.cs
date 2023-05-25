using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using iTextSharp.text;
using iTextSharp.text.pdf;
using RentCar.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Font = System.Drawing.Font;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace RentCar.Controllers.BranchSys
{
    public class ActiveContractController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: ActiveContract
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
            var cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Status == "A" &&
            c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Owner_Branch == BranchCode && c.CR_Cas_Contract_Basic_Expected_End_Date>=DateTime.Now)
                .Include(l=>l.CR_Mas_Com_Lessor)
                .Include(c=>c.CR_Mas_Sup_Sector);
            return View(cR_Cas_Contract_Basic.ToList());
        }

        public FileResult ShowPDF(string Cno)
        {
            string path = "";
            var contract = db.CR_Cas_Contract_Basic.OrderByDescending(c => c.CR_Cas_Contract_Basic_Copy).FirstOrDefault(c => c.CR_Cas_Contract_Basic_No == Cno);
            if (contract != null)
            {
                path = contract.CR_Cas_Contract_Basic_CreateContract_Pdf;
            }
            string mime = MimeMapping.GetMimeMapping(path);

            return File(path, mime);
        }




        // GET: ActiveContract/Details/5
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

        // GET: ActiveContract/Create
        public ActionResult Create()
        {
            ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Logo");
            return View();
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


        // POST: ActiveContract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CR_Cas_Contract_Basic_No,CR_Cas_Contract_Basic_Copy," +
            "CR_Cas_Contract_Basic_Year,CR_Cas_Contract_Basic_Type,CR_Cas_Contract_Basic_Lessor,CR_Cas_Contract_Basic_Sector," +
            "CR_Cas_Contract_Basic_Owner_Branch,CR_Cas_Contract_Basic_Date,CR_Cas_Contract_Basic_Time,CR_Cas_Contract_Basic_Renter_Id," +
            "CR_Cas_Contract_Basic_Car_Serail_No,CR_Cas_Contract_Basic_Driver_Id,CR_Cas_Contract_Basic_Additional_Driver_Id," +
            "CR_Cas_Contract_Basic_Start_Date,CR_Cas_Contract_Basic_Start_Time,CR_Cas_Contract_Basic_Expected_End_Date," +
            "CR_Cas_Contract_Basic_Expected_End_Time,CR_Cas_Contract_Basic_Expected_Rental_Days,CR_Cas_Contract_Basic_is_Renter_Driver," +
            "CR_Cas_Contract_Basic_is_Additional_Driver,CR_Cas_Contract_Basic_Additional_Driver_Value,CR_Cas_Contract_Basic_Authorization_Value," +
            "CR_Cas_Contract_Basic_Use_Within_Country,CR_Cas_Contract_Basic_Daily_Rent,CR_Cas_Contract_Basic_Weekly_Rent," +
            "CR_Cas_Contract_Basic_Monthly_Rent,CR_Cas_Contract_Basic_Additional_Value,CR_Cas_Contract_Basic_Choices_Value," +
            "CR_Cas_Contract_Basic_Tax_Rate,CR_Cas_Contract_Basic_User_Discount,CR_Cas_Contract_Basic_Daily_Free_KM,CR_Cas_Contract_Basic_Additional_KM_Value," +
            "CR_Cas_Contract_Basic_Free_Additional_Hours,CR_Cas_Contract_Basic_Hour_Max,CR_Cas_Contract_Basic_Extra_Hour_Value,CR_Cas_Contract_Basic_is_Km_Open," +
            "CR_Cas_Contract_Basic_is_Receiving_Branch,CR_Cas_Contract_Basic_Value,CR_Cas_Contract_Basic_Discount_Value,CR_Cas_Contract_Basic_After_Discount_Value," +
            "CR_Cas_Contract_Basic_Tax_Value,CR_Cas_Contract_Basic_Net_Value,CR_Cas_Contract_Basic_Payed_Value,CR_Cas_Contract_Basic_User_Insert," +
            "CR_Cas_Contract_Basic_Car_Price_Basic_No,CR_Cas_Contract_Basic_Status")] CR_Cas_Contract_Basic cR_Cas_Contract_Basic,
            string TaxVal,string ContractDiscountVal,string ContractVal,string NetVal,string PayedVal, string EndDate , string ExpectedRentalDays,
            string totalDays,string TotalChoices, string ContractET,string CasherName, string Reasons, string CR_Cas_Contract_Basic_Previous_Balance, string PayType,string TotalToPay)
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            if (ModelState.IsValid)
            {
                
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

                        //////////////////////////////Create Contract/////////////////////////////////////
                        DateTime year = DateTime.Now;
                        var y = year.ToString("yy");
                        //var cont = db.CR_Cas_Contract_Basic.OrderByDescending(c => c.CR_Cas_Contract_Basic_No == cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_No).FirstOrDefault();
                        var cont =db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_No == cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_No).
                            OrderByDescending(d => d.CR_Cas_Contract_Basic_Copy).FirstOrDefault();
                        if (cont != null)
                        {                                                                                                    
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Copy = cont.CR_Cas_Contract_Basic_Copy+1;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Year = int.Parse(y);
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Type = 90;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor = LessorCode;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Sector = cont.CR_Cas_Contract_Basic_Sector;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Owner_Branch = cont.CR_Cas_Contract_Basic_Owner_Branch;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Date = DateTime.Now;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Time = cont.CR_Cas_Contract_Basic_Time;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id = cont.CR_Cas_Contract_Basic_Renter_Id;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_is_Renter_Driver = cont.CR_Cas_Contract_Basic_is_Renter_Driver;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Time = cont.CR_Cas_Contract_Basic_Time;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Driver_Id = cont.CR_Cas_Contract_Basic_Driver_Id;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Serail_No = cont.CR_Cas_Contract_Basic_Car_Serail_No;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_is_Additional_Driver = cont.CR_Cas_Contract_Basic_is_Additional_Driver;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_Driver_Id = cont.CR_Cas_Contract_Basic_Additional_Driver_Id;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Start_Date = DateTime.Now;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Start_Time = cont.CR_Cas_Contract_Basic_Start_Time;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Time = cont.CR_Cas_Contract_Basic_Time;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_is_Receiving_Renter=cont.CR_Cas_Contract_Basic_is_Receiving_Renter;


                            if (EndDate!=null && EndDate != "")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date = cont.CR_Cas_Contract_Basic_Start_Date?.AddDays(int.Parse(totalDays));
                            }
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Time = cont.CR_Cas_Contract_Basic_Expected_End_Time;
                            if(ExpectedRentalDays!=null && ExpectedRentalDays != "")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_Rental_Days =int.Parse(totalDays);
                            }
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_Driver_Value = cont.CR_Cas_Contract_Basic_Additional_Driver_Value;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Time = cont.CR_Cas_Contract_Basic_Time;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Authorization_Value = cont.CR_Cas_Contract_Basic_Authorization_Value;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Use_Within_Country = cont.CR_Cas_Contract_Basic_Use_Within_Country;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Rent = cont.CR_Cas_Contract_Basic_Daily_Rent;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Weekly_Rent = cont.CR_Cas_Contract_Basic_Weekly_Rent;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Monthly_Rent = cont.CR_Cas_Contract_Basic_Monthly_Rent;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_Value = cont.CR_Cas_Contract_Basic_Additional_Value ;


                            if (TotalChoices != null && TotalChoices != "")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Choices_Value = cont.CR_Cas_Contract_Basic_Choices_Value;
                            }

                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Tax_Rate = cont.CR_Cas_Contract_Basic_Tax_Rate;

                            if(TaxVal!=null && TaxVal != "")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Tax_Value = decimal.Parse(TaxVal)+ cont.CR_Cas_Contract_Basic_Tax_Value;
                            }
                    
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_User_Discount = cont.CR_Cas_Contract_Basic_User_Discount;
                            if(ContractDiscountVal!=null && ContractDiscountVal != "")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Discount_Value = decimal.Parse(ContractDiscountVal) + cont.CR_Cas_Contract_Basic_Discount_Value;
                            }

                            if(ContractVal!=null && ContractVal != "")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Value = decimal.Parse(ContractVal);
                            }


                            if (ContractET != null && ContractET != "")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_After_Discount_Value = decimal.Parse(ContractET) + cont.CR_Cas_Contract_Basic_After_Discount_Value;
                            }

                            if (NetVal!=null && NetVal != "")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Net_Value = decimal.Parse(NetVal);
                            }

                            if(PayedVal!=null && PayedVal != "")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Payed_Value = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Payed_Value + decimal.Parse(PayedVal);
                            }


                            decimal PayedValue = 0;
                            if (PayedVal != "")
                            {
                                PayedValue = decimal.Parse(PayedVal);
                            }

                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Free_KM = cont.CR_Cas_Contract_Basic_Daily_Free_KM;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_KM_Value = cont.CR_Cas_Contract_Basic_Additional_KM_Value;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Free_Additional_Hours = cont.CR_Cas_Contract_Basic_Free_Additional_Hours;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Hour_Max = cont.CR_Cas_Contract_Basic_Hour_Max;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Extra_Hour_Value = cont.CR_Cas_Contract_Basic_Extra_Hour_Value;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_is_Km_Open = cont.CR_Cas_Contract_Basic_is_Km_Open;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_is_Receiving_Branch = cont.CR_Cas_Contract_Basic_is_Receiving_Branch;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Time = cont.CR_Cas_Contract_Basic_Time;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_User_Insert = UserLogin;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Price_Basic_No = cont.CR_Cas_Contract_Basic_Car_Price_Basic_No;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Status = "A";


                            if (cont == null)
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Previous_Balance = 0;
                            }
                            else 
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Previous_Balance = Convert.ToDecimal(CR_Cas_Contract_Basic_Previous_Balance);
                            }

                            //cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Previous_Balance =cont.CR_Cas_Contract_Basic_Previous_Balance;// + PayedValue * (-1)
                            //Renewed contract

                            ////////////////////////////////alerts/////////////////////////////
                            //var currentdate = DateTime.Now;
                            //var AuthEndDate = currentdate.AddDays(30);
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_End_Authorization = cont.CR_Cas_Contract_Basic_End_Authorization;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_CurrentMeters = cont.CR_Cas_Contract_Basic_CurrentMeters;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_User_Discount = cont.CR_Cas_Contract_Basic_User_Discount;

                            cR_Cas_Contract_Basic.CR_Cas_Contract_BasicAuthorization_Staus = "T";
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Alert_Status = cont.CR_Cas_Contract_Basic_Alert_Status;

                            // Statictis
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Nationalities = cont.CR_Cas_Contract_Basic_Statistics_Nationalities;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Country = cont.CR_Cas_Contract_Basic_Statistics_Country;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Gender = cont.CR_Cas_Contract_Basic_Statistics_Gender;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Jobs = cont.CR_Cas_Contract_Basic_Statistics_Jobs;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Regions_Branch = cont.CR_Cas_Contract_Basic_Statistics_Regions_Branch;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Regions_Renter = cont.CR_Cas_Contract_Basic_Statistics_Regions_Renter;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_City_Branch = cont.CR_Cas_Contract_Basic_Statistics_City_Branch;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_City_Renter = cont.CR_Cas_Contract_Basic_Statistics_City_Renter;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Brand = cont.CR_Cas_Contract_Basic_Statistics_Brand;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Model = cont.CR_Cas_Contract_Basic_Statistics_Model;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Year = cont.CR_Cas_Contract_Basic_Statistics_Year;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Membership_Code = cont.CR_Cas_Contract_Basic_Statistics_Membership_Code;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_KM = cont.CR_Cas_Contract_Basic_Statistics_KM;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Time_No = cont.CR_Cas_Contract_Basic_Statistics_Time_No;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Age_No = cont.CR_Cas_Contract_Basic_Statistics_Age_No;


                            ///////////////////////////////////////// Statistics_Day_No ////////////////////////////////////////////////////
                            var currentdate = DateTime.Today;
                            var name = currentdate.DayOfWeek.ToString();
                            if (name == "Monday")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_No = 3;
                            }
                            if (name == "Tuesday")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_No = 4;
                            }
                            if (name == "Wednesday")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_No = 5;
                            }
                            if (name == "Thursday")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_No = 6;
                            }
                            if (name == "Friday")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_No = 7;
                            }
                            if (name == "Saturday")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_No = 1;
                            }
                            if (name == "Sunday")
                            {
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_No = 2;
                            }
                            ///////////////////////////////////////// Statistics_Day_No ////////////////////////////////////////////////////
                            if (ExpectedRentalDays != "")
                            {
                                if (Convert.ToInt32(ExpectedRentalDays) <= 3)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_Count = 1;
                                }
                                else if (Convert.ToInt32(ExpectedRentalDays) > 3 && Convert.ToInt32(ExpectedRentalDays) <= 7)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_Count = 2;
                                }
                                else if (Convert.ToInt32(ExpectedRentalDays) >= 8 && Convert.ToInt32(ExpectedRentalDays) <= 10)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_Count = 3;
                                }
                                else if (Convert.ToInt32(ExpectedRentalDays) >= 11 && Convert.ToInt32(ExpectedRentalDays) <= 15)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_Count = 4;
                                }
                                else if (Convert.ToInt32(ExpectedRentalDays) >= 16 && Convert.ToInt32(ExpectedRentalDays) <= 25)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_Count = 5;
                                }
                                else if (Convert.ToInt32(ExpectedRentalDays) >= 26 && Convert.ToInt32(ExpectedRentalDays) <= 30)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_Count = 6;
                                }
                                else if (Convert.ToInt32(ExpectedRentalDays) > 30)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Day_Count = 7;
                                }
                            }
                            ///////////////////////////////////////// Statistics_Value_No ////////////////////////////////////////////////////

                            if (ContractET != "")
                            {
                                if (Convert.ToDecimal(ContractET) < 300)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Value_No = 1;
                                }
                                else if (Convert.ToDecimal(ContractET) <= 300 && Convert.ToDecimal(ContractET) <= 500)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Value_No = 2;
                                }
                                else if (Convert.ToDecimal(ContractET) <= 701 && Convert.ToDecimal(ContractET) <= 1000)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Value_No = 3;
                                }
                                else if (Convert.ToDecimal(ContractET) <= 1201 && Convert.ToDecimal(ContractET) <= 2000)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Value_No = 4;
                                }
                                else if (Convert.ToDecimal(ContractET) <= 2001 && Convert.ToDecimal(ContractET) <= 3000)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Value_No = 5;
                                }
                                else if (Convert.ToDecimal(ContractET) <= 3001 && Convert.ToDecimal(ContractET) <= 4000)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Value_No = 6;
                                }
                                else if (Convert.ToDecimal(ContractET) > 4000)
                                {
                                    cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Statistics_Value_No = 7;
                                }
                            }


                            //int expecteddays = int.Parse(ExpectedRentalDays);
                            if (cont.CR_Cas_Contract_Basic_Hour_DateTime_Alert != null)
                            {
                                DateTime dt = (DateTime)cont.CR_Cas_Contract_Basic_Hour_DateTime_Alert;
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Hour_DateTime_Alert = dt.AddDays(int.Parse(ExpectedRentalDays));
                            }
                           
    
                            if (cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_Rental_Days > 1)
                            {
                                var d = (DateTime)cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date;
                                var d1 = d.AddDays(-1);
                                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Day_DateTime_Alert = d1 + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Start_Time;

                            }

                            
                            decimal RenterBalance = 0;
                            var lessorRenter = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id;
                            /////////////////////////////////////Update Cas Renter Lessor///////////////////////
                            if (PayedValue > 0)
                            {
                                var RenterLessor = db.CR_Cas_Renter_Lessor.FirstOrDefault(r => r.CR_Cas_Renter_Lessor_Id == cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id 
                                && r.CR_Cas_Renter_Lessor_Code == LessorCode);
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
                                }
                            }
                            
                            ////////////////////////////////////CR_Cas_Account_Receipt//////////////////////////
                            
                           
                            if (PayedValue > 0)
                            {
                                CR_Cas_Account_Receipt Receipt = new CR_Cas_Account_Receipt();
                                var Sector = "1";
                                var autoinc = GetReceiptLastRecord(LessorCode, BranchCode).CR_Cas_Account_Receipt_No;
                                Receipt.CR_Cas_Account_Receipt_No = y + "-" + Sector + "-" + "60" + "-" + LessorCode + "-" + BranchCode + autoinc;
                                Receipt.CR_Cas_Account_Receipt_Year = y;
                                Receipt.CR_Cas_Account_Receipt_Type = "60";
                                Receipt.CR_Cas_Account_Receipt_Lessor_Code = LessorCode;
                                Receipt.CR_Cas_Account_Receipt_Branch_Code = BranchCode;
                                Receipt.CR_Cas_Account_Receipt_Date = DateTime.Now;
                                Receipt.CR_Cas_Account_Receipt_Contract_Operation = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_No;
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
                                    db.Entry(salesPoint).State = EntityState.Modified;
                                }
                                /////////////////////////////////Update Cas User Information//////////////////////
                                var userinfo = db.CR_Cas_User_Information.Single(u => u.CR_Cas_User_Information_Id == UserLogin);
                                if (userinfo != null)
                                {
                                    Receipt.CR_Cas_Account_Receipt_User_Previous_Balance = userinfo.CR_Cas_User_Information_Balance;
                                    userinfo.CR_Cas_User_Information_Balance += PayedValue;
                                    db.Entry(userinfo).State = EntityState.Modified;
                                }
                                //////////////////////////////////////////////////////////////////////////////////
                                Receipt.CR_Cas_Account_Receipt_Renter_Code = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id;
                                Receipt.CR_Cas_Account_Receipt_Renter_Previous_Balance = RenterBalance;
                                Receipt.CR_Cas_Account_Receipt_User_Code = UserLogin;
                                //Receipt.CR_Cas_Account_Receipt_User_Previous_Balance = 0;
                                Receipt.CR_Cas_Account_Receipt_Car_Code = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Serail_No;
                                Receipt.CR_Cas_Account_Receipt_Status = "A";
                                Receipt.CR_Cas_Account_Receipt_Reference_Type = "تمديد عقد";
                                Receipt.CR_Cas_Account_Receipt_Is_Passing = "1";
                                Receipt.CR_Cas_Account_Receipt_Reasons = Reasons;
                                db.CR_Cas_Account_Receipt.Add(Receipt);
                            }

                            /////////////////////////////////////////////////////////////////////////////////////


                            ///////////////////////////////////////////////////////////////////
                            /////////////////////////////////// Contract Tracing/////////////////////////////
                            //////CR_Cas_Administrative_Procedures Ad = new CR_Cas_Administrative_Procedures();
                            
                            //////var sector = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Sector.ToString();
                            //////var ProcedureCode = "91";
                            //////var autoInc = GetLastRecord(ProcedureCode, sector);
                            ////////var LessorCode = Session["LessorCode"].ToString();
                            //////Ad.CR_Cas_Administrative_Procedures_No = y + "-" + sector + "-" + ProcedureCode + "-" + LessorCode + "-" + BranchCode + GetLastRecord(ProcedureCode, sector).CR_Cas_Administrative_Procedures_No;
                            //////Ad.CR_Cas_Administrative_Procedures_Date = DateTime.Now;
                            //////Ad.CR_Cas_Administrative_Procedures_Doc_Date = DateTime.Now;
                            //////string currentTime = DateTime.Now.ToString("HH:mm:ss");
                            //////Ad.CR_Cas_Administrative_Procedures_Time = TimeSpan.Parse(currentTime);
                            //////Ad.CR_Cas_Administrative_Procedures_Year = y;
                            //////Ad.CR_Cas_Administrative_Procedures_Sector = sector;
                            //////Ad.CR_Cas_Administrative_Procedures_Code = ProcedureCode;
                            //////Ad.CR_Cas_Administrative_Int_Procedures_Code = int.Parse(ProcedureCode);
                            //////Ad.CR_Cas_Administrative_Procedures_Lessor = LessorCode;
                            //////Ad.CR_Cas_Administrative_Procedures_Targeted_Action = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_No;
                            //////Ad.CR_Cas_Administrative_Procedures_User_Insert = Session["UserLogin"].ToString();
                            //////Ad.CR_Cas_Administrative_Procedures_Type = "P";
                            //////Ad.CR_Cas_Administrative_Procedures_Action = true;
                            //////Ad.CR_Cas_Administrative_Procedures_Doc_Date = null;
                            //////Ad.CR_Cas_Administrative_Procedures_Doc_Start_Date = null;
                            //////Ad.CR_Cas_Administrative_Procedures_Doc_End_Date = null;
                            //////Ad.CR_Cas_Administrative_Procedures_Doc_No = "";
                            //////db.CR_Cas_Administrative_Procedures.Add(Ad);
                            ////////////////////////////////////////////////////////////////////////////////
                            ///
                        }

                        //////////////////////////////////////////////PDF////////////////////////////////////////
                        ///
                        string folderimages = Server.MapPath(string.Format("~/{0}/", "/images"));
                        string FolderContract = Server.MapPath(string.Format("~/{0}/", "/images/Contract"));
                        string FolderLessor = Server.MapPath(string.Format("~/{0}/", "/images/Contract/" + LessorCode));
                        string FolderBranch = Server.MapPath(string.Format("~/{0}/", "/images/Contract/" + LessorCode + "/" + BranchCode));
                        string FolderContractNo = Server.MapPath(string.Format("~/{0}/", "/images/Contract/" + LessorCode + "/" + BranchCode + "/" + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_No));
                        string OpenPdf = Server.MapPath(string.Format("~/{0}/", "/images/Contract/" + LessorCode + "/" + BranchCode + "/" + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_No + "/" + "OpenPdf"));
                        string CreateCopyFolder = Server.MapPath(string.Format("~/{0}/", "/images/Contract/" + LessorCode + "/" + BranchCode + "/" + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_No + "/" + "OpenPdf"
                            + "/" + cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Copy));

                        if (!Directory.Exists(folderimages))
                        {
                            Directory.CreateDirectory(folderimages);
                        }
                        if (!Directory.Exists(FolderContract))
                        {
                            Directory.CreateDirectory(FolderContract);
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

                        var Tprice = (int.Parse(ExpectedRentalDays) * cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Rent).ToString();

                        var CarPrice = db.CR_Cas_Car_Price_Basic.FirstOrDefault(price => price.CR_Cas_Car_Price_Basic_No == cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Price_Basic_No);
                        if (CarPrice != null)
                        {
                            ///
                            //////////////////////////////////////////////////////////////////////////////////
                            cont.CR_Cas_Contract_Basic_Status = "y";
                            db.Entry(cR_Cas_Contract_Basic).State = EntityState.Modified;
                            string fname = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_No + ".pdf";
                            string fullpath = CreateCopyFolder + fname;
                            cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_CreateContract_Pdf = fullpath;
                            db.CR_Cas_Contract_Basic.Add(cR_Cas_Contract_Basic);
                            db.SaveChanges();
                            TempData["TempModel"] = "Saved";
                            dbTran.Commit();
                           

                            SavePDF(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_No, CarPrice, fullpath, LessorCode, BranchCode, UserLogin,PayType,CasherName,Reasons,cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id,TotalToPay, PayedVal, Tprice);

                            return RedirectToAction("Index");

                        }

                        
                    }
                    catch (DbEntityValidationException ex)
                    {
                        dbTran.Rollback();
                        TempData["TempModel"] = "Error";
                        throw ex;
                    }
                }
            }
            ViewBag.PayType = new SelectList(db.CR_Mas_Sup_Payment_Method.Where(p => p.CR_Mas_Sup_Payment_Method_Type == "1" && p.CR_Mas_Sup_Payment_Method_Status == "A")
                 , "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Ar_Name");

            ViewBag.CasherName = new SelectList(db.CR_Cas_Sup_SalesPoint.Where(p =>p.CR_Cas_Sup_SalesPoint_Status == "A" 
            && p.CR_Cas_Sup_SalesPoint_Com_Code==LessorCode && p.CR_Cas_Sup_SalesPoint_Brn_Code==BranchCode)
                 , "CR_Cas_Sup_SalesPoint_Code", "CR_Cas_Sup_SalesPoint_Ar_Name");

            return View(cR_Cas_Contract_Basic);
        }

      /*  public FileResult PDFFlyer(string p)
        {
          
            string path = p;

            string mime = MimeMapping.GetMimeMapping(path);

            return File(path, mime);
        }*/

        private  void SavePDF(string Sno, CR_Cas_Car_Price_Basic CarPrice, string fullpath, string LessorCode, string BranchCode, string UserLogin ,string PayType , string CasherName ,string Reasons,string lessorRenter,string totalToPay,string payedValue,string price)
        {

            TempData["printCRCopy"] = fullpath;

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/ContractBasicReports/ContractCr"), "ContractRenew.rpt"));


            var Renter = db.CR_Cas_Renter_Lessor.FirstOrDefault(r => r.CR_Cas_Renter_Lessor_Id == lessorRenter
                                && r.CR_Cas_Renter_Lessor_Code == LessorCode);
            var contract = db.CR_Cas_Contract_Basic.OrderByDescending(x=>x.CR_Cas_Contract_Basic_Copy).FirstOrDefault(c => c.CR_Cas_Contract_Basic_No == Sno);
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
                    //rd.SetParameterValue("CommercialRegistration", lessor.CR_Mas_Com_Lessor_Commercial_Registration_No.Trim());
                    if (lessor.CR_Mas_Com_Lessor_Tax_Number != null)
                    {
                        rd.SetParameterValue("TaxNumber", lessor.CR_Mas_Com_Lessor_Tax_Number.Trim());
                    }
                    else
                    {
                        rd.SetParameterValue("TaxNumber", "     ");
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
                     }

                    // Car ///////////////////////


                    var car = db.CR_Cas_Sup_Car_Information.FirstOrDefault(c => c.CR_Cas_Sup_Car_Serail_No == contract.CR_Cas_Contract_Basic_Car_Serail_No);
                    if (car != null)
                    {
                        rd.SetParameterValue("CarSerialNo", car.CR_Cas_Sup_Car_Serail_No.Trim());
                        var CarName = car.CR_Mas_Sup_Brand.CR_Mas_Sup_Brand_Ar_Name.Trim() + " " + car.CR_Mas_Sup_Model.CR_Mas_Sup_Model_Ar_Name + " " + car.CR_Cas_Sup_Car_Year + " " +
                            car.CR_Mas_Sup_Color.CR_Mas_Sup_Color_Ar_Name.Trim() + " " +
                            car.CR_Mas_Sup_Category_Car.CR_Mas_Sup_Category.CR_Mas_Sup_Category_Ar_Name.Trim();
                        rd.SetParameterValue("CarName", CarName);

                        if (car.CR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Door_No!=null)
                        {
                            rd.SetParameterValue("CarDoorNo", car.CR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Door_No.ToString());

                        }
                        else
                        {
                            rd.SetParameterValue("CarDoorNo", " ");

                        }
                        if (car.CR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Passengers_No != null)
                        {
                            rd.SetParameterValue("CarPassengersNo", car.CR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Passengers_No.ToString());

                        }
                        else
                        {
                            rd.SetParameterValue("CarPassengersNo", " ");

                        }
                        if (car.CR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Bag_Bags != null)
                        {
                            rd.SetParameterValue("CarBigBagsNo", car.CR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Bag_Bags.ToString());

                        }
                        else
                        {
                            rd.SetParameterValue("CarBigBagsNo", " ");

                        }
                        if (car.CR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Small_Bags != null)
                        {
                            rd.SetParameterValue("CarSmallBagsNo", car.CR_Mas_Sup_Category_Car.CR_Mas_Sup_Category_Car_Small_Bags.ToString());

                        }
                        else
                        {
                            rd.SetParameterValue("CarSmallBagsNo", " ");

                        }

                        var cardocs = db.CR_Cas_Sup_Car_Doc_Mainten.Where(d => d.CR_Cas_Sup_Car_Doc_Mainten_Serail_No == car.CR_Cas_Sup_Car_Serail_No);
                        if (cardocs != null)
                        {
                            rd.SetParameterValue("PeriodicMaintenanceEndDate", "     ");
                            foreach (var docs in cardocs)
                            {

                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "29")
                                {
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                                    {
                                        rd.SetParameterValue("CarInspectionEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date?.ToString("yyyy/MM/dd"));
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("CarInspectionEndDate", "     ");
                                    }
                                   
                                }
                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "28")
                                {
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_No != null)
                                    {
                                        rd.SetParameterValue("TrafficPermit", docs.CR_Cas_Sup_Car_Doc_Mainten_No.Trim());
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("TrafficPermit", "     ");
                                    }

                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                                    {
                                        rd.SetParameterValue("TrafficPermitEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date?.ToString("yyyy/MM/dd"));
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("TrafficPermitEndDate", "     ");
                                    }

                                }

                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "27")
                                {
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_No != null)
                                    {
                                        rd.SetParameterValue("InsuranceDocNo", docs.CR_Cas_Sup_Car_Doc_Mainten_No.Trim());
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("InsuranceDocNo", "     ");
                                    }

                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                                    {
                                        rd.SetParameterValue("InsuranceEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date?.ToString("yyyy/MM/dd"));
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("InsuranceEndDate", "     ");
                                    }
                                    
                                }
                                if (docs.CR_Cas_Sup_Car_Doc_Mainten_Code == "26")
                                {
                                    if (docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date != null)
                                    {
                                        rd.SetParameterValue("CarDrivingLicenceEndDate", docs.CR_Cas_Sup_Car_Doc_Mainten_End_Date?.ToString("yyyy/MM/dd"));
                                    }
                                    else
                                    {
                                        rd.SetParameterValue("CarDrivingLicenceEndDate", "     ");
                                    }

                                }

                            }
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////

                    var LessorDirectorSignature = lessor.CR_Mas_Com_Lessor_Signature_Director;
                    if (LessorDirectorSignature != "" && LessorDirectorSignature != null)
                    {
                        var LDsignature = LessorDirectorSignature.Replace("~", "");
                        LDsignature = LDsignature.Replace("/", "\\");
                        LDsignature = LDsignature.Substring(1, LDsignature.Length - 1);
                        var LDS = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + LDsignature;
                        rd.SetParameterValue("LessorDirectorSignature", LDS);
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

                    }
                    var Stamp = lessor.CR_Mas_Com_Lessor_Stamp;
                    var stm = Stamp.Replace("~", "");
                    stm = stm.Replace("/", "\\");
                    stm = stm.Substring(1, stm.Length - 1);
                    var stp = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + stm;
                    rd.SetParameterValue("CompanyStamp", stp);

                    //////////
                    
                    var RenterSignature = Renter.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Signature;

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
                if (CarPrice != null)
                {
                    
                    rd.SetParameterValue("FreeKm", CarPrice.CR_Cas_Car_Price_Basic_No_Daily_Free_KM.ToString());
                    rd.SetParameterValue("KmValue", CarPrice.CR_Cas_Car_Price_Basic_Additional_KM_Value.ToString());
                    rd.SetParameterValue("HoursMax", CarPrice.CR_Cas_Car_Price_Basic_Hour_Max.ToString());
                    rd.SetParameterValue("DailyRentPrice", contract.CR_Cas_Contract_Basic_Daily_Rent.ToString());
                    rd.SetParameterValue("FreeHours", CarPrice.CR_Cas_Car_Price_Basic_No_Free_Additional_Hours.ToString());
                    rd.SetParameterValue("ExtraHourValue", CarPrice.CR_Cas_Car_Price_Basic_Extra_Hour_Value.ToString());

                    if (CarPrice.CR_Cas_Car_Price_Basic_Carrying_Accident!=null)
                    {
                        rd.SetParameterValue("ContractAccidentFees", CarPrice.CR_Cas_Car_Price_Basic_Carrying_Accident.ToString());
                    }
                    else
                    {
                        rd.SetParameterValue("ContractAccidentFees", " ");
                    }
                    if (CarPrice.CR_Cas_Car_Price_Basic_Stealing != null)
                    {
                        rd.SetParameterValue("ContractStealingFees", CarPrice.CR_Cas_Car_Price_Basic_Stealing.ToString());
                    }
                    else
                    {
                        rd.SetParameterValue("ContractStealingFees", " ");
                    }

                    if (CarPrice.CR_Cas_Car_Price_Basic_Drowning != null)
                    {
                        rd.SetParameterValue("ContractDrawningFees", CarPrice.CR_Cas_Car_Price_Basic_Drowning.ToString());
                    }
                    else
                    {
                        rd.SetParameterValue("ContractDrawningFees", " ");
                    }
                    if (CarPrice.CR_Cas_Car_Price_Basic_Rental_Tax_Rate != null)
                    {
                        rd.SetParameterValue("DiscountPercentage", CarPrice.CR_Cas_Car_Price_Basic_Rental_Tax_Rate.ToString());
                    }
                    else
                    {
                        rd.SetParameterValue("DiscountPercentage", " ");
                    }
                    if (CarPrice.CR_Cas_Car_Price_Basic_Carrying_Fire != null)
                    {
                        rd.SetParameterValue("ContractFireFees", CarPrice.CR_Cas_Car_Price_Basic_Carrying_Fire.ToString());
                    }
                    else
                    {
                        rd.SetParameterValue("ContractFireFees", " ");
                    }
                }


                var logo = lessor.CR_Mas_Com_Lessor_Logo_Print.ToString();
                var log = logo.Replace("~", "");
                log = log.Replace("/", "\\");
                log = log.Substring(1, log.Length - 1);
                var lm = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + log;
                rd.SetParameterValue("Logo", lm);



                if (contract.CR_Cas_Contract_Basic_Date != null)
                {
                    rd.SetParameterValue("ContractDate", contract.CR_Cas_Contract_Basic_Date?.ToString("yyyy/MM/dd"));
                }
                else
                {
                    rd.SetParameterValue("ContractDate", "     ");
                }

                if (contract.CR_Cas_Contract_Basic_Start_Date != null)
                {
                    rd.SetParameterValue("ContractStartDate", contract.CR_Cas_Contract_Basic_Start_Date?.ToString("yyyy/MM/dd"));
                }
                else
                {
                    rd.SetParameterValue("ContractStartDate", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_Expected_End_Date != null)
                {
                    rd.SetParameterValue("ContractEndDate", contract.CR_Cas_Contract_Basic_Expected_End_Date?.ToString("yyyy/MM/dd"));
                }
                else
                {
                    rd.SetParameterValue("ContractEndDate", "     ");
                }

                if (contract.CR_Cas_Contract_Basic_Start_Time != null)
                {
                    rd.SetParameterValue("ContractStartTime", contract.CR_Cas_Contract_Basic_Start_Time.ToString());
                }
                else
                {
                    rd.SetParameterValue("ContractStartTime", "    ");
                }

                if (contract.CR_Cas_Contract_Basic_Expected_End_Time != null)
                {
                    rd.SetParameterValue("ContractEndTime", contract.CR_Cas_Contract_Basic_Expected_End_Time.ToString());
                }
                else
                {
                    rd.SetParameterValue("ContractEndTime", "     ");
                }

                if (contract.CR_Cas_Contract_Basic_Use_Within_Country != null)
                {
                    if (contract.CR_Cas_Contract_Basic_Use_Within_Country == false)
                    {
                        rd.SetParameterValue("AuthType", "تفويض داخلي");
                    }
                    else
                    {
                        rd.SetParameterValue("AuthType", "تفويض خارجي");
                    }
                }
                else
                {
                    rd.SetParameterValue("AuthType", "      ");
                }
                
                if (contract.CR_Cas_Contract_Basic_End_Authorization != null)
                {
                    rd.SetParameterValue("AuthEndDate", contract.CR_Cas_Contract_Basic_End_Authorization?.ToString("yyyy/MM/dd"));
                }
                else
                {
                    rd.SetParameterValue("AuthEndDate", "    ");
                }

                if (contract.CR_Cas_Contract_Basic_Expected_Rental_Days != null)
                {
                    rd.SetParameterValue("ExpectedDays", contract.CR_Cas_Contract_Basic_Expected_Rental_Days.ToString());
                    
                }
                else
                {
                    rd.SetParameterValue("ExpectedDays", "     ");
                }
                // laaast 
                var account = db.CR_Cas_Account_Receipt.OrderByDescending(x=>x.CR_Cas_Account_Receipt_Date).FirstOrDefault(x => x.CR_Cas_Account_Receipt_Contract_Operation == Sno);
                if (account!=null)
                {
                    rd.SetParameterValue("ReceiptNo",account.CR_Cas_Account_Receipt_No);
                }
                else
                {
                    rd.SetParameterValue("ReceiptNo", "       ");
                }
                if (Renter.CR_Cas_Renter_Lessor_Balance != null && Renter.CR_Cas_Renter_Lessor_Balance != 0)
                {
                    rd.SetParameterValue("PreviousBalance", Renter.CR_Cas_Renter_Lessor_Balance);
                }
                else
                {
                    rd.SetParameterValue("PreviousBalance", "      ");
                }
                if (price != null)
                {
                    rd.SetParameterValue("Price", price);
                }
                else
                {
                    rd.SetParameterValue("Price", "      ");
                }
                               
                if (contract.CR_Cas_Contract_Basic_Choices_Value != null && contract.CR_Cas_Contract_Basic_Choices_Value!=0)
                {
                    rd.SetParameterValue("ChoicesTotal", contract.CR_Cas_Contract_Basic_Choices_Value.ToString());
                }
                else
                {
                    rd.SetParameterValue("ChoicesTotal", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_Additional_Driver_Value != null)
                {
                    rd.SetParameterValue("ValueAdditionalDriver", contract.CR_Cas_Contract_Basic_Additional_Driver_Value.ToString());
                }
                else
                {
                    rd.SetParameterValue("ValueAdditionalDriver", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_Additional_Value != null&&contract.CR_Cas_Contract_Basic_Additional_Value != 0)
                {
                    rd.SetParameterValue("AdditionalTotal", contract.CR_Cas_Contract_Basic_Additional_Value.ToString());
                }
                else
                {
                    rd.SetParameterValue("AdditionalTotal", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_Authorization_Value != null)
                {
                    rd.SetParameterValue("AuthorizationPercentage", contract.CR_Cas_Contract_Basic_Authorization_Value.ToString());
                }
                else
                {
                    rd.SetParameterValue("AuthorizationPercentage", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_User_Discount != null && contract.CR_Cas_Contract_Basic_User_Discount != 0)
                {
                    rd.SetParameterValue("ContractUserDiscount", contract.CR_Cas_Contract_Basic_User_Discount.ToString());
                }
                else
                {
                    rd.SetParameterValue("ContractUserDiscount", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_Discount_Value != null && contract.CR_Cas_Contract_Basic_Discount_Value != 0)
                {
                    rd.SetParameterValue("DiscountValue", contract.CR_Cas_Contract_Basic_Discount_Value.ToString());
                }
                else
                {
                    rd.SetParameterValue("DiscountValue", "       ");
                }

                if (contract.CR_Cas_Contract_Basic_Value != null)
                {
                    rd.SetParameterValue("ContractValue", contract.CR_Cas_Contract_Basic_Value.ToString());
                }
                else
                {
                    rd.SetParameterValue("ContractValue", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_After_Discount_Value != null)
                {
                    rd.SetParameterValue("ContractAfterDiscountValue", contract.CR_Cas_Contract_Basic_After_Discount_Value.ToString());
                }
                else
                {
                    rd.SetParameterValue("ContractAfterDiscountValue", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_Tax_Value != null)
                {
                    rd.SetParameterValue("TaxValue", contract.CR_Cas_Contract_Basic_Tax_Value.ToString());
                }
                else
                {
                    rd.SetParameterValue("TaxValue", "      ");
                }

                if (contract.CR_Cas_Contract_Basic_Net_Value != null)
                {
                    rd.SetParameterValue("ContractNetValue", contract.CR_Cas_Contract_Basic_Net_Value.ToString());
                }
                else
                {
                    rd.SetParameterValue("ContractNetValue", "      ");
                }

                if (totalToPay != null)
                {
                    rd.SetParameterValue("TotalToPay", totalToPay);
                }
                else
                {
                    rd.SetParameterValue("TotalToPay", "      ");
                }


                if (payedValue != null)
                {
                    rd.SetParameterValue("ContractPayedValue", payedValue);
                }
                else
                {
                    rd.SetParameterValue("ContractPayedValue", "      ");
                }


                if (contract.CR_Cas_Contract_Basic_CurrentMeters != null)
                {
                    rd.SetParameterValue("CurrentKm", contract.CR_Cas_Contract_Basic_CurrentMeters.ToString());
                }
                else
                {
                    rd.SetParameterValue("CurrentKm", "     ");
                }

                if (PayType != null || PayType != "")
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


                if (CasherName != null || CasherName != "")
                {
                    var casher = db.CR_Cas_Sup_SalesPoint.FirstOrDefault(c => c.CR_Cas_Sup_SalesPoint_Code == CasherName);
                    if (casher != null )
                    {
                        rd.SetParameterValue("CasherName", casher.CR_Cas_Sup_SalesPoint_Ar_Name.Trim());
                        var BankName = db.CR_Cas_Sup_Bank.FirstOrDefault(b => b.CR_Cas_Sup_Bank_Code == casher.CR_Cas_Sup_SalesPoint_Bank_Code);
                        if (BankName != null)
                        {
                            if (BankName.CR_Cas_Sup_Bank_Account_No!=null && BankName.CR_Cas_Sup_Bank_Account_No!="")
                            {
                                rd.SetParameterValue("AccountNo", BankName.CR_Cas_Sup_Bank_Account_No.Trim());
                            }
                            else
                            {
                                rd.SetParameterValue("AccountNo", "      ");
                            }
                            if (BankName.CR_Cas_Sup_Bank_Ar_Name != null && BankName.CR_Cas_Sup_Bank_Ar_Name != "")
                            {
                                rd.SetParameterValue("AccountName", BankName.CR_Cas_Sup_Bank_Ar_Name.Trim());
                            }
                            else
                            {
                                rd.SetParameterValue("AccountName", "      ");
                            }
                            if (BankName.CR_Mas_Sup_Bank.CR_Mas_Sup_Bank_Ar_Name != null && BankName.CR_Mas_Sup_Bank.CR_Mas_Sup_Bank_Ar_Name != "")
                            {
                                rd.SetParameterValue("BankName", BankName.CR_Mas_Sup_Bank.CR_Mas_Sup_Bank_Ar_Name.Trim());
                            }
                            else
                            {
                                rd.SetParameterValue("BankName", "    ");
                            }
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


                if (Session["UserName"].ToString()!=null)
                {
                    rd.SetParameterValue("UserName", Session["UserName"].ToString());
                }
                else
                {
                    rd.SetParameterValue("UserName", "     ");
                }
                if (lessor.CR_Mas_Com_Lessor_Signature_Ar_Director_Name != null)
                {
                    rd.SetParameterValue("DirectorName", lessor.CR_Mas_Com_Lessor_Signature_Ar_Director_Name.Trim());
                }
                else
                {
                    rd.SetParameterValue("DirectorName", "   ");
                }

                if (branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name != null)
                {
                    rd.SetParameterValue("BranchDirectorName", branch.CR_Cas_Sup_Branch_Signature_Ar_Director_Name.Trim());
                }
                else
                {
                    rd.SetParameterValue("BranchDirectorName", "    ");
                }

                if (Reasons != null)
                {
                    rd.SetParameterValue("Reasons", Reasons);
                }
                else
                {
                    rd.SetParameterValue("Reasons", "    ");
                }
                
               
            }

            rd.ExportToDisk(ExportFormatType.PortableDocFormat, fullpath);


            Task.Delay(2000).Wait();

            var firstPath  = CarPrice.CR_Cas_Contract_Basic.ToArray()[0].CR_Cas_Contract_Basic_CreateContract_Pdf;

            string[] paths = { firstPath, fullpath };
            var output     = fullpath;
            MergePDFs(paths, output + "1");
             
            var cb = db.CR_Cas_Contract_Basic.OrderByDescending(x=>x.CR_Cas_Contract_Basic_Copy).FirstOrDefault(l => l.CR_Cas_Contract_Basic_No == contract.CR_Cas_Contract_Basic_No);
            SendMail(cb);

        }

        public  void MergePDFs(string[] fileNames, string outFile)
        {
      
            using (var stream = new FileStream(outFile, FileMode.Append,  FileAccess.Write))
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
                
            }

            if (System.IO.File.Exists(outFile.Remove(outFile.Length - 1)))
            {
                System.IO.File.Delete(outFile.Remove(outFile.Length - 1));
            }

            if (System.IO.File.Exists(outFile))
            {
                System.IO.File.Move(outFile , outFile.Remove(outFile.Length - 1));
            }
        }






        private void SendMail(CR_Cas_Contract_Basic contract)
        {
            string projectFolder = Server.MapPath(string.Format("~/{0}/", "images"));
            string image1 = Path.Combine(projectFolder, "6.jpg");
            var logoo = Session["LessorLogo"].ToString();
            var log = logoo.Replace("~", "");
            log = log.Replace("/", "\\");
            log = log.Substring(1, log.Length - 1);
            var lm = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + log;

            Image image = Image.FromFile(image1);
            Image logo = Image.FromFile(lm);

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
            graphics.DrawString(contract.CR_Cas_Sup_Car_Information.CR_Cas_Sup_Car_Collect_Ar_Name.Trim(), carfont, brush, new PointF(730, 535));
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


            string htmlBody = "<html><body><h1>Contract Extension </h1><br><img src=cid:Contract style='width:100%;height:100%'></body></html>";

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


            if (contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email != null)
            {
                /*         mail.To.Add(contract.CR_Mas_Renter_Information.CR_Mas_Renter_Information_Email);*/
                mail.To.Add("bnanbnanmail@gmail.com");
            }
            mail.Subject = " Contract Extension Mail ";
            mail.Body = inline.ContentId;

            mail.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.office365.com");
            smtpClient.Port = 587;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("Bnanrent@outlook.com", "bnan123123");

            // Send the message
            smtpClient.Send(mail);

        }









        public CR_Cas_Administrative_Procedures GetLastRecord(string ProcedureCode, string sector)
        {

            DateTime year = DateTime.Now;
            var y = year.ToString("yy");
            var LessorCode = Session["LessorCode"].ToString();

            var Lrecord = db.CR_Cas_Administrative_Procedures.Where(x => x.CR_Cas_Administrative_Procedures_Lessor == LessorCode &&
                x.CR_Cas_Administrative_Procedures_Code == ProcedureCode
                && x.CR_Cas_Administrative_Procedures_Sector == sector
                && x.CR_Cas_Administrative_Procedures_Year == y)
                .Max(x => x.CR_Cas_Administrative_Procedures_No.Substring(x.CR_Cas_Administrative_Procedures_No.Length - 4, 4));

            CR_Cas_Administrative_Procedures T = new CR_Cas_Administrative_Procedures();
            if (Lrecord != null)
            {
                Int64 val = Int64.Parse(Lrecord) + 1;
                T.CR_Cas_Administrative_Procedures_No = val.ToString("0000");
            }
            else
            {
                T.CR_Cas_Administrative_Procedures_No = "0001";
            }

            return T;
        }

        // GET: ActiveContract/Edit/5
        public ActionResult Edit(string id1)
        {
            if (id1 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contractcopy = db.CR_Cas_Contract_Basic.Where(c=>c.CR_Cas_Contract_Basic_No==id1).OrderByDescending(d=>d.CR_Cas_Contract_Basic_Copy).FirstOrDefault();
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id1,contractcopy.CR_Cas_Contract_Basic_Copy);
            if (cR_Cas_Contract_Basic == null)
            {
                return HttpNotFound();
            }
            if(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Rent!=0 && cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Rent != null)
            {
                ViewBag.Price = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Daily_Rent;
            }
            else if(cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Weekly_Rent != 0 && cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Weekly_Rent != null){
                ViewBag.Price = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Weekly_Rent;
            }
            else
            {
                ViewBag.Price = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Monthly_Rent;
            }

            ViewBag.ChoicesTotal = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_Rental_Days * cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Choices_Value;
            ViewBag.AdditionalTotal = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_Value;

            ViewBag.ContractDate= string.Format("{0:yyyy/MM/dd}", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Date);
            ViewBag.ContractEndDate = string.Format("{0:yyyy/MM/dd}", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date);
            //ViewBag.OldNbrDays = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_Rental_Days;
            ViewBag.discount = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_User_Discount;
            ViewBag.PayedVal = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Payed_Value;
            ViewBag.PreviousBalance = cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Previous_Balance;

            if (cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_End_Authorization != null)
            {
                DateTime authenddate = (DateTime)cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_End_Authorization;
                DateTime ContractEndDate = (DateTime)cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Expected_End_Date;
                var diff = (authenddate - ContractEndDate).TotalDays;

                ViewBag.MaxRentalDays = Convert.ToInt32(diff - 1);
            }

            
            if (cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_is_Additional_Driver == false)
            {
                cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Additional_Driver_Value = 0;
            }

            if (cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Use_Within_Country == true)
            {
                ViewBag.Auth = "خارجي  ينتهي في " + string.Format("{0:yyyy/MM/dd}", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_End_Authorization);
            }
            else
            {
                ViewBag.Auth = "داخلي ينتهي في "+ string.Format("{0:yyyy/MM/dd}", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_End_Authorization);
            }

            ViewBag.PayType = new SelectList(db.CR_Mas_Sup_Payment_Method.Where(p => p.CR_Mas_Sup_Payment_Method_Type == "1" && p.CR_Mas_Sup_Payment_Method_Status == "A")
                 , "CR_Mas_Sup_Payment_Method_Code", "CR_Mas_Sup_Payment_Method_Ar_Name");

            ViewBag.CasherName = new SelectList(db.CR_Cas_Sup_SalesPoint.Where(p => p.CR_Cas_Sup_SalesPoint_Status == "A"
            && p.CR_Cas_Sup_SalesPoint_Com_Code == cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor && p.CR_Cas_Sup_SalesPoint_Brn_Code == cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Owner_Branch)
                 , "CR_Cas_Sup_SalesPoint_Code", "CR_Cas_Sup_SalesPoint_Ar_Name"); ;


            var CasUser = db.CR_Cas_Renter_Lessor.FirstOrDefault(u=>u.CR_Cas_Renter_Lessor_Id==cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id 
            && u.CR_Cas_Renter_Lessor_Code==cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor);
            if (CasUser != null)
            {
                ViewBag.PreviousBalance = CasUser.CR_Cas_Renter_Lessor_Balance;
            }


            return View(cR_Cas_Contract_Basic);
        }

        // POST: ActiveContract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CR_Cas_Contract_Basic_No,CR_Cas_Contract_Basic_Copy,CR_Cas_Contract_Basic_Year,CR_Cas_Contract_Basic_Type,CR_Cas_Contract_Basic_Lessor,CR_Cas_Contract_Basic_Sector,CR_Cas_Contract_Basic_Owner_Branch,CR_Cas_Contract_Basic_Date,CR_Cas_Contract_Basic_Time,CR_Cas_Contract_Basic_Renter_Id,CR_Cas_Contract_Basic_Car_Serail_No,CR_Cas_Contract_Basic_Driver_Id,CR_Cas_Contract_Basic_Additional_Driver_Id,CR_Cas_Contract_Basic_Start_Date,CR_Cas_Contract_Basic_Start_Time,CR_Cas_Contract_Basic_Expected_End_Date,CR_Cas_Contract_Basic_Expected_End_Time,CR_Cas_Contract_Basic_Expected_Rental_Days,CR_Cas_Contract_Basic_is_Renter_Driver,CR_Cas_Contract_Basic_is_Additional_Driver,CR_Cas_Contract_Basic_Additional_Driver_Value,CR_Cas_Contract_Basic_Authorization_Value,CR_Cas_Contract_Basic_Use_Within_Country,CR_Cas_Contract_Basic_Daily_Rent,CR_Cas_Contract_Basic_Weekly_Rent,CR_Cas_Contract_Basic_Monthly_Rent,CR_Cas_Contract_Basic_Additional_Value,CR_Cas_Contract_Basic_Choices_Value,CR_Cas_Contract_Basic_Tax_Rate,CR_Cas_Contract_Basic_User_Discount,CR_Cas_Contract_Basic_Daily_Free_KM,CR_Cas_Contract_Basic_Additional_KM_Value,CR_Cas_Contract_Basic_Free_Additional_Hours,CR_Cas_Contract_Basic_Hour_Max,CR_Cas_Contract_Basic_Extra_Hour_Value,CR_Cas_Contract_Basic_is_Km_Open,CR_Cas_Contract_Basic_is_Receiving_Branch,CR_Cas_Contract_Basic_Value,CR_Cas_Contract_Basic_Discount_Value,CR_Cas_Contract_Basic_After_Discount_Value,CR_Cas_Contract_Basic_Tax_Value,CR_Cas_Contract_Basic_Net_Value,CR_Cas_Contract_Basic_Payed_Value,CR_Cas_Contract_Basic_User_Insert,CR_Cas_Contract_Basic_Car_Price_Basic_No,CR_Cas_Contract_Basic_Status")] CR_Cas_Contract_Basic cR_Cas_Contract_Basic)
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

        // GET: ActiveContract/Delete/5
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

        // POST: ActiveContract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
            db.CR_Cas_Contract_Basic.Remove(cR_Cas_Contract_Basic);
            db.SaveChanges();
            return RedirectToAction("Index");
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
            if (paycode == "10")
            {
                SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(x => x.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && x.CR_Cas_Sup_SalesPoint_Brn_Code == BranchCode &&
                x.CR_Cas_Sup_SalesPoint_Bank_Code == Code).ToList();
            }
            else if (paycode != "" && paycode != null)
            {
                SalesPoint = db.CR_Cas_Sup_SalesPoint.Where(x => x.CR_Cas_Sup_SalesPoint_Com_Code == LessorCode && x.CR_Cas_Sup_SalesPoint_Brn_Code == BranchCode &&
               x.CR_Cas_Sup_SalesPoint_Bank_Code != Code).ToList();
            }
            else
            {
                SalesPoint = null;
            }


            return Json(SalesPoint, JsonRequestBehavior.AllowGet);
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
