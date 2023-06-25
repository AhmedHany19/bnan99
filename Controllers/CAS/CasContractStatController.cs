using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentCar.Models;

namespace RentCar.Controllers
{
    public class CasContractStatController : Controller
    {
        private RentCarDBEntities db = new RentCarDBEntities();

        // GET: CasContractStat
        public ActionResult Index()
        {
            var cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic
                .Include(c => c.CR_Cas_Car_Price_Basic)
                .Include(c => c.CR_Cas_Sup_Car_Information)
                .Include(c => c.CR_Mas_Com_Lessor)
                .Include(c => c.CR_Mas_Renter_Information)
                .Include(c => c.CR_Mas_Sup_Sector);
            return View(cR_Cas_Contract_Basic.ToList());
        }


        public PartialViewResult PartialContractStatus()
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
                var contracts = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Status != "U" && c.CR_Cas_Contract_Basic_Status != "y");
                if (contracts != null)
                {
                    ViewBag.NbrTot = contracts.Count();
                }

                ViewBag.A = contracts.Where(c => c.CR_Cas_Contract_Basic_Status == "A").Count();
                ViewBag.E = contracts.Where(c => c.CR_Cas_Contract_Basic_Status == "E").Count();
                ViewBag.C = contracts.Where(c => c.CR_Cas_Contract_Basic_Status == "C").Count();
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }


            return PartialView();
        }
        public PartialViewResult PartialContractStat()
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
                var contracts = db.CR_Cas_Contract_Basic.Where(c=>c.CR_Cas_Contract_Basic_Lessor==LessorCode && c.CR_Cas_Contract_Basic_Status != "U" && c.CR_Cas_Contract_Basic_Status != "y");
                if (contracts != null)
                {
                    ViewBag.NbrTot = contracts.Count();
                }

                ViewBag.Makkah = contracts.Where(c=>c.CR_Cas_Contract_Basic_Statistics_Regions_Branch=="11").Count();
                ViewBag.Madinah = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "12").Count();
                ViewBag.Riyadh = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "13").Count();
                ViewBag.Kassim = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "14").Count();
                ViewBag.Charguiyah = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "15").Count();
                ViewBag.Assir = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "16").Count();
                ViewBag.Tabuk = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "17").Count();
                ViewBag.Hael = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "18").Count();
                ViewBag.North = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "19").Count();
                ViewBag.Jazzan = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "20").Count();
                ViewBag.Najran = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "21").Count();
                ViewBag.Baha = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "22").Count();
                ViewBag.Jawf = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Regions_Branch == "23").Count();
            }
            catch
            {
                RedirectToAction("Login", "Account");
            }

            
            return PartialView();
        }
        
        public PartialViewResult PartialContractStatCity()
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            Dictionary<string, string> Data = new Dictionary<string, string>();
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }


                var contracts = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Status != "U" && c.CR_Cas_Contract_Basic_Status != "y");
                if (contracts != null)
                {
                    ViewBag.NbrTot = contracts.Count();
                }

                var city = db.CR_Mas_Sup_City.Where(c => c.CR_Mas_Sup_City_Status == "A");
                foreach (var ct in city)
                {
                    Data.Add(ct.CR_Mas_Sup_City_Ar_Name, contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_City_Branch == ct.CR_Mas_Sup_City_Code && c.CR_Cas_Contract_Basic_Lessor==LessorCode).Count().ToString());

                }

            }
            catch
            {
                RedirectToAction("Login", "Account");
            }


            return PartialView(Data);
        }


        public PartialViewResult PartialContractStatBranch()
        {
            var LessorCode = "";
            var UserLogin = "";
            var BranchCode = "";
            Dictionary<string, string> Data = new Dictionary<string, string>();
            try
            {
                LessorCode = Session["LessorCode"].ToString();
                BranchCode = Session["BranchCode"].ToString();

                UserLogin = System.Web.HttpContext.Current.Session["UserLogin"].ToString();
                if (UserLogin == null || LessorCode == null || BranchCode == null)
                {
                    RedirectToAction("Account", "Login");
                }


                var contracts = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Status != "U" && c.CR_Cas_Contract_Basic_Status != "y");
                if (contracts != null)
                {
                    ViewBag.NbrTot = contracts.Count();
                }

                var branch = db.CR_Cas_Sup_Branch.Where(c => c.CR_Cas_Sup_Branch_Status == "A" && c.CR_Cas_Sup_Lessor_Code ==LessorCode);
                foreach (var br in branch)
                {
                    Data.Add(br.CR_Cas_Sup_Branch_Ar_Name, contracts.Where(c => c.CR_Cas_Contract_Basic_Owner_Branch == br.CR_Cas_Sup_Branch_Code && br.CR_Cas_Sup_Lessor_Code==LessorCode).Count().ToString());

                }

            }
            catch
            {
                RedirectToAction("Login", "Account");
            }


            return PartialView(Data);
        }


        public PartialViewResult PartialContractStatDays()
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


                var contracts = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Status != "U" && c.CR_Cas_Contract_Basic_Status != "y");
                if (contracts != null)
                {
                    ViewBag.NbrTot = contracts.Count();
                    ViewBag.Saturday = contracts.Where(c=>c.CR_Cas_Contract_Basic_Statistics_Day_No==1).Count();
                    ViewBag.Sunday = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_No == 2).Count();
                    ViewBag.Monday = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_No == 3).Count();
                    ViewBag.Tuesday = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_No == 4).Count();
                    ViewBag.Wednesday = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_No == 5).Count();
                    ViewBag.Thursday = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_No == 6).Count();
                    ViewBag.Friday = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_No == 7).Count();
                }

               

            }
            catch
            {
                RedirectToAction("Login", "Account");
            }


            return PartialView();
        }


        public PartialViewResult PartialContractStatPeriod()
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


                var contracts = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Status != "U" && c.CR_Cas_Contract_Basic_Status != "y");
                if (contracts != null)
                {
                    ViewBag.NbrTot = contracts.Count();
                    ViewBag.P1 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_Count == 1).Count();
                    ViewBag.P2 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_Count == 2).Count();
                    ViewBag.P3 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_Count == 3).Count();
                    ViewBag.P4 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_Count == 4).Count();
                    ViewBag.P5 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_Count == 5).Count();
                    ViewBag.P6 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_Count == 6).Count();
                    ViewBag.P7 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Day_Count == 7).Count();
                }



            }
            catch
            {
                RedirectToAction("Login", "Account");
            }


            return PartialView();
        }


        public PartialViewResult PartialContractStatKm()
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


                var contracts = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Status != "U" && c.CR_Cas_Contract_Basic_Status != "y");
                if (contracts != null)
                {
                    ViewBag.NbrTot = contracts.Count();
                    ViewBag.D1 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_KM == 1).Count();
                    ViewBag.D2 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_KM == 2).Count();
                    ViewBag.D3 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_KM == 3).Count();
                    ViewBag.D4 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_KM == 4).Count();
                    ViewBag.D5 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_KM == 5).Count();
                }



            }
            catch
            {
                RedirectToAction("Login", "Account");
            }


            return PartialView();
        }

        public PartialViewResult PartialContractStatAge()
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


                var contracts = db.CR_Cas_Contract_Basic.Where(c => c.CR_Cas_Contract_Basic_Lessor == LessorCode && c.CR_Cas_Contract_Basic_Status != "U" && c.CR_Cas_Contract_Basic_Status != "y");
                if (contracts != null)
                {
                    ViewBag.NbrTot = contracts.Count();
                    ViewBag.A1 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Age_No == 1).Count();
                    ViewBag.A2 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Age_No == 2).Count();
                    ViewBag.A3 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Age_No == 3).Count();
                    ViewBag.A4 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Age_No == 4).Count();
                    ViewBag.A5 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Age_No == 5).Count();
                    ViewBag.A6 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Age_No == 6).Count();
                    ViewBag.A7 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Age_No == 7).Count();
                    ViewBag.A8 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Age_No == 8).Count();
                    ViewBag.A9 = contracts.Where(c => c.CR_Cas_Contract_Basic_Statistics_Age_No == 9).Count();
                }



            }
            catch
            {
                RedirectToAction("Login", "Account");
            }


            return PartialView();
        }


        //// GET: CasContractStat/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
        //    if (cR_Cas_Contract_Basic == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cR_Cas_Contract_Basic);
        //}

        //// GET: CasContractStat/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CR_Cas_Contract_Basic_Car_Price_Basic_No = new SelectList(db.CR_Cas_Car_Price_Basic, "CR_Cas_Car_Price_Basic_No", "CR_Cas_Car_Price_Basic_Year");
        //    ViewBag.CR_Cas_Contract_Basic_Car_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code");
        //    ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Long_Name");
        //    ViewBag.CR_Cas_Contract_Basic_Renter_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector");
        //    ViewBag.CR_Cas_Contract_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name");
        //    return View();
        //}

        //// POST: CasContractStat/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "CR_Cas_Contract_Basic_No,CR_Cas_Contract_Basic_Copy,CR_Cas_Contract_Basic_Year,CR_Cas_Contract_Basic_Type,CR_Cas_Contract_Basic_Lessor,CR_Cas_Contract_Basic_Sector,CR_Cas_Contract_Basic_Owner_Branch,CR_Cas_Contract_Basic_Date,CR_Cas_Contract_Basic_Time,CR_Cas_Contract_Basic_Renter_Id,CR_Cas_Contract_Basic_is_Renter_Driver,CR_Cas_Contract_Basic_Driver_Id,CR_Cas_Contract_Basic_is_Km_Open,CR_Cas_Contract_Basic_is_Insurance,CR_Cas_Contract_Basic_is_Receiving_Branch,CR_Cas_Contract_Basic_is_Receiving_Renter,CR_Cas_Contract_Basic_Car_Serail_No,CR_Cas_Contract_Basic_is_Additional_Driver,CR_Cas_Contract_Basic_Additional_Driver_Id,CR_Cas_Contract_Basic_Start_Date,CR_Cas_Contract_Basic_Start_Time,CR_Cas_Contract_Basic_Expected_End_Date,CR_Cas_Contract_Basic_Expected_End_Time,CR_Cas_Contract_Basic_Expected_Rental_Days,CR_Cas_Contract_Basic_Additional_Driver_Value,CR_Cas_Contract_Basic_Authorization_Value,CR_Cas_Contract_Basic_Use_Within_Country,CR_Cas_Contract_Basic_End_Authorization,CR_Cas_Contract_BasicAuthorization_Staus,CR_Cas_Contract_BasicAuthorization_No,CR_Cas_Contract_Basic_Daily_Rent,CR_Cas_Contract_Basic_Weekly_Rent,CR_Cas_Contract_Basic_Monthly_Rent,CR_Cas_Contract_Basic_CurrentMeters,CR_Cas_Contract_Basic_Additional_Value,CR_Cas_Contract_Basic_Choices_Value,CR_Cas_Contract_Basic_Tax_Rate,CR_Cas_Contract_Basic_User_Discount,CR_Cas_Contract_Basic_Daily_Free_KM,CR_Cas_Contract_Basic_Additional_KM_Value,CR_Cas_Contract_Basic_Free_Additional_Hours,CR_Cas_Contract_Basic_Hour_Max,CR_Cas_Contract_Basic_Extra_Hour_Value,CR_Cas_Contract_Basic_Value,CR_Cas_Contract_Basic_Discount_Value,CR_Cas_Contract_Basic_After_Discount_Value,CR_Cas_Contract_Basic_Tax_Value,CR_Cas_Contract_Basic_Net_Value,CR_Cas_Contract_Basic_Payed_Value,CR_Cas_Contract_Basic_Previous_Balance,CR_Cas_Contract_Basic_Hour_DateTime_Alert,CR_Cas_Contract_Basic_Hour_MsgNo,CR_Cas_Contract_Basic_Day_DateTime_Alert,CR_Cas_Contract_Basic_Day_MsgNo,CR_Cas_Contract_Basic_EndContract_MsgNo,CR_Cas_Contract_Basic_Alert_Status,CR_Cas_Contract_Basic_Car_Price_Basic_No,CR_Cas_Contract_Basic_Status,CR_Cas_Contract_Basic_Receiving_Branch,CR_Cas_Contract_Basic_Actual_Total_KM,CR_Cas_Contract_Basic_Delivery_Reading_KM,CR_Cas_Contract_Basic_Actual_Additional_Free_KM,CR_Cas_Contract_Basic_Additional_Hours,CR_Cas_Contract_Basic_Actual_Extra_KM_Value,CR_Cas_Contract_Basic_Actual_Extra_Hour_Value,CR_Cas_Contract_Basic_Contarct_is_Expenses,CR_Cas_Contract_Basic_Contarct_Expenses_Value,CR_Cas_Contract_Basic_Contarct_Expenses_Description,CR_Cas_Contract_Basic_Contarct_is_Compensation,CR_Cas_Contract_Basic_Contarct_Compensation_Value,CR_Cas_Contract_Basic_Contarct_Compensation_Description,CR_Cas_Contract_Basic_Close_Previous_Balance,CR_Cas_Contract_Basic_Contarct_Remaining_Amount,CR_Cas_Contract_Basic_Statistics_Nationalities,CR_Cas_Contract_Basic_Statistics_Country,CR_Cas_Contract_Basic_Statistics_Gender,CR_Cas_Contract_Basic_Statistics_Jobs,CR_Cas_Contract_Basic_Statistics_Regions_Branch,CR_Cas_Contract_Basic_Statistics_City_Branch,CR_Cas_Contract_Basic_Statistics_Regions_Renter,CR_Cas_Contract_Basic_Statistics_City_Renter,CR_Cas_Contract_Basic_Statistics_Brand,CR_Cas_Contract_Basic_Statistics_Model,CR_Cas_Contract_Basic_Statistics_Year,CR_Cas_Contract_Basic_Statistics_Membership_Code,CR_Cas_Contract_Basic_Statistics_Day_No,CR_Cas_Contract_Basic_Statistics_Time_No,CR_Cas_Contract_Basic_Statistics_Day_Count,CR_Cas_Contract_Basic_Statistics_Age_No,CR_Cas_Contract_Basic_Statistics_Value_No,CR_Cas_Contract_Basic_Statistics_KM,CR_Cas_Contract_Basic_CreateContract_Pdf,CR_Cas_Contract_Basic_CloseContract_Pdf,CR_Cas_Contract_Basic_CreateTGA_Pdf,CR_Cas_Contract_Basic_CloseTGA_Pdf,CR_Cas_Contract_Basic_User_Insert,CR_Cas_Contract_Basic_User_Close")] CR_Cas_Contract_Basic cR_Cas_Contract_Basic)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CR_Cas_Contract_Basic.Add(cR_Cas_Contract_Basic);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CR_Cas_Contract_Basic_Car_Price_Basic_No = new SelectList(db.CR_Cas_Car_Price_Basic, "CR_Cas_Car_Price_Basic_No", "CR_Cas_Car_Price_Basic_Year", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Price_Basic_No);
        //    ViewBag.CR_Cas_Contract_Basic_Car_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Serail_No);
        //    ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Long_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor);
        //    ViewBag.CR_Cas_Contract_Basic_Renter_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id);
        //    ViewBag.CR_Cas_Contract_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Sector);
        //    return View(cR_Cas_Contract_Basic);
        //}

        //// GET: CasContractStat/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
        //    if (cR_Cas_Contract_Basic == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CR_Cas_Contract_Basic_Car_Price_Basic_No = new SelectList(db.CR_Cas_Car_Price_Basic, "CR_Cas_Car_Price_Basic_No", "CR_Cas_Car_Price_Basic_Year", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Price_Basic_No);
        //    ViewBag.CR_Cas_Contract_Basic_Car_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Serail_No);
        //    ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Long_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor);
        //    ViewBag.CR_Cas_Contract_Basic_Renter_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id);
        //    ViewBag.CR_Cas_Contract_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Sector);
        //    return View(cR_Cas_Contract_Basic);
        //}

        //// POST: CasContractStat/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "CR_Cas_Contract_Basic_No,CR_Cas_Contract_Basic_Copy,CR_Cas_Contract_Basic_Year,CR_Cas_Contract_Basic_Type,CR_Cas_Contract_Basic_Lessor,CR_Cas_Contract_Basic_Sector,CR_Cas_Contract_Basic_Owner_Branch,CR_Cas_Contract_Basic_Date,CR_Cas_Contract_Basic_Time,CR_Cas_Contract_Basic_Renter_Id,CR_Cas_Contract_Basic_is_Renter_Driver,CR_Cas_Contract_Basic_Driver_Id,CR_Cas_Contract_Basic_is_Km_Open,CR_Cas_Contract_Basic_is_Insurance,CR_Cas_Contract_Basic_is_Receiving_Branch,CR_Cas_Contract_Basic_is_Receiving_Renter,CR_Cas_Contract_Basic_Car_Serail_No,CR_Cas_Contract_Basic_is_Additional_Driver,CR_Cas_Contract_Basic_Additional_Driver_Id,CR_Cas_Contract_Basic_Start_Date,CR_Cas_Contract_Basic_Start_Time,CR_Cas_Contract_Basic_Expected_End_Date,CR_Cas_Contract_Basic_Expected_End_Time,CR_Cas_Contract_Basic_Expected_Rental_Days,CR_Cas_Contract_Basic_Additional_Driver_Value,CR_Cas_Contract_Basic_Authorization_Value,CR_Cas_Contract_Basic_Use_Within_Country,CR_Cas_Contract_Basic_End_Authorization,CR_Cas_Contract_BasicAuthorization_Staus,CR_Cas_Contract_BasicAuthorization_No,CR_Cas_Contract_Basic_Daily_Rent,CR_Cas_Contract_Basic_Weekly_Rent,CR_Cas_Contract_Basic_Monthly_Rent,CR_Cas_Contract_Basic_CurrentMeters,CR_Cas_Contract_Basic_Additional_Value,CR_Cas_Contract_Basic_Choices_Value,CR_Cas_Contract_Basic_Tax_Rate,CR_Cas_Contract_Basic_User_Discount,CR_Cas_Contract_Basic_Daily_Free_KM,CR_Cas_Contract_Basic_Additional_KM_Value,CR_Cas_Contract_Basic_Free_Additional_Hours,CR_Cas_Contract_Basic_Hour_Max,CR_Cas_Contract_Basic_Extra_Hour_Value,CR_Cas_Contract_Basic_Value,CR_Cas_Contract_Basic_Discount_Value,CR_Cas_Contract_Basic_After_Discount_Value,CR_Cas_Contract_Basic_Tax_Value,CR_Cas_Contract_Basic_Net_Value,CR_Cas_Contract_Basic_Payed_Value,CR_Cas_Contract_Basic_Previous_Balance,CR_Cas_Contract_Basic_Hour_DateTime_Alert,CR_Cas_Contract_Basic_Hour_MsgNo,CR_Cas_Contract_Basic_Day_DateTime_Alert,CR_Cas_Contract_Basic_Day_MsgNo,CR_Cas_Contract_Basic_EndContract_MsgNo,CR_Cas_Contract_Basic_Alert_Status,CR_Cas_Contract_Basic_Car_Price_Basic_No,CR_Cas_Contract_Basic_Status,CR_Cas_Contract_Basic_Receiving_Branch,CR_Cas_Contract_Basic_Actual_Total_KM,CR_Cas_Contract_Basic_Delivery_Reading_KM,CR_Cas_Contract_Basic_Actual_Additional_Free_KM,CR_Cas_Contract_Basic_Additional_Hours,CR_Cas_Contract_Basic_Actual_Extra_KM_Value,CR_Cas_Contract_Basic_Actual_Extra_Hour_Value,CR_Cas_Contract_Basic_Contarct_is_Expenses,CR_Cas_Contract_Basic_Contarct_Expenses_Value,CR_Cas_Contract_Basic_Contarct_Expenses_Description,CR_Cas_Contract_Basic_Contarct_is_Compensation,CR_Cas_Contract_Basic_Contarct_Compensation_Value,CR_Cas_Contract_Basic_Contarct_Compensation_Description,CR_Cas_Contract_Basic_Close_Previous_Balance,CR_Cas_Contract_Basic_Contarct_Remaining_Amount,CR_Cas_Contract_Basic_Statistics_Nationalities,CR_Cas_Contract_Basic_Statistics_Country,CR_Cas_Contract_Basic_Statistics_Gender,CR_Cas_Contract_Basic_Statistics_Jobs,CR_Cas_Contract_Basic_Statistics_Regions_Branch,CR_Cas_Contract_Basic_Statistics_City_Branch,CR_Cas_Contract_Basic_Statistics_Regions_Renter,CR_Cas_Contract_Basic_Statistics_City_Renter,CR_Cas_Contract_Basic_Statistics_Brand,CR_Cas_Contract_Basic_Statistics_Model,CR_Cas_Contract_Basic_Statistics_Year,CR_Cas_Contract_Basic_Statistics_Membership_Code,CR_Cas_Contract_Basic_Statistics_Day_No,CR_Cas_Contract_Basic_Statistics_Time_No,CR_Cas_Contract_Basic_Statistics_Day_Count,CR_Cas_Contract_Basic_Statistics_Age_No,CR_Cas_Contract_Basic_Statistics_Value_No,CR_Cas_Contract_Basic_Statistics_KM,CR_Cas_Contract_Basic_CreateContract_Pdf,CR_Cas_Contract_Basic_CloseContract_Pdf,CR_Cas_Contract_Basic_CreateTGA_Pdf,CR_Cas_Contract_Basic_CloseTGA_Pdf,CR_Cas_Contract_Basic_User_Insert,CR_Cas_Contract_Basic_User_Close")] CR_Cas_Contract_Basic cR_Cas_Contract_Basic)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(cR_Cas_Contract_Basic).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CR_Cas_Contract_Basic_Car_Price_Basic_No = new SelectList(db.CR_Cas_Car_Price_Basic, "CR_Cas_Car_Price_Basic_No", "CR_Cas_Car_Price_Basic_Year", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Price_Basic_No);
        //    ViewBag.CR_Cas_Contract_Basic_Car_Serail_No = new SelectList(db.CR_Cas_Sup_Car_Information, "CR_Cas_Sup_Car_Serail_No", "CR_Cas_Sup_Car_Lessor_Code", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Car_Serail_No);
        //    ViewBag.CR_Cas_Contract_Basic_Lessor = new SelectList(db.CR_Mas_Com_Lessor, "CR_Mas_Com_Lessor_Code", "CR_Mas_Com_Lessor_Ar_Long_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Lessor);
        //    ViewBag.CR_Cas_Contract_Basic_Renter_Id = new SelectList(db.CR_Mas_Renter_Information, "CR_Mas_Renter_Information_Id", "CR_Mas_Renter_Information_Sector", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Renter_Id);
        //    ViewBag.CR_Cas_Contract_Basic_Sector = new SelectList(db.CR_Mas_Sup_Sector, "CR_Mas_Sup_Sector_Code", "CR_Mas_Sup_Sector_Ar_Name", cR_Cas_Contract_Basic.CR_Cas_Contract_Basic_Sector);
        //    return View(cR_Cas_Contract_Basic);
        //}

        //// GET: CasContractStat/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
        //    if (cR_Cas_Contract_Basic == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(cR_Cas_Contract_Basic);
        //}

        //// POST: CasContractStat/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    CR_Cas_Contract_Basic cR_Cas_Contract_Basic = db.CR_Cas_Contract_Basic.Find(id);
        //    db.CR_Cas_Contract_Basic.Remove(cR_Cas_Contract_Basic);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
